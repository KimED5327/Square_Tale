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

    int _questID = 0;                   // 현재 퀘스트 다이얼로그가 진행되는 퀘스트 ID
    int _lineIdx = 0;                   // 대화 상자에 출력되는 대사의 index 값 
    bool _isTalking = false;            // 현재 대화중인지 확인하는 변수 
    public QuestState _state;           // 현재 퀘스트 다이얼로그가 진행되는 퀘스트의 진행상태 
    string _questTitle;                 // 퀘스트 제목 
    QuestNPC _questNPC;                 // 현재 대화상대인 NPC 참조값 
    QuestCompleteUI _questCompleteUI;   // 퀘스트 완료 팝업 UI 참조값 
    Tutorial _tutorial;                 // 튜토리얼 진행을 위한 튜토리얼 캐싱.

    string _keywordEffectName = "키워드 획득";

    // UI 관련 변수 
    Transform _npcTransform;
    Transform _player;
    public float _delayBeforeGettingKeyword = 2f;    // 퀘스트 완료 시 키워드 보상 획득 딜레이 

    [Header("Panel UI")]
    GameObject _hudCanvas;
    [Tooltip("기본 다이얼로그 Panel")]
    [SerializeField] GameObject _dialoguePanel;
    [Tooltip("퀘스트 다이얼로그 Panel")]
    [SerializeField] GameObject _questDialoguePanel;
    [Tooltip("퀘스트 수락 Panel")]
    [SerializeField] GameObject _questAcceptedPanel;

    [Header("Quest Accept UI")]
    [Tooltip("퀘스트 수락 Panel 내 퀘스트 Title")]
    [SerializeField] Text _questAcceptedTitle;

    [Header("Quest Dialogue UI")]
    [Tooltip("퀘스트 다이얼로그 Panel 내 퀘스트 Title")]
    [SerializeField] Text _txtQuestTitle;
    [Tooltip("퀘스트 다이얼로그 Panel 내 화자 이름")]
    [SerializeField] Text _txtName;
    [Tooltip("퀘스트 다이얼로그 Panel 내 대사")]
    [SerializeField] Text _txtLines;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } 
    }

    private void OnEnable()
    {
        _player = FindObjectOfType<PlayerMove>().transform;
        _hudCanvas = FindObjectOfType<GameHudMenu>().gameObject;
        _tutorial = FindObjectOfType<Tutorial>();
        _questCompleteUI = FindObjectOfType<QuestCompleteUI>();
    }

    /// <summary>
    /// 퀘스트 수락, 완료 시의 대화 실행 
    /// </summary>
    /// <param name="questID"></param>
    public void DoQuestDialouge()
    {
        GetLine(_questID);
        _questDialoguePanel.SetActive(_isTalking);
        SoundManager.instance.PlayEffectSound("Click", 1f);

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
        _dialoguePanel.SetActive(false);
        _questNPC.GetComponent<ZoomNPC>().ZoomOutNPC();
        _questNPC.GetComponent<Transform>().tag = "QuestNPC";
        _questNPC.TurnOnNameTag();
    }

    /// <summary>
    /// 퀘스트 다이얼로그 Panel 닫기 
    /// </summary>
    public void CloseQuestDialoguePanel()
    {
        _questDialoguePanel.SetActive(false);
        _questNPC.GetComponent<ZoomNPC>().ZoomOutNPC();
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
        //line.Substring(0,MinMaxCount++)

        LineUnit lineUnit = QuestDialogueDB.instance.GetDialogue(questID).GetLineUnit(_state, _lineIdx);

        // 다음 대사가 없을 경우 대사인덱스 초기화 및 대화 종료 
        if (lineUnit == null)
        {
            _isTalking = false;
            _lineIdx = 0;
            return;
        }

        string line = lineUnit.GetLine();

        // 대사의 화자(유저/NPC)에 맞게 이름 설정 
        if (lineUnit.GetNpcID() == 0)
        {
            string name = PlayerPrefs.GetString("Nickname");

            if (!JsonManager.instance.IsNullString(name)) _txtName.text = name;
            else _txtName.text = "플레이어";
        }
        else
        {
            _txtName.text = NpcDB.instance.GetNPC(lineUnit.GetNpcID()).GetName();
        }

        _txtLines.text = line;
        _isTalking = true;
        _lineIdx++;
    }

    /// <summary>
    /// 퀘스트 수락에 따라 팝업메뉴을 실행하고, 퀘스트 매니져에 진행중인 퀘스트 추가 및 NPC 상태값 변경 
    /// </summary>
    void AcceptQuest()
    {
        // 수락한 퀘스트를 퀘스트 매니져의 진행중인 퀘스트 리스트에 추가 
        Quest questAccepted = QuestDB.instance.GetQuest(_questID).DeepCopy();
        QuestManager.instance.AddOngoingQuest(questAccepted);

        // 튜토리얼 퀘스트일 경우 대화창만 비활성화 
        if (questAccepted.GetQuestType() == QuestType.TYPE_TUTORIAL)
        {
            CloseQuestDialoguePanel();

            // 튜토리얼 시작하기 
            StartCoroutine(TutorialDelayCall());
            return;
        }

        // 퀘스트 수락 팝업메뉴 실행 
        SetQuestAcceptPanel();
        _questAcceptedPanel.SetActive(true);
        CloseQuestDialoguePanel();
        SoundManager.instance.PlayEffectSound("Quest_Accept", 1f);
    }

    // 1초 대기 후 튜토리얼 등장하게 코루틴 추가
    IEnumerator TutorialDelayCall()
    {
        yield return new WaitForSeconds(1f);

        _tutorial.CallTutorial(TutorialType.HUD);
    }

    /// <summary>
    /// 퀘스트 완료에 따라 팝업메뉴을 실행하고, 퀘스트 매니져에 완료된 퀘스트 추가 및 NPC 상태값 변경
    /// </summary>
    public void CompleteQuest()
    {
        // 키워드 보상이 있는 경우 일정 딜레이 후 키워드 획득 
        if (QuestDB.instance.GetQuest(_questID).GetKeywordList().Count > 0)
        {
            StartCoroutine("GetKeyword");
        }

        // 완료된 퀘스트를 퀘스트 매니져의 완료된 퀘스트 리스트에 추가 
        Quest questCompleted = QuestManager.instance.GetOngoingQuestByID(_questID).DeepCopy();
        QuestManager.instance.AddFinishedQuest(questCompleted);

        // 완료된 퀘스트를 퀘스트 매니져의 진행중인 퀘스트 리스트에서 삭제
        QuestManager.instance.DeleteOngoingQuest();

        // 퀘스트 완료 팝업메뉴 실행 
        _questCompleteUI.OpenQuestCompletePanel(questCompleted);
        if (_questDialoguePanel.activeInHierarchy) CloseQuestDialoguePanel();
        SoundManager.instance.PlayEffectSound("Quest_Complete", 0.9f);
    }

    /// <summary>
    /// 퀘스트 수락 Panel의 UI 데이터 값 설정 
    /// </summary>
    public void SetQuestAcceptPanel()
    {
        _questAcceptedTitle.text = "'" + _questTitle + "'";
    }

    /// <summary>
    /// 일정시간의 딜레이 후 키워드 획득 
    /// </summary>
    /// <returns></returns>
    IEnumerator GetKeyword()
    {
        yield return new WaitForSeconds(_delayBeforeGettingKeyword);

        ObjectPooling.instance.GetObjectFromPool(_keywordEffectName, _player.position);
        KeywordData.instance.AcquireKeyword(QuestDB.instance.GetQuest(_questID).GetKeywordList()[0]);
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
