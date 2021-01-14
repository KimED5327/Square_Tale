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
    [SerializeField] GameObject _questMenuPanel = null;

    [Header("퀘스트 정보 UI")]
    [SerializeField] Text _txtTitle;    // 정보란의 퀘스트 제목 
    [SerializeField] Text _txtNpcName;      // NPC 이름  
    [SerializeField] Text _txtDes;          // 퀘스트 내용  
    [SerializeField] Text _txtGoal;         // 퀘스트 목표 

    [SerializeField] GameObject _questBtnPrefab = null;
    [SerializeField] Transform _content = null;

    QuestSort _questSort;
    QuestSlot _slot = new QuestSlot();

    void Start()
    {
        //_inven = FindObjectOfType<Inventory>();

        //_slots = new ShopSlot[_slotMaxCount];
        //for (int i = 0; i < _slotMaxCount; i++)
        //{
        //    ShopSlot slot = Instantiate(_questSlotPrefab, _tfSlotParent).GetComponent<ShopSlot>();
        //    _slots[i] = slot;
        //}

        //TouchTabBtn(0);
    }

    public void OpenMenu()
    {
        InteractionManager._isOpen = true; 
        GameHudMenu.instance.HideMenu();
        _questMenuPanel.SetActive(true);
        _questSort = QuestSort.ONGOING;

        // 메뉴 초기화 
        ResetMenu();
    }

    public void CloseMenu()
    {
        InteractionManager._isOpen = false; 
        GameHudMenu.instance.ShowMenu();
        _questMenuPanel.SetActive(false);
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

    void ShowOngoingQuest()
    {
        // 모든 퀘스트 슬롯 삭제 
        DeleteAllSlots();

        for (int i = 0; i < QuestManager.instance.GetOngoingQuestList().Count; i++)
        {
            QuestSlot slot = Instantiate(_questBtnPrefab, _content).GetComponent<QuestSlot>();

            slot.SetQuest(QuestManager.instance.GetOngoingQuestByIdx(i));
            slot.SetTitle(slot.GetQuest().GetTitle());

            if (i == 0) SetQuestInfo(slot.GetQuest());
        }

        //_slots = new ShopSlot[_slotMaxCount];
        //for (int i = 0; i < _slotMaxCount; i++)
        //{
        //    ShopSlot slot = Instantiate(_goSlotPrefab, _tfSlotParent).GetComponent<ShopSlot>();
        //    _slots[i] = slot;
        //}
    }

    void ShowFinishedQuest()
    {
        
    }

    /// <summary>
    /// 퀘스트 정보란 데이터 값 설정 
    /// </summary>
    void SetQuestInfo(Quest quest)
    {
        _txtTitle.text = quest.GetTitle();
        _txtNpcName.text = NpcDB.instance.GetNPC(quest.GetNpcID()).GetName();
        _txtDes.text = quest.GetDes();
        _txtGoal.text = QuestManager.instance.GetQuestGoal(quest);

        //if (quest.GetQuestType() == QuestType.TYPE_DELIVERITEM)
        //    _txtGoal.text = QuestManager.instance.GetDeliverItemGoal(quest);
    }

    /// <summary>
    /// 모든 퀘스트 슬롯을 삭제 
    /// </summary>
    void DeleteAllSlots()
    {
        foreach(Transform child in _content.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
