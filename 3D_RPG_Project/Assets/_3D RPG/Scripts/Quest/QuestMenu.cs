using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 퀘스트 메뉴 UI를 관리하는 클래스
/// </summary>
public class QuestMenu : MonoBehaviour
{
    /// <summary>
    /// 퀘스트 메뉴의 분류(진행/완료)
    /// </summary>
    enum QuestSort
    {
        ONGOING,
        FINISHED,
    }

    [Tooltip("퀘스트 메뉴 Panel")]
    [SerializeField] GameObject _menuPanel;         // 퀘스트 메뉴 패널 
    [SerializeField] GameObject _infoPanel;         // 퀘스트 정보 패널 

    [Header("퀘스트 정보 UI")]
    [SerializeField] Text _txtTitle;                // 퀘스트 제목 
    [SerializeField] Text _txtNpcName;              // NPC 이름  
    [SerializeField] Text _txtDes;                  // 퀘스트 내용  
    [SerializeField] Text _txtGoal;                 // 퀘스트 목표 
    [SerializeField] Text _txtNoSolotMsg;           // 슬롯 메시지 

    [SerializeField] GameObject _ongoingSlotPrefab;     // 진행 퀘스트 슬롯 프리팹 
    [SerializeField] GameObject _finishedSlotPrefab;    // 완료 퀘스트 슬롯 프리팹 
    [SerializeField] Transform _ongoingContent;         // 진행 슬롯 컨텐트
    [SerializeField] Transform _finishedContent;        // 완료 슬롯 컨텐트 

    QuestSort _questSort;                           // 퀘스트 분류(진행/완료)

    List<QuestSlot> _ongoingSlots = new List<QuestSlot>();      // 진행 퀘스트 슬롯 리스트 
    List<QuestSlot> _finishedSlots = new List<QuestSlot>();     // 완료 퀘스트 슬롯 리스트 

    void Start()
    {
        SetMenuOnStart();
    }

    public void OpenMenu()
    {
        InteractionManager._isOpen = true; 
        GameHudMenu.instance.HideMenu();
        _menuPanel.SetActive(true);
        _questSort = QuestSort.ONGOING;

        // 메뉴 초기화 
        ResetMenu();
    }

    public void CloseMenu()
    {
        InteractionManager._isOpen = false; 
        GameHudMenu.instance.ShowMenu();
        _menuPanel.SetActive(false);
    }

    /// <summary>
    /// 퀘스트 매니져의 진행/완료 퀘스트 데이터를 받아와 슬롯 세팅 
    /// </summary>
    void SetMenuOnStart()
    {
        DeleteAllSlots();

        // 진행중인 퀘스트를 슬롯에 추가 
        for (int i = 0; i < QuestManager.instance.GetOngoingQuestList().Count; i++)
        {
            QuestSlot slot = Instantiate(_ongoingSlotPrefab, _ongoingContent).GetComponent<QuestSlot>();

            Quest quest = QuestManager.instance.GetOngoingQuestByIdx(i);

            slot.SetQuest(quest);
            slot.SetTitle(quest.GetTitle());
            _ongoingSlots.Add(slot);
        }

        // 완료된 퀘스트를 슬롯에 추가 
        for (int i = 0; i < QuestManager.instance.GetFinishedQuestList().Count; i++)
        {
            QuestSlot slot = Instantiate(_finishedSlotPrefab, _finishedContent).GetComponent<QuestSlot>();

            Quest quest = QuestManager.instance.GetFinishedQuestByIdx(i);

            // 완료한 퀘스트는 마크 대신 퀘스트 ID + 제목으로 표기 
            string title = quest.GetQuestID() + ". " + quest.GetTitle();

            slot.SetQuest(quest);
            slot.SetTitle(title);
            _finishedSlots.Add(slot);
        }
    }

    /// <summary>
    /// 메뉴 분류값(진행/완료) 상태에 따라 퀘스트 메뉴 초기화 
    /// </summary>
    void ResetMenu()
    {
        switch (_questSort)
        {
            case QuestSort.ONGOING:
                ShowOngoingQuest();
                break;

            case QuestSort.FINISHED:
                ShowFinishedQuest();
                break;
        }
    }

    /// <summary>
    /// 진행 퀘스트 메뉴 출력 
    /// </summary>
    public void ShowOngoingQuest()
    {
        _ongoingContent.gameObject.SetActive(true);
        _finishedContent.gameObject.SetActive(false);

        // 진행 중인 퀘스트가 있을 경우 정보 출력 
        if (_ongoingSlots.Count > 0)
        {
            _infoPanel.SetActive(true);
            _txtNoSolotMsg.enabled = false;
            SetQuestInfo(_ongoingSlots[0].GetQuest());
        }
        else // 진행 중인 퀘스트가 없을 경우 메시지 출력 
        {
            _infoPanel.SetActive(false);
            _txtNoSolotMsg.enabled = true;
            _txtNoSolotMsg.text = "진행 중인 퀘스트가 없습니다.";
        }
    }

    /// <summary>
    /// 완료 퀘스트 메뉴 출력 
    /// </summary>
    public void ShowFinishedQuest()
    {
        _finishedContent.gameObject.SetActive(true);
        _ongoingContent.gameObject.SetActive(false);

        // 진행 중인 퀘스트가 있을 경우 정보 출력 
        if (_finishedSlots.Count > 0)
        {
            _infoPanel.SetActive(true);
            _txtNoSolotMsg.enabled = false;
            SetQuestInfo(_finishedSlots[0].GetQuest());

            // 슬롯이 2개 이상일 경우 선택/비선택 슬롯의 색상을 나누어 설정 
            if(_finishedSlots.Count > 1)
            {
                SetFinishedSlotsToGray();
                _finishedSlots[0].TurnOnColor();
            }
        }
        else // 진행 중인 퀘스트가 없을 경우 메시지 출력 
        {
            _infoPanel.SetActive(false);
            _txtNoSolotMsg.enabled = true;
            _txtNoSolotMsg.text = "완료된 퀘스트가 없습니다.";
        }
    }

    /// <summary>
    /// 진행 슬롯에 현재 진행중인 퀘스트를 추가 
    /// </summary>
    /// <param name="quest"></param>
    public void AddOngoingSlot(Quest quest)
    {
        QuestSlot slot = Instantiate(_ongoingSlotPrefab, _ongoingContent).GetComponent<QuestSlot>();

        slot.SetQuest(quest);
        slot.SetTitle(quest.GetTitle());
        _ongoingSlots.Add(slot);
    }

    /// <summary>
    /// 진행 슬롯에서 완료한 퀘스트 삭제 
    /// </summary>
    /// <param name="quest"></param>
    public void DeleteOngoingSlotAsFinished(int questID)
    {
        for (int i = 0; i < _ongoingSlots.Count; i++)
        {
            if (_ongoingSlots[i].GetQuest().GetQuestID() != questID) continue; 

            _ongoingSlots.RemoveAt(i);
            DeleteOngoingSlotByID(questID);
        }
    }

    /// <summary>
    /// 완료 슬롯에 완료한 퀘스트를 추가 
    /// </summary>
    /// <param name="quest"></param>
    public void AddFinishedSlot(Quest quest)
    {
        QuestSlot slot = Instantiate(_finishedSlotPrefab, _finishedContent).GetComponent<QuestSlot>();

        // 완료한 퀘스트는 마크 대신 퀘스트 ID + 제목으로 표기 
        string title = quest.GetQuestID() + ". " + quest.GetTitle();

        slot.SetQuest(quest);
        slot.SetTitle(title);
        _finishedSlots.Add(slot);
    }

    /// <summary>
    /// 퀘스트 정보란 데이터 값 설정 
    /// </summary>
    public void SetQuestInfo(Quest quest)
    {
        _txtTitle.text = quest.GetTitle();
        _txtNpcName.text = NpcDB.instance.GetNPC(quest.GetNpcID()).GetName();
        _txtDes.text = quest.GetDes();
        _txtGoal.text = QuestManager.instance.GetQuestGoal(quest);
    }

    /// <summary>
    /// 모든 완료 슬롯에 회색 마스크 이미지 씌우기
    /// </summary>
    public void SetFinishedSlotsToGray()
    {
        for (int i = 0; i < _finishedSlots.Count; i++)
        {
            _finishedSlots[i].TurnOffColor();
        }
    }

    /// <summary>
    /// 모든 퀘스트 슬롯을 삭제 
    /// </summary>
    void DeleteAllSlots()
    {
        foreach(Transform child in _ongoingContent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// 진행 슬롯 Content에서 파라미터로 받은 ID와 같은 퀘스트 ID를 가진 자식객체를 삭제 
    /// </summary>
    /// <param name="questID"></param>
    void DeleteOngoingSlotByID(int questID)
    {
        foreach (Transform child in _ongoingContent.transform)
        {
            if (child.GetComponent<QuestSlot>().GetQuest().GetQuestID() != questID) continue;

            Destroy(child.gameObject);
        }
    }
}
