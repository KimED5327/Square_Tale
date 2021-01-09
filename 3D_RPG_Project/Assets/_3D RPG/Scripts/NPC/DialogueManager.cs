using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// NPC의 퀘스트 다이얼로그를 수행하고, 다이얼로그 창 닫기 등 대화 관련 액션을 수행하는 매니져
/// </summary>
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    int _questID = 0;               // 현재 퀘스트 다이얼로그가 진행되는 퀘스트 ID
    int _lineIdx = 0;               // 대화 상자에 출력되는 대사의 index 값 
    bool _isTalking = false;        // 현재 대화중인지 확인하는 변수 
    public QuestState _state;       // 현재 퀘스트 다이얼로그가 진행되는 퀘스트의 진행상태 
    QuestNPC _questNPC;             // 현재 대화상대인 NPC 참조값 
    string _questTitle;             // 퀘스트 제목 
    string questInfoKey = "info";   // 해시테이블 QuestInfo 키  

    string _keywordEffectName = "키워드 획득";

    // UI 관련 변수 
    Transform _npcTransform;
    Transform _player;
    float _delayBeforeGettingKeyword = 1.7f;    // 퀘스트 완료 시 키워드 보상 획득 딜레이 

    [Header("Panel UI")]
    GameObject _hudCanvas;
    [Tooltip("기본 다이얼로그 Panel")]
    [SerializeField] GameObject _dialoguePanel;
    [Tooltip("퀘스트 다이얼로그 Panel")]
    [SerializeField] GameObject _questDialoguePanel;
    [Tooltip("퀘스트 수락 Panel")]
    [SerializeField] GameObject _questAcceptedPanel;
    [Tooltip("퀘스트 완료 Panel")]
    [SerializeField] GameObject _questCompletedPanel;

    [Header("Quest Accept UI")]
    [Tooltip("퀘스트 수락 Panel 내 퀘스트 Title")]
    [SerializeField] Text _questAcceptedTitle;

    [Header("Quest Complete UI")]
    [Tooltip("퀘스트 완료 Panel 내 퀘스트 Title")]
    [SerializeField] Text _questCompletedTitle;

    [Header("Quest Dialogue UI")]
    [Tooltip("퀘스트 다이얼로그 Panel 내 퀘스트 Title")]
    [SerializeField] Text _txtQuestTitle;
    [Tooltip("퀘스트 다이얼로그 Panel 내 NPC 이름 Text")]
    [SerializeField] Text _npcName;
    [Tooltip("퀘스트 다이얼로그 Panel 내 NPC 대사 Text")]
    [SerializeField] Text _npcLines;
    [Tooltip("퀘스트 다이얼로그 Panel 내 Player 대사 Text")]
    [SerializeField] Text _playerLines;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            _player = FindObjectOfType<PlayerMove>().transform;
            _hudCanvas = FindObjectOfType<GameHudMenu>().gameObject;
        } 
    }

    /// <summary>
    /// 퀘스트 수락, 완료 시의 대화 실행 
    /// </summary>
    /// <param name="questID"></param>
    public void DoQuestDialouge()
    {
        GetLine(_questID);
        _questDialoguePanel.SetActive(_isTalking);

        // 대화가 완료된 시점에서 퀘스트 진행상태에 맞게 동작 수행 
        if (!_isTalking)
        {
            switch (_state)
            {
                // 퀘스트 수락에 따른 액션 수행  
                case QuestState.QUEST_OPENED:
                    AcceptQuest();
                    break;  
                
                // 퀘스트 완료에 따른 액션 수행 
                case QuestState.QUEST_COMPLETABLE:
                    CompleteQuest();
                    break;
            }
        }
    }

    /// <summary>
    /// 기본 다이얼로그 Panel 닫기 
    /// </summary>
    public void CloseDialoguePanel()
    {
        _player.GetComponent<CameraController>().enabled = true;
        _dialoguePanel.SetActive(false);
        _hudCanvas.SetActive(true);
        _questNPC.TurnOnNameTag();
        _questNPC.GetComponent<Transform>().tag = "QuestNPC";
    }

    /// <summary>
    /// 퀘스트 다이얼로그 Panel 닫기 
    /// </summary>
    public void CloseQuestDialoguePanel()
    {
        _player.GetComponent<CameraController>().enabled = true;
        _questDialoguePanel.SetActive(false);
        _hudCanvas.SetActive(true);
        _questNPC.TurnOnNameTag();

        // 바로 tag를 QuestNPC로 바꾸면 중복터치 문제가 생기므로 
        // 일정 딜레이 후 tag를 변경하는 코루틴을 호출하는 함수를 실행 
        _questNPC.PlaySetQuestNpcTagCoroutine();

        // 퀘스트 다이얼로그를 종료하므로 대사 번호도 0으로 변경 
        _lineIdx = 0;
    }

    /// <summary>
    /// 퀘스트 대사를 퀘스트 다이얼로그 창에 한줄씩 출력 
    /// </summary>
    /// <param name="questID"></param>
    void GetLine(int questID)
    {
        LineUnit lineUnit = QuestDialogueDB.instance.GetDialogue(questID).GetLineUnit(_state, _lineIdx);

        if (lineUnit == null)
        {
            _isTalking = false;
            _lineIdx = 0;
            return;
        }

        string line = lineUnit.GetLine();

        if (lineUnit.GetNpcID() == 0)
        {
            _npcName.gameObject.SetActive(false);
            _npcLines.gameObject.SetActive(false);
            _playerLines.gameObject.SetActive(true);
            _playerLines.text = line;
        }
        else
        {
            _npcName.gameObject.SetActive(true);
            _npcLines.gameObject.SetActive(true);
            _playerLines.gameObject.SetActive(false);
            _npcName.text = NpcDB.instance.GetNPC(lineUnit.GetNpcID()).GetName();
            _npcLines.text = line;
        }

        _isTalking = true;
        _lineIdx++;
    }

    /// <summary>
    /// 퀘스트 수락에 따라 팝업메뉴을 실행하고, 퀘스트 매니져에 진행중인 퀘스트 추가 및 NPC 상태값 변경 
    /// </summary>
    void AcceptQuest()
    {
        // 퀘스트 수락 팝업메뉴 실행 
        SetQuestAcceptPanel();
        _questAcceptedPanel.SetActive(true);
        CloseQuestDialoguePanel();

        // 퀘스트를 부여한 NPC의 상태값 변경 
        _questNPC.SetOngoingQuestID(_questID);
        _questNPC.SetQuestState(QuestState.QUEST_ONGOING);
        _questNPC.SetQuestMark();

        // 수락한 퀘스트를 퀘스트 매니져의 진행중인 퀘스트 리스트에 추가 
        Quest questAccepted = new Quest();
        questAccepted = QuestDB.instance.GetQuest(_questID);

        // 수락한 퀘스트 Type이 'NPC와 대화'일 경우, 퀘스트를 부여한 NPC의 참조값을 저장 
        if (questAccepted.GetQuestType() == QuestType.TYPE_TALKWITHNPC)
        {
            TalkWithNpc talkWithNpc = questAccepted.GetQuestInfo()[questInfoKey] as TalkWithNpc;
            talkWithNpc.SetQuestGiver(_questNPC);

            //Debug.Log("퀘스트 부여자 ID : " + talkWithNpc.GetQuestGiver().GetNpcID());
            //Debug.Log("퀘스트 완료자 ID : " + talkWithNpc.GetQuestFinisher().GetNpcID());
        }

        QuestManager.instance.AddOngoingQuest(questAccepted);
    }

    /// <summary>
    /// 퀘스트 완료에 따라 팝업메뉴을 실행하고, 퀘스트 매니져에 완료된 퀘스트 추가 및 NPC 상태값 변경
    /// </summary>
    void CompleteQuest()
    {
        // 퀘스트 완료 팝업메뉴 실행 
        SetQuestCompletePanel();
        _questCompletedPanel.SetActive(true);
        CloseQuestDialoguePanel();

        // 키워드 보상이 있는 경우 일정 딜레이 후 키워드 획득 
        if (QuestDB.instance.GetQuest(_questID).GetKeywords().Count > 0)
        {
            StartCoroutine("GetKeyword");
        }

        // 퀘스트를 완료한 NPC의 상태값 변경 
        _questNPC.SetOngoingQuestID(0);
        _questNPC.DeleteCompletedQuest(_questID);
        _questNPC.CheckAvailableQuest();
        _questNPC.SetQuestMark();

        // 완료된 퀘스트를 퀘스트 매니져의 완료된 퀘스트 리스트에 추가 
        Quest questCompleted = new Quest();
        questCompleted = QuestDB.instance.GetQuest(_questID);

        // 완료된 퀘스트 Type이 'NPC와 대화'일 경우, 퀘스트를 부여한 NPC의 상태값 변경  
        if (questCompleted.GetQuestType() == QuestType.TYPE_TALKWITHNPC)
        {
            TalkWithNpc talkWithNpc = questCompleted.GetQuestInfo()[questInfoKey] as TalkWithNpc;
            talkWithNpc.GetQuestGiver().SetOngoingQuestID(0);
            talkWithNpc.GetQuestGiver().DeleteCompletedQuest(_questID);
            talkWithNpc.GetQuestGiver().CheckAvailableQuest();
            talkWithNpc.GetQuestGiver().SetQuestMark();
        }

        QuestManager.instance.AddFinishedQuest(questCompleted);

        // 완료된 퀘스트를 퀘스트 매니져의 진행중인 퀘스트 리스트에서 삭제
        QuestManager.instance.DeleteOngoingQuest();
    }

    /// <summary>
    /// 퀘스트 수락 Panel의 UI 데이터 값 설정 
    /// </summary>
    public void SetQuestAcceptPanel()
    {
        _questAcceptedTitle.text = "'" + _questTitle + "'";
    }

    /// <summary>
    /// 퀘스트 완료 Panel의 UI 데이터 값 설정 
    /// </summary>
    public void SetQuestCompletePanel()
    {
        _questCompletedTitle.text = "'" + _questTitle + "'";
    }

    /// <summary>
    /// 일정시간의 딜레이 후 키워드 획득 
    /// </summary>
    /// <returns></returns>
    IEnumerator GetKeyword()
    {
        yield return new WaitForSeconds(_delayBeforeGettingKeyword);

        ObjectPooling.instance.GetObjectFromPool(_keywordEffectName, _player.position);
        KeywordData.instance.AcquireKeyword(QuestDB.instance.GetQuest(_questID).GetKeywords()[0]);
        _player.GetComponent<PlayerMove>().Victory();
    }

    /// <summary>
    /// 퀘스트 다이얼로그 건너뛰기 기능 수행 
    /// </summary>
    public void SkipDialogue()
    {
        // 대사의 index count 보다 큰 값을 입력하여 대사 종료 
        int lineCount = QuestDialogueDB.instance.GetDialogue(_questID).GetLinesCount(_state);
        _lineIdx = lineCount;

        DoQuestDialouge();
    }

    /// <summary>
    /// 현재 대화가 이루어질 퀘스트에 대한 정보 세팅 
    /// </summary>
    /// <param name="questID"></param>
    /// <param name="questNPC"></param>
    public void SetQuestInfo(int questID, QuestNPC questNPC)
    {
        _questID = questID;
        _questNPC = questNPC;
        _state = _questNPC.GetQuestState();
        _txtQuestTitle.text = QuestDB.instance.GetQuest(questID).GetTitle();
        _questTitle = QuestDB.instance.GetQuest(questID).GetTitle();
    }

    /// <summary>
    /// 대화상대인 퀘스트 NPC의 참조값 받아오기 
    /// </summary>
    /// <param name="questNPC"></param>
    public void SetQuestNPC(QuestNPC questNPC) { _questNPC = questNPC; }
}
