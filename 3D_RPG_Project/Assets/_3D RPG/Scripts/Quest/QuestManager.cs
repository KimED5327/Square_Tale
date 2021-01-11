using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 진행, 완료된 퀘스트를 관리하고 퀘스트 타입별로 완료 조건 검사를 수행하는 매니져 
/// </summary>
public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public delegate void EventHandler();
    public static event EventHandler CheckAvailableQuest;

    Inventory _inventory;            // 인벤토리 참조자 
    string _questInfoKey = "info";   // 퀘스트 타입 해시테이블 키 

    /// <summary>
    /// 현재 진행중인 퀘스트 리스트 
    /// </summary>
    List<Quest> _ongoingQuests = new List<Quest>();

    /// <summary>
    /// 완료된 퀘스트 리스트 
    /// </summary>
    List<Quest> _finishedQuests = new List<Quest>();

    private void Awake()
    {
        if (instance == null) instance = this; 
    }

    private void Start()
    {
        _inventory = FindObjectOfType<Inventory>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            //Debug.Log("3번 아이템 개수" + _inventory.GetItemCount(ItemDatabase.instance.GetItem(3)));
            //Debug.Log("4번 아이템 개수" + _inventory.GetItemCount(ItemDatabase.instance.GetItem(4)));

            //_inventory.TryToPushInventory(ItemDatabase.instance.GetItem(3));
            //Debug.Log("3번 아이템 획득");

            _inventory.TryToPushInventory(ItemDatabase.instance.GetItem(10));
            Debug.Log("10번 아이템 소지개수 : " + _inventory.GetItemCount(ItemDatabase.instance.GetItem(10)));
        }
    }

    /// <summary>
    /// 진행중인 퀘스트 리스트에 ongoingQuest를 원소로 추가하기 
    /// </summary>
    /// <param name="ongoingQuest"></param>
    public void AddOngoingQuest(Quest ongoingQuest)
    {
        Debug.Log(ongoingQuest.GetQuestID() + "번 퀘스트 수락");
        _ongoingQuests.Add(ongoingQuest);

        // 퀘스트 부여자의 상대값 변경 
        SetQuestGiverToOngoingState(ongoingQuest);

        // 퀘스트 수락에 따른 퀘스트 타입별 상호작용 
        AcceptInterationPerType(ongoingQuest);
    }

    /// <summary>
    /// 완료된 퀘스트 리스트에 finishedQuest를 원소로 추가하기 
    /// </summary>
    /// <param name="finishedQuest"></param>
    public void AddFinishedQuest(Quest finishedQuest)
    {
        Debug.Log(finishedQuest.GetQuestID() + "번 퀘스트 완료");
        _finishedQuests.Add(finishedQuest);

        // 퀘스트 완료자의 상태값을 퀘스트 완료로 변경 
        SetQuestFinisherToCompleteState(finishedQuest);

        // 퀘스트 부여자와 완료자가 일치하지 않을 경우 퀘스트 부여자의 상태값을 퀘스트 완료로 변경 
        if (finishedQuest.GetQuestGiver() != finishedQuest.GetQuestFinisher())
            SetQuestGiverToCompleteState(finishedQuest);

        // 퀘스트 완료에 따른 퀘스트 타입 별 상호작용 
        CompleteInteractionPerType(finishedQuest);

        // 퀘스트 완료로 해금된 퀘스트가 있다면 오픈 
        OpenQuest(finishedQuest.GetQuestID());
    }

    /// <summary>
    /// 퀘스트가 완료되어 진행중인 퀘스트 리스트에서 삭제하기 
    /// </summary>
    public void DeleteOngoingQuest()
    {
        _ongoingQuests.RemoveAt(0);
    }

    /// <summary>
    /// 퀘스트 수락에 따른 퀘스트 타입 별 상호작용 
    /// </summary>
    void AcceptInterationPerType(Quest quest)
    {
        switch (quest.GetQuestType())
        {
            case QuestType.TYPE_DELIVERITEM:
                // 아이템 소지 여부 확인하여 퀘스트 조건 검사하기 
                if (CheckItemsToDeliver(quest)) SetQuestFinisherToCompletableState(quest);
                break;

            case QuestType.TYPE_CARRYITEM:
                // 아이템 소지 여부 확인하여 퀘스트 조건 검사하기 
                if (CheckItemsToCarry(quest)) SetQuestFinisherToCompletableState(quest);
                break;

            case QuestType.TYPE_OPERATEOBJECT:
                break;

            case QuestType.TYPE_KILLENEMY:
                break;

            case QuestType.TYPE_TALKWITHNPC:
                // 대화 상대자인 퀘스트 완료자의 상태값을 퀘스트 완료가능으로 변경 
                SetQuestFinisherToCompletableState(quest);
                break;
        }
    }
    
    /// <summary>
    /// 퀘스트 완료에 따른 퀘스트 타입 별 상호작용 
    /// </summary>
    void CompleteInteractionPerType(Quest quest)
    {
        switch (quest.GetQuestType())
        {
            case QuestType.TYPE_DELIVERITEM:
                // 전달된 아이템을 인벤토리 목록에서 삭제 
                DeleteItemsDelivered(quest);
                break;

            case QuestType.TYPE_CARRYITEM:
                break;

            case QuestType.TYPE_OPERATEOBJECT:
                break;

            case QuestType.TYPE_KILLENEMY:
                break;

            case QuestType.TYPE_TALKWITHNPC:
                //SetQuestGiverStatus(quest);
                break;
        }
    }

    /// <summary>
    /// 선행 퀘스트가 완료된 퀘스트의 진행상태를 미해금 상태에서 진행가능 상태로 변경 
    /// </summary>
    public void OpenQuest(int questID)
    {
        // 완료된 퀘스트의 ID(questID)가 선행 퀘스트 ID와 일치하는 퀘스트를 진행가능 상태로 변경 
        for (int i = 1; i < QuestDB.instance.GetMaxCount() + 1; i++)
        {
            if (QuestDB.instance.GetQuest(i).GetPrecedentID() != questID) continue;

            Debug.Log(QuestDB.instance.GetQuest(i).GetQuestID() + "번 퀘스트 해금");
            QuestDB.instance.GetQuest(i).SetState(QuestState.QUEST_OPENED);
            CheckAvailableQuest();
        }
    }

    /// <summary>
    /// 진행 중인 퀘스트 중 '아이템 전달' 타입의 퀘스트가 있다면, 퀘스트 달성 요건 확인 
    /// </summary>
    public void CheckDeliverItemQuest()
    {
        for (int i = 0; i < _ongoingQuests.Count; i++)
        {
            if (_ongoingQuests[i].GetQuestType() != QuestType.TYPE_DELIVERITEM) continue;

            if (CheckItemsToDeliver(_ongoingQuests[i])) SetQuestFinisherToCompletableState(_ongoingQuests[i]);
        }
    }

    /// <summary>
    /// Type1. '아이템 전달' 퀘스트의 달성 요건 확인 후, 요건 충족 여부를 bool 값으로 리턴   
    /// </summary>
    public bool CheckItemsToDeliver(Quest quest)
    {
        bool isAvailable = true;

        DeliverItem deliverItem = quest.GetQuestInfo()[_questInfoKey] as DeliverItem;

        for (int i = 0; i < deliverItem.GetItemList().Count; i++)
        {
            if(!_inventory.HaveItemCount(ItemDatabase.instance.GetItem(deliverItem.GetItem(i).GetItemID()),
               deliverItem.GetItem(i).GetCount()))
            {
                Debug.Log(deliverItem.GetItem(i).GetItemID() + "번 아이템 : " +
                    _inventory.GetItemCount(ItemDatabase.instance.GetItem(deliverItem.GetItem(i).GetItemID())) + "개 소지");
                isAvailable = false; 
            }
        }

        return isAvailable;
    }

    /// <summary>
    /// Type1. '아이템 전달' 퀘스트 완료 후 전달된 아이템을 인벤토리에서 제거 
    /// </summary>
    /// <param name="quest"></param>
    public void DeleteItemsDelivered(Quest quest)
    {
        DeliverItem deliverItem = quest.GetQuestInfo()[_questInfoKey] as DeliverItem;

        for (int i = 0; i < deliverItem.GetItemList().Count; i++)
        {
            _inventory.DecreaseItemCount(ItemDatabase.instance.GetItem(deliverItem.GetItem(i).GetItemID()),
                deliverItem.GetItem(i).GetCount());

            Debug.Log(deliverItem.GetItem(i).GetItemID() + "번 아이템 : " +
                _inventory.GetItemCount(ItemDatabase.instance.GetItem(deliverItem.GetItem(i).GetItemID())) + "개 삭제");
        }
    }

    /// <summary>
    /// Type4. 진행 중인 퀘스트 중 '아이템 소지' 타입의 퀘스트가 있다면, 퀘스트 달성 요건 확인 
    /// </summary>
    public void CheckCarryItemQuest()
    {
        for (int i = 0; i < _ongoingQuests.Count; i++)
        {
            if (_ongoingQuests[i].GetQuestType() != QuestType.TYPE_CARRYITEM) continue;

            if (CheckItemsToCarry(_ongoingQuests[i])) SetQuestFinisherToCompletableState(_ongoingQuests[i]);
        }
    }

    /// <summary>
    /// Type4. '아이템 소지' 퀘스트의 달성 요건 확인 후, 요건 충족 여부를 bool 값으로 리턴 
    /// </summary>
    /// <param name="quest"></param>
    public bool CheckItemsToCarry(Quest quest)
    {
        CarryItem carryItem = quest.GetQuestInfo()[_questInfoKey] as CarryItem;

        bool isAvailable = (_inventory.HaveItemCount(ItemDatabase.instance.GetItem(carryItem.GetItemID()),
            carryItem.GetItemCount())) ? true : false; 

        return isAvailable;
    }

    /// <summary>
    /// 퀘스트를 수락하여 퀘스트 부여자의 상태 값을 퀘스트 진행중으로 변경 
    /// </summary>
    /// <param name="quest"></param>
    void SetQuestGiverToOngoingState(Quest quest)
    {
        quest.GetQuestGiver().SetOngoingQuestID(quest.GetQuestID());
        quest.GetQuestGiver().SetQuestState(QuestState.QUEST_ONGOING);
        quest.GetQuestGiver().SetQuestMark();
    }

    /// <summary>
    /// 퀘스트를 완료하여 퀘스트 부여자의 상태 값을 퀘스트 완료로 변경 
    /// </summary>
    /// <param name="state"></param>
    void SetQuestGiverToCompleteState(Quest quest)
    {
        quest.GetQuestGiver().SetOngoingQuestID(0);
        quest.GetQuestGiver().DeleteCompletedQuest(quest.GetQuestID());
        quest.GetQuestGiver().UpdateQuestState();
        quest.GetQuestGiver().SetQuestMark();
    }

    /// <summary>
    /// 퀘스트 완료조건을 충족하여 퀘스트 완료자의 상태 값을 퀘스트 완료가능으로 변경 
    /// </summary>
    /// <param name="state"></param>
    void SetQuestFinisherToCompletableState(Quest quest)
    {
        quest.GetQuestFinisher().SetOngoingQuestID(quest.GetQuestID());
        quest.GetQuestFinisher().SetQuestState(QuestState.QUEST_COMPLETABLE);
        quest.GetQuestFinisher().SetQuestMark();
    }

    /// <summary>
    /// 퀘스트를 완료하여 퀘스트 완료자의 상태 값을 퀘스트 완료로 변경 
    /// </summary>
    /// <param name="quest"></param>
    void SetQuestFinisherToCompleteState(Quest quest)
    {
        quest.GetQuestFinisher().SetOngoingQuestID(0);
        quest.GetQuestFinisher().DeleteCompletedQuest(quest.GetQuestID());
        quest.GetQuestFinisher().UpdateQuestState();
        quest.GetQuestFinisher().SetQuestMark();
    }
}
