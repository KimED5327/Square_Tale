using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestNPC : MonoBehaviour
{
    [SerializeField] NpcWithLines _npc;
    public int _npcID;
    QuestState _questState;
    bool _isParsingDone;

    [Header("NPC UI")]
    [Tooltip("기본 다이얼로그 캔버스")]
    [SerializeField] GameObject _dialogueCanvas;
    [Tooltip("퀘스트 다이얼로그 캔버스")]
    [SerializeField] GameObject _questDialogueCanvas;
    [Tooltip("NPC 퀘스트마크 이미지")]
    [SerializeField] Image _imgQuestMark;
    [Tooltip("퀘스트마크 - 진행가능")]
    [SerializeField] Sprite _imgQuestOpened;
    [Tooltip("퀘스트마크 - 진행중")]
    [SerializeField] Sprite _imgQuestOngoing;
    [Tooltip("퀘스트마크 - 완료가능")]
    [SerializeField] Sprite _imgQuestCompletable;
    [Tooltip("NPC 네임태그 텍스트")]
    [SerializeField] Text _txtNameTag;
    [Tooltip("다이얼로그 박스 내 NPC 이름 텍스트")]
    [SerializeField] Text _txtNpcName;
    [Tooltip("다이얼로그 박스 내 NPC 대사 텍스트")]
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
    /// npcID에 맞는 데이터 받아와서 _npc에 저장 
    /// </summary>
    private void ParsingData()
    {
        if (NPCLoader.instance.ParsingCompleted() && !_isParsingDone)
        {
            _npc = NpcDB.instance.GetNPC(_npcID);
            _isParsingDone = true;
            CheckAvailableQuest();
            SetNameTag();
            SetQuestMark();
            //SetDefaultLines();
        }
    }

    /// <summary>
    /// 진행 가능한 퀘스트가 있는지 탐색 
    /// </summary>
    public bool CheckAvailableQuest()
    {
        bool isAvailable = false;

        for (int i = 0; i < _npc.GetQuestsCount(); i++)
        {
            if(QuestDB.instance.GetQuest(_npc.GetQuestID(i)).GetState() == QuestState.QUEST_OPENED)
            {
                isAvailable = true;
                _questState = QuestState.QUEST_OPENED;
            }
        }

        return isAvailable;
    }

    /// <summary>
    /// 진행 가능한 퀘스트 ID 리턴 
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
    /// NPC의 기본 대사들 중 랜덤한 대사를 세팅 
    /// </summary>
    public void SetDefaultLines()
    {
        _txtNpcName.text = _npc.GetName();
        _txtNpcLines.text = _npc.GetLine(Random.Range(0, _npc.GetLinesCount()));
    }

    /// <summary>
    /// NPC의 퀘스트 진행상태에 따른 퀘스트 마크 세팅 
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
    /// NPC 네임태그 텍스트 세팅 
    /// </summary>
    private void SetNameTag()
    {
        _txtNameTag.text = _npc.GetName();
        _txtNpcName.text = _npc.GetName();
    }

    public void ClickNPC()
    {
        transform.tag = "Untagged";

        switch (_questState)
        {
            case QuestState.QUEST_OPENED:
                DialogueManager.instance.SetQuestInfo(GetAvailableQuestID(), QuestState.QUEST_OPENED);
                DialogueManager.instance.QuestOpenedDialogue();
                break;

            case QuestState.QUEST_ONGOING:
                break;

            case QuestState.QUEST_COMPLETABLE:
                DialogueManager.instance.SetQuestInfo(GetAvailableQuestID(), QuestState.QUEST_COMPLETABLE);
                DialogueManager.instance.QuestOpenedDialogue();
                break;

            default:
                SetDefaultLines();
                _dialogueCanvas.SetActive(true);
                DialogueManager.instance.SetNameTag(_txtNameTag);
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
}
