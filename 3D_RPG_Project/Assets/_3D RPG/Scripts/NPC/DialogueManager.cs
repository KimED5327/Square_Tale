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

    int _questID = 0;           // 현재 퀘스트 다이얼로그가 진행되는 퀘스트 ID
    int _lineIdx = 0;           // 대화 상자에 출력되는 대사의 index 값 
    bool _isTalking = false;    // 현재 대화중인지 확인하는 변수 
    QuestState _state;          // 현재 퀘스트 다이얼로그가 진행되는 퀘스트의 진행상태 
    QuestNPC _questNPC;         // 현재 대화상대인 NPC 참조값 

    // UI 관련 변수 
    Transform _npcTransform;
 
    [SerializeField] Transform _player;

    [Header("Panel UI")]
    [Tooltip("HUD Canvas")]
    [SerializeField] GameObject _hudCanvas;
    [Tooltip("기본 다이얼로그 Panel")]
    [SerializeField] GameObject _dialoguePanel;
    [Tooltip("퀘스트 다이얼로그 Panel")]
    [SerializeField] GameObject _questDialoguePanel;
    [Tooltip("퀘스트 수락 Panel")]
    [SerializeField] GameObject _questAcceptedPanel;

    [Header("QuestPopup UI")]
    [Tooltip("퀘스트 수락 Panel 내 퀘스트 Title")]
    [SerializeField] Text _questAcceptedTitle;

    [Header("Quest Dialogue UI")]
    [Tooltip("퀘스트 다이얼로그 Panel 내 퀘스트 Title")]
    [SerializeField] Text _questTitle;
    [Tooltip("퀘스트 다이얼로그 Panel 내 NPC 이름 Text")]
    [SerializeField] Text _npcName;
    [Tooltip("퀘스트 다이얼로그 Panel 내 NPC 대사 Text")]
    [SerializeField] Text _npcLines;
    [Tooltip("퀘스트 다이얼로그 Panel 내 Player 대사 Text")]
    [SerializeField] Text _playerLines;

    private void Awake()
    {
        if (instance == null) instance = this; 
    }

    /// <summary>
    /// 퀘스트를 수락할 때 이루어지는 대화 
    /// </summary>
    /// <param name="questID"></param>
    public void QuestOpenedDialogue()
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
    /// 퀘스트가 완료되었을 때 이루어지는 대화 
    /// </summary>
    public void QuestCompletedDialogue()
    {

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
        _questNPC.GetComponent<Transform>().tag = "QuestNPC";

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
    public void AcceptQuest()
    {
        // 퀘스트 수락 팝업메뉴 실행 
        _questAcceptedTitle.text = "'" + _questTitle.text + "'";
        _questAcceptedPanel.SetActive(true);
        CloseQuestDialoguePanel();
        //_questAcceptedPanel.GetComponent<Animator>().Play("FadeOut");

        // 수락한 퀘스트를 퀘스트 매니져의 진행중인 퀘스트 리스트에 추가 
        Quest questAccepted = new Quest();
        questAccepted = QuestDB.instance.GetQuest(_questID);
        QuestManager.instance.AddOngoingQuest(questAccepted);

        // 퀘스트를 부여한 NPC의 상태값 변경 
        _questNPC.SetOngoingQuestID(_questID);
        _questNPC.SetQuestState(QuestState.QUEST_ONGOING);
        _questNPC.SetQuestMark();
    }

    public void CompleteQuest() { }

    /// <summary>
    /// 퀘스트 다이얼로그 건너뛰기 기능 수행 
    /// </summary>
    public void SkipDialogue()
    {
        // 대사의 index count 보다 큰 값을 입력하여 대사 종료 
        int lineCount = QuestDialogueDB.instance.GetDialogue(_questID).GetLinesCount(_state);
        _lineIdx = lineCount;

        QuestOpenedDialogue();
    }

    /// <summary>
    /// 현재 대화가 이루어질 퀘스트에 대한 정보 세팅 
    /// </summary>
    /// <param name="questID"></param>
    /// <param name="state"></param>
    public void SetQuestInfo(int questID, QuestState state)
    {
        _questID = questID;
        _state = state;
        _questTitle.text = QuestDB.instance.GetQuest(questID).GetTitle();
    }


    public void SetQuestInfo(int questID, QuestNPC questNPC)
    {
        _questID = questID;
        _questNPC = questNPC;
        _state = _questNPC.GetQuestState();
        _questTitle.text = QuestDB.instance.GetQuest(questID).GetTitle();
    }

    /// <summary>
    /// 대화상대인 퀘스트 NPC의 참조값 받아오기 
    /// </summary>
    /// <param name="questNPC"></param>
    public void SetQuestNPC(QuestNPC questNPC) { _questNPC = questNPC; }
}
