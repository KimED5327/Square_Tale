using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 진행, 완료된 퀘스트를 관리하고 퀘스트 타입별로 완료 조건 검사를 수행하는 매니져 
/// </summary>
public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public delegate void EventHandler();
    public static event EventHandler CheckAvailableQuest;

    public delegate void SyncHandler(Quest quest);
    public static event SyncHandler SyncWithQuestOnStart;

    public Inventory _inventory;               // 인벤토리 참조자 
    public QuestHUD _questHUD;                 // QuestHUD 참조자 
    public QuestMenu _questMenu;               // QuestMenu 참조자 
    public PlayerStatus _playerStatus;         // PlayerStatus 참조자 
    bool _isHudOpen = false;            // QuestHUD 창 오픈여부 확인 변수 
    bool _isCompletableIconOn = false;  // QuestHUD 완료가능 아이콘 on/off 변수 
    string _questInfoKey = "info";      // 퀘스트 타입 해시테이블 키 

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
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else Destroy(gameObject);
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.O))
        //{
        //    //Debug.Log("3번 아이템 개수" + _inventory.GetItemCount(ItemDatabase.instance.GetItem(3)));
        //    //Debug.Log("4번 아이템 개수" + _inventory.GetItemCount(ItemDatabase.instance.GetItem(4)));

        //    _inventory.TryToPushInventory(ItemDatabase.instance.GetItem(3));
        //    Debug.Log("3번 아이템 획득");

        //    _inventory.TryToPushInventory(ItemDatabase.instance.GetItem(7));
        //    Debug.Log("7번 아이템 획득");

        //    _inventory.TryToPushInventory(ItemDatabase.instance.GetItem(9));
        //    Debug.Log("9번 아이템 소지개수 : " + _inventory.GetItemCount(ItemDatabase.instance.GetItem(9)));
        //}
    }

    /// <summary>
    /// 진행중인 퀘스트 리스트에 ongoingQuest를 원소로 추가하기 
    /// </summary>
    /// <param name="ongoingQuest"></param>
    public void AddOngoingQuest(Quest ongoingQuest)
    {
        Debug.Log(ongoingQuest.GetQuestID() + "번 퀘스트 수락");
        ongoingQuest.SetState(QuestState.QUEST_ONGOING);
        _ongoingQuests.Add(ongoingQuest);

        // 퀘스트 메뉴에 진행 퀘스트 슬롯 추가 (이후의 상호작용을 위해 다른 명령보다 먼저 실행.)
        _questMenu.AddOngoingSlot(ongoingQuest);

        // 퀘스트 부여자의 상대값 변경 
        SetQuestGiverToOngoingState(ongoingQuest);

        // 퀘스트 수락에 따른 퀘스트 타입별 상호작용 
        AcceptInterationPerType(ongoingQuest);

        // 퀘스트 HUD 값 세팅 
        _questHUD.SetOngoingQuestHUD(ongoingQuest);
    }

    /// <summary>
    /// 완료된 퀘스트 리스트에 finishedQuest를 원소로 추가하기 
    /// </summary>
    /// <param name="finishedQuest"></param>
    public void AddFinishedQuest(Quest finishedQuest)
    {
        Debug.Log(finishedQuest.GetQuestID() + "번 퀘스트 완료");
        finishedQuest.SetState(QuestState.QUEST_COMPLETED);
        _finishedQuests.Add(finishedQuest);

        // 보상 지급 
        AcquireRewards(finishedQuest);

        // 퀘스트 완료자의 상태값을 퀘스트 완료로 변경 
        SetQuestFinisherToCompleteState(finishedQuest);

        // 퀘스트 부여자와 완료자가 일치하지 않을 경우 퀘스트 부여자의 상태값을 퀘스트 완료로 변경 
        if (finishedQuest.GetQuestGiver() != finishedQuest.GetQuestFinisher())
            SetQuestGiverToCompleteState(finishedQuest);

        // 퀘스트 완료에 따른 퀘스트 타입 별 상호작용 
        CompleteInteractionPerType(finishedQuest);

        // 퀘스트 완료로 해금된 퀘스트가 있다면 오픈 
        OpenQuest(finishedQuest.GetQuestID());

        // 퀘스트 메뉴에 완료한 퀘스트를 진행 슬롯에서 삭제 및 완료 슬롯에 추가 
        _questMenu.DeleteOngoingSlotAsFinished(finishedQuest.GetQuestID());
        _questMenu.AddFinishedSlot(finishedQuest);

        // 퀘스트 HUD 비활성화 
        _questHUD.DisalbeHUD();
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

            case QuestType.TYPE_KILLENEMY:
                break;

            case QuestType.TYPE_TALKWITHNPC:
                break;
        }
    }

    /// <summary>
    /// 퀘스트 보상 획득하기 
    /// </summary>
    /// <param name="quest"></param>
    void AcquireRewards(Quest quest)
    {
        // 골드 추가 
        _inventory.SetGold(_inventory.GetGold() + quest.GetGold());

        // 경험치 추가 
        _playerStatus.IncreaseExp(quest.GetExp());

        // 아이템 보상이 있을 경우 인벤토리에 추가 
        if (quest.GetItemID() != 0)
            _inventory.TryToPushInventory(ItemDatabase.instance.GetItem(quest.GetItemID()));

        // 블록 보상이 있을 경우 블록 추가 
        if (quest.GetBlockList().Count > 0)
        {
            for (int i = 0; i < quest.GetBlockList().Count; i++)
            {
                BlockManager.IncreaseBlockCount(quest.GetBlock(i).GetBlockID(), quest.GetBlock(i).GetCount());
            }
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
            Debug.Log("퀘스트 " + QuestDB.instance.GetQuest(i).GetQuestID() + "번 상태 : " + QuestDB.instance.GetQuest(i).GetState());
            CheckAvailableQuest();
        }
    }

    /// <summary>
    /// 해당 퀘스트 ID가 완료된 퀘스트 목록에 있는지 확인 
    /// </summary>
    /// <param name="questID"></param>
    /// <returns></returns>
    public bool CheckIfQuestIsCompleted(int questID)
    {
        bool isCompleted = false;

        foreach(Quest quest in _finishedQuests)
        {
            if (quest.GetQuestID() == questID)
            {
                isCompleted = true;
                return isCompleted;
            }
        }

        return isCompleted;
    }

    /// <summary>
    /// 진행 중인 퀘스트 중 '아이템 전달' 타입의 퀘스트가 있다면, 퀘스트 달성 요건 확인 
    /// </summary>
    public void CheckDeliverItemQuest()
    {
        for (int i = 0; i < _ongoingQuests.Count; i++)
        {
            if (_ongoingQuests[i].GetQuestType() != QuestType.TYPE_DELIVERITEM) continue;

            // 퀘스트 HUD 업데이트 
            _questHUD.UpdateHUD(_ongoingQuests[i]);

            // 현재 퀘스트가 완료가능 상태인데 요건 충족이 안된 경우 다시 진행중 상태로 변경 
            if(!CheckItemsToDeliver(_ongoingQuests[i])) 
            {
                if(_ongoingQuests[i].GetState() == QuestState.QUEST_COMPLETABLE) SetQuestFinisherToOngoingState(_ongoingQuests[i]);
                continue; 
            }

            // 요건이 충족된 경우 퀘스트 완료자를 완료 가능 상태로 변경 
            SetQuestFinisherToCompletableState(_ongoingQuests[i]);
        }
    }

    /// <summary>
    /// '아이템 전달' 퀘스트의 달성 요건 확인 후, 요건 충족 여부를 bool 값으로 리턴   
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
    /// '아이템 전달' 퀘스트 완료 후 전달된 아이템을 인벤토리에서 제거 
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
    /// 진행 중인 퀘스트 중 '아이템 소지' 타입의 퀘스트가 있다면, 퀘스트 달성 요건 확인 
    /// </summary>
    public void CheckCarryItemQuest()
    {
        for (int i = 0; i < _ongoingQuests.Count; i++)
        {
            if (_ongoingQuests[i].GetQuestType() != QuestType.TYPE_CARRYITEM) continue;

            // 현재 퀘스트가 완료가능 상태인데 요건 충족이 안된 경우 다시 진행중 상태로 변경 
            if (!CheckItemsToCarry(_ongoingQuests[i]))
            {
                if (_ongoingQuests[i].GetState() == QuestState.QUEST_COMPLETABLE) SetQuestFinisherToOngoingState(_ongoingQuests[i]);
                continue;
            }

            // 요건이 충족된 경우 퀘스트 완료자를 완료 가능 상태로 변경 
            SetQuestFinisherToCompletableState(_ongoingQuests[i]);
        }
    }

    /// <summary>
    /// '아이템 소지' 퀘스트의 달성 요건 확인 후, 요건 충족 여부를 bool 값으로 리턴 
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
    /// 진행 중인 퀘스트 중 '몬스터 처치' 타입의 퀘스트가 있다면, 퀘스트 달성 요건 확인 
    /// </summary>
    /// <param name="enemyID"></param>
    public void CheckKillEnemyQuest(int enemyID)
    {
        for (int i = 0; i < _ongoingQuests.Count; i++)
        {
            if (_ongoingQuests[i].GetQuestType() != QuestType.TYPE_KILLENEMY) continue;

            if (CheckEnemiesToKill(_ongoingQuests[i], enemyID))
            {
                SetQuestFinisherToCompletableState(_ongoingQuests[i]);
            }

            // 퀘스트 HUD 업데이트 
            _questHUD.UpdateHUD(_ongoingQuests[i]);
        }
    }

    /// <summary>
    /// '몬스터 처치' 퀘스트의 달성 요건 확인 후, 요건 충족 여부를 bool 값으로 리턴 
    /// </summary>
    /// <param name="quest"></param>
    /// <param name="enemyID"></param>
    /// <returns></returns>
    public bool CheckEnemiesToKill(Quest quest, int enemyID)
    {
        bool isCompleted = false;

        KillEnemy killEnemy = quest.GetQuestInfo()[_questInfoKey] as KillEnemy;

        foreach(EnemyUnit enemy in killEnemy.GetEnemyList())
        {
            if (enemy.GetEnemyID() != enemyID) continue;

            if (enemy.GetCount() > 0)
            {
                Debug.Log(enemy.GetEnemyID() + "번 몬스터 잡은 횟수" + enemy.GetCount());

                enemy.SetCount(enemy.GetCount() - 1);
                if(enemy.GetCount() <= 0) isCompleted = true;
                break; 
            }
        }

        return isCompleted;
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
    /// 퀘스트 완료조건이 미충족되어 퀘스트 완료자의 상태 값을 퀘스트 진행중으로 변경 
    /// </summary>
    /// <param name="quest"></param>
    void SetQuestFinisherToOngoingState(Quest quest)
    {
        _questHUD.TurnOffCompletableIcon();
        _isCompletableIconOn = false;
        quest.SetState(QuestState.QUEST_ONGOING);

        _questMenu.TurnOffCompletableIcon(quest.GetQuestID());

        Debug.Log(quest.GetQuestID() + "번 퀘스트 진행중");

        if (quest.GetQuestFinisher() == null) return;

        quest.GetQuestFinisher().SetOngoingQuestID(quest.GetQuestID());
        quest.GetQuestFinisher().SetQuestState(QuestState.QUEST_ONGOING);
        quest.GetQuestFinisher().SetQuestMark();
    }

    /// <summary>
    /// 퀘스트 완료조건을 충족하여 퀘스트 완료자의 상태 값을 퀘스트 완료가능으로 변경 
    /// </summary>
    /// <param name="state"></param>
    void SetQuestFinisherToCompletableState(Quest quest)
    {
        // 퀘스트 HUD의 완료가능 아이콘 활성화 
        _questHUD.TurnOnCompletableIcon();
        _isCompletableIconOn = true;
        quest.SetState(QuestState.QUEST_COMPLETABLE);

        _questMenu.TurnOnCompletableIcon(quest.GetQuestID());

        Debug.Log(quest.GetQuestID() + "번 퀘스트 완료가능");

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

    /// <summary>
    /// 씬이 초기화될 때 진행 중인 퀘스트를 확인하여 NPC 데이터 및 상태값 업데이트 
    /// </summary>
    public void SyncWithNpcOnStart()
    {
        if (_ongoingQuests.Count <= 0) return;

        foreach (Quest quest in _ongoingQuests)
        {
            SyncWithQuestOnStart(quest);
        }
    }

    /// <summary>
    /// 퀘스트 타입에 맞는 퀘스트 목표를 string 타입으로 리턴 
    /// </summary>
    /// <param name="quest"></param>
    /// <returns></returns>
    public string GetQuestGoal(Quest quest)
    {
        // 완료된 퀘스트일 경우 퀘스트 목표를 그대로 리턴 
        if (quest.GetState() == QuestState.QUEST_COMPLETED) return quest.GetGoal();

        string goal;

        // 진행중일 경우 타입에 맞게 변환하여 리턴 
        switch (quest.GetQuestType())
        {
            case QuestType.TYPE_DELIVERITEM:
                goal = GetDeliverItemGoal(quest);
                break;

            case QuestType.TYPE_KILLENEMY:
                goal = GetKillEnemyGoal(quest);
                break;

            default:
                goal = quest.GetGoal();
                break;
        }

        return goal;
    }

    /// <summary>
    /// '아이템 전달' 타입 퀘스트의 목표 string 타입으로 리턴  
    /// </summary>
    /// <param name="quest"></param>
    /// <returns></returns>
    public string GetDeliverItemGoal(Quest quest)
    {
        DeliverItem deliverItem = quest.GetQuestInfo()[_questInfoKey] as DeliverItem;

        string goal = "";

        for (int i = 0; i < deliverItem.GetItemList().Count; i++)
        {
            Item item = ItemDatabase.instance.GetItem(deliverItem.GetItem(i).GetItemID());

            // 소지중인 개수가 목표 개수보다 클 경우 목표 개수로 표시 
            int carryCount = (_inventory.GetItemCount(item) > deliverItem.GetItem(i).GetCount()) ?
                deliverItem.GetItem(i).GetCount() : _inventory.GetItemCount(item);

            goal += (item.name + " (" + carryCount + "/" + deliverItem.GetItem(i).GetCount() + ")\n");            
        }

        return goal;
    }

    /// <summary>
    /// '몬스터 처치' 타입 퀘스트의 목표 string 타입으로 리턴 
    /// </summary>
    /// <param name="quest"></param>
    /// <returns></returns>
    public string GetKillEnemyGoal(Quest quest)
    {
        KillEnemy killEnemy = quest.GetQuestInfo()[_questInfoKey] as KillEnemy;
        KillEnemy originDB = QuestDB.instance.GetQuest(quest.GetQuestID()).GetQuestInfo()[_questInfoKey] as KillEnemy;

        string goal = "";

        for (int i = 0; i < killEnemy.GetEnemyList().Count; i++)
        {
            // 오리지널 DB로부터 받아온 처치해야 하는 몬스터 수 
            int countToKill = originDB.GetEnemy(i).GetCount();

            // (처치해야 하는 몬스터 수 - 진행중인 퀘스트 데이터의 몬스터 수(처치할 때마다 --))로 처치한 몬스터 수 표기
            goal = EnemyDB.instance.GetName(killEnemy.GetEnemy(i).GetEnemyID()) + " 처치 (" +
                (countToKill - killEnemy.GetEnemy(i).GetCount()) + "/" + countToKill + ")\n";
        }

        return goal;
    }

    /// <summary>
    /// 씬이 시작될 때마다 상호작용하는 객체들을 링크 
    /// </summary>
    public void InitializeLink()
    {
        Debug.Log("InitializeLink 실행");

        _inventory = FindObjectOfType<Inventory>();
        _questHUD = FindObjectOfType<QuestHUD>();
        _questMenu = FindObjectOfType<QuestMenu>();
        _playerStatus = FindObjectOfType<PlayerStatus>();

        if (_isHudOpen) _questHUD.SetIsHudOpen(true);
        if (_isCompletableIconOn) _questHUD.SetIsCompletableIconOn(true);
    }

    /// <summary>
    /// 현재 진행중인 퀘스트 중 파라미터의 ID 값을 가진 퀘스트를 찾아서 리턴 (없을 경우 null 리턴)
    /// </summary>
    /// <param name="questID"></param>
    /// <returns></returns>
    public Quest GetOngoingQuestByID(int questID)
    {
        foreach(Quest quest in _ongoingQuests)
        {
            if (quest.GetQuestID() == questID) return quest; 
        }

        return null;
    }

    public bool SearchCompleteQuestID(int id)
    {
        Quest completeQuest = _finishedQuests.Find(list => list.GetQuestID() == id);

        return completeQuest != null;
    }

    //getter
    public bool GetIsHudOpen() { return _isHudOpen; }
    public List<Quest> GetOngoingQuestList() { return _ongoingQuests; }
    public List<Quest> GetFinishedQuestList() { return _finishedQuests; }
    public Quest GetOngoingQuestByIdx(int idx) { return _ongoingQuests[idx]; }
    public Quest GetFinishedQuestByIdx(int idx) { return _finishedQuests[idx]; }
    public Quest GetOngoingQuestByID() { return _ongoingQuests[0]; }

    //setter
    public void SetIsHudOpen(bool value) { _isHudOpen = value; }

}
