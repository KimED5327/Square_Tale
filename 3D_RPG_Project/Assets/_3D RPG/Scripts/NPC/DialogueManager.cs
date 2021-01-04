using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    int _questID = 0;
    int _lineIdx = 0;
    bool _isTalking = false;
    QuestState _state;

    Transform _npcTransform;
    Text _txtNameTag;

    [SerializeField] Transform _player;

    [Header("Quest Dialogue UI")]
    [SerializeField] GameObject _hudCanvas;
    [SerializeField] GameObject _dialogueCanvas; 
    [SerializeField] GameObject _questDialogueCanvas;
    [SerializeField] Text _questTitle;
    [SerializeField] Text _npcName;
    [SerializeField] Text _npcLines;
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
        _questDialogueCanvas.SetActive(_isTalking);
    }

    /// <summary>
    /// 퀘스트가 현재 진행 중일 때의 대화 
    /// </summary>
    public void QuestOngoingDialogue()
    {

    }

    /// <summary>
    /// 퀘스트가 완료되었을 때 이루어지는 대화 
    /// </summary>
    public void QuestCompletedDialogue()
    {

    }

    /// <summary>
    /// 디폴트 다이얼로그 캔버스 닫기 
    /// </summary>
    public void CloseDialogueCanvas()
    {
        _player.GetComponent<CameraController>().enabled = true;
        _dialogueCanvas.SetActive(false);
        _hudCanvas.SetActive(true);
        _txtNameTag.enabled = true;
        _txtNameTag.transform.parent.transform.parent.tag = "QuestNPC";
    }

    /// <summary>
    /// 퀘스트 정보 세팅 
    /// </summary>
    /// <param name="questID"></param>
    /// <param name="state"></param>
    public void SetQuestInfo(int questID, QuestState state)
    {
        _questID = questID;
        _state = state; 
        _questTitle.text = QuestDB.instance.GetQuest(questID).GetTitle();
    }

    /// <summary>
    /// 퀘스트 대사 다이얼로그 창에 한줄씩 출력 
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
    /// NPC의 네임태그를 on/off 할 수 있도록 현재 대화 중인 NPC 네임태그 받아오기 
    /// </summary>
    /// <param name="nameTag"></param>
    public void SetNameTag(Text nameTag) { _txtNameTag = nameTag; }
}
