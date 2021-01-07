using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// npcID에 맞는 퀘스트 NPC의 데이터 정보를 가지며, 기본대사 출력, 진행가능 퀘스트 탐색 등 퀘스트NPC의 기능 수행
/// </summary>
public class QuestNPC : MonoBehaviour
{
    /// <summary>
    /// npcID에 맞는 NPC 데이터로서, NpcDB에 저장된 데이터와는 별개이며 데이터값 수정 가능 
    /// </summary>
    [SerializeField] NpcWithLines _npc = new NpcWithLines();

    /// <summary>
    /// 원래 퀘스트용 변수이나 NPC의 퀘스트 진행상태를 파악하는 데 사용 (0.해금된 퀘스트 없음 1.진행가능 퀘스트 있음 2.퀘스트 진행중 3.퀘스트 완료가능 4.NPC의 모든 퀘스트 완료)
    /// </summary>
    [SerializeField] QuestState _questState;
    
    /// <summary>
    /// 현재 NPC가 진행중인 퀘스트 ID (0 : 진행중인 퀘스트 없음)
    /// </summary>
    public int _ongoingQuestID;     // 현재 NPC가 진행 중인 퀘스트 ID
    public int _npcID;              // 해당 객체의 NPC ID 
    bool _isParsingDone;            // _npc객체에 NpcDB의 데이터가 파싱되었는지 확인 
    string questInfoKey = "info";   // 해시테이블 QuestInfo 키  

    [Header("NPC UI")]
    [Tooltip("기본 다이얼로그 Panel")]
    [SerializeField] GameObject _dialoguePanel;
    [Tooltip("퀘스트 다이얼로그 Panel")]
    [SerializeField] GameObject _questDialoguePanel;
    [Tooltip("NPC 머리 상단에 띄우는 퀘스트마크 Image")]
    [SerializeField] Image _imgQuestMark;
    [Tooltip("퀘스트마크 Sprite - 진행가능(노란색 느낌표)")]
    [SerializeField] Sprite _imgQuestOpened;
    [Tooltip("퀘스트마크 Sprite - 진행중(회색 물음표)")]
    [SerializeField] Sprite _imgQuestOngoing;
    [Tooltip("퀘스트마크 Sprite - 완료가능(노란색 물음표)")]
    [SerializeField] Sprite _imgQuestCompletable;
    [Tooltip("NPC 머리 상단에 띄우는 네임태그 Text")]
    [SerializeField] Text _txtNameTag;
    [Tooltip("기본 다이얼로그 박스 내 NPC 이름 Text")]
    [SerializeField] Text _txtNpcName;
    [Tooltip("기본 다이얼로그 박스 내 NPC 대사 Text")]
    [SerializeField] Text _txtNpcLines;

    // Start is called before the first frame update
    void Start()
    {
        _questState = QuestState.QUEST_VEILED;
    }

    // Update is called once per frame
    void Update()
    {
        ParsingData(); 
    }

    /// <summary>
    /// npcID에 맞는 데이터를 받아와서 동적할당한 NpcWithLines 객체(_npc)에 저장 
    /// </summary>
    private void ParsingData()
    {
        if (NPCLoader.instance.ParsingCompleted() && !_isParsingDone)
        {
            _npc = NpcDB.instance.GetNPC(_npcID);
            CheckAvailableQuest();
            SetNameTag();
            SetQuestMark();
            _isParsingDone = true;
        }
    }

    /// <summary>
    /// 현재 진행 가능한 퀘스트가 있는지 탐색 
    /// </summary>
    public bool CheckAvailableQuest()
    {
        bool isAvailable = false;

        if(_npc.GetQuestsCount() <= 0)
        {
            _questState = QuestState.QUEST_COMPLETED;
            return false; 
        }

        for (int i = 0; i < _npc.GetQuestsCount(); i++)
        { 
            if (QuestDB.instance.GetQuest(_npc.GetQuestID(i)).GetState() != QuestState.QUEST_OPENED) continue;

            if(QuestDB.instance.GetQuest(_npc.GetQuestID(i)).GetQuestType() == QuestType.TYPE_TALKWITHNPC &&
               QuestDB.instance.GetQuest(_npc.GetQuestID(i)).GetNpcID() != _npcID)
            {
                TalkWithNpc talkWithNpc = QuestDB.instance.GetQuest(_npc.GetQuestID(i)).GetQuestInfo()[questInfoKey] as TalkWithNpc;
                talkWithNpc.SetQuestFinisher(this);
                continue;
            }

            isAvailable = true;
            _questState = QuestState.QUEST_OPENED;
        }

        if (!isAvailable) _questState = QuestState.QUEST_VEILED;

        return isAvailable;
    }

    /// <summary>
    /// 진행 가능한 퀘스트 ID 리턴 (0 : 진행 가능한 퀘스트 없음)
    /// </summary>
    /// <returns></returns>
    public int GetAvailableQuestID()
    {
        int questID = 0;

        for (int i = 0; i < _npc.GetQuestsCount(); i++)
        {
            if (QuestDB.instance.GetQuest(_npc.GetQuestID(i)).GetState() == QuestState.QUEST_OPENED)
            {
                questID = _npc.GetQuestID(i);
                _questState = QuestState.QUEST_OPENED;
            }
        }

        return questID;
    }

    /// <summary>
    /// 완료된 퀘스트는 NPC의 퀘스트 목록에서 지우기 
    /// </summary>
    /// <param name="questID"></param>
    public void DeleteCompletedQuest(int questID)
    {
        for (int i = 0; i < _npc.GetQuestsCount(); i++)
        {
            if (questID == _npc.GetQuestID(i))
            {
                _npc.GetQuestList().RemoveAt(i);
                Debug.Log(_npcID + "번 NPC의 " + questID + "번 퀘스트 삭제됨");
            }
        }
    }

    /// <summary>
    /// NPC 머리 상단의 네임태그 텍스트 세팅 
    /// </summary>
    private void SetNameTag()
    {
        _txtNameTag.text = _npc.GetName();
    }

    /// <summary>
    /// NPC의 퀘스트 진행상태에 따라 퀘스트 마크(NPC 머리 상단 위치) 세팅 
    /// </summary>
    public void SetQuestMark()
    {
        switch (_questState)
        {
            case QuestState.QUEST_OPENED:
                _imgQuestMark.enabled = true; 
                _imgQuestMark.sprite = _imgQuestOpened;
                break;

            case QuestState.QUEST_ONGOING:
                _imgQuestMark.enabled = true;
                _imgQuestMark.sprite = _imgQuestOngoing;
                break;

            case QuestState.QUEST_COMPLETABLE:
                _imgQuestMark.enabled = true;
                _imgQuestMark.sprite = _imgQuestCompletable;
                break;

            default:
                _imgQuestMark.enabled = false; 
                break;
        }
    }

    /// <summary>
    /// NPC의 기본 대사들 중 랜덤한 대사 하나를 기본 다이얼로그 박스 내 세팅  
    /// </summary>
    public void SetDefaultLines()
    {
        _txtNpcName.text = _npc.GetName();
        _txtNpcLines.text = _npc.GetLine(Random.Range(0, _npc.GetLinesCount()));
    }

    /// <summary>
    /// 진행중인 퀘스트가 있을 경우, 해당 퀘스트ID가 진행중일 때의 대사를 기본 다이얼로그 박스 내 세팅   
    /// </summary>
    public void SetQuestOngoingLines()
    {
        _txtNpcName.text = _npc.GetName();
        _txtNpcLines.text = QuestDialogueDB.instance.GetDialogue(_ongoingQuestID).GetLine(_questState, 0);
    }

    /// <summary>
    /// NPC 클릭(터치) 시 퀘스트 진행상태에 따라 다이얼로그 진행(퀘스트 진행가능, 진행중, 완료가능 상태를 제외하면 디폴트 대사 출력)
    /// </summary>
    public void ClickNPC()
    {
        //NPC 클릭 후 다이얼로그 진행 시에 NPC가 다시 터치되지 않도록 태그 변경(ray로 tag를 체크해서 터치 감지)
        transform.tag = "Untagged";

        switch (_questState)
        {
            //진행 가능한 퀘스트가 있는 경우 
            case QuestState.QUEST_OPENED:
                DialogueManager.instance.SetQuestInfo(GetAvailableQuestID(), this);
                DialogueManager.instance.QuestOpenedDialogue();
                break;
            
            //현재 특정한 퀘스트를 진행중인 경우 (디폴트 패널로 대사 출력)
            case QuestState.QUEST_ONGOING:

                // 해당 상태에 출력 가능한 대사가 없을 경우 return; 
                if (!QuestDialogueDB.instance.GetDialogue(_ongoingQuestID).CheckDialogue(_questState)) return;

                SetQuestOngoingLines();
                _dialoguePanel.SetActive(true);
                DialogueManager.instance.SetQuestNPC(this);
                break;
            
            //완료 가능한 퀘스트가 있는 경우 
            case QuestState.QUEST_COMPLETABLE:
                DialogueManager.instance.SetQuestInfo(_ongoingQuestID, this);
                DialogueManager.instance.QuestOpenedDialogue();
                break;

            // 그 외 상태에서는 다이얼로그 패널로 디폴트 대사 출력  
            default:
                SetDefaultLines();
                _dialoguePanel.SetActive(true);
                DialogueManager.instance.SetQuestNPC(this);
                break;
        }
    }

    private void Test()
    {
        Debug.Log("NPC" + _npcID + " : " + _npc.GetName());

        for (int i = 0; i < _npc.GetLinesCount(); i++)
        {
            Debug.Log(_npc.GetLine(i));
        }
    }

    /// <summary>
    /// 퀘스트 진행중 상태에 대한 대사 유무를 bool 값으로 리턴  
    /// </summary>
    /// <returns></returns>
    public bool CheckOngoingQuestDialogue()
    {
        return QuestDialogueDB.instance.GetDialogue(_ongoingQuestID).CheckDialogue(_questState);
    }

    /// <summary>
    /// NPC 머리 상단의 네임태그 활성화 
    /// </summary>
    public void TurnOnNameTag() { _txtNameTag.enabled = true; }

    /// <summary>
    /// NPC 머리 상단의 네임태그 비활성화 
    /// </summary>
    public void TurnOffNameTag() { _txtNameTag.enabled = false; }

    public int GetNpcID() { return _npcID; }
    public void SetNpcID(int npcID) { _npcID = npcID; }

    public int GetOngoingQuestID() { return _ongoingQuestID; }
    public void SetOngoingQuestID(int questID) { _ongoingQuestID = questID; }

    public QuestState GetQuestState() { return _questState; }
    public void SetQuestState(QuestState state) { _questState = state; }
}
