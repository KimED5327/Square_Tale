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

    [Header("퀘스트 메뉴 Panel")]
    [SerializeField] GameObject _menuPanel;         // 퀘스트 메뉴 패널 
    [SerializeField] GameObject _infoPanel;         // 퀘스트 정보 패널 
    [SerializeField] Transform _rewardPanel;       // 퀘스트 리워드 패널 

    [Header("스크롤 UI")]
    [SerializeField] GameObject _ongoingSlotPrefab;     // 진행 퀘스트 슬롯 프리팹 
    [SerializeField] GameObject _finishedSlotPrefab;    // 완료 퀘스트 슬롯 프리팹 
    [SerializeField] ScrollRect _scrollRect;            // 스크롤 랙트(컨텐트 교체용)
    [SerializeField] Transform _ongoingContent;         // 진행 슬롯 컨텐트
    [SerializeField] Transform _finishedContent;        // 완료 슬롯 컨텐트 

    [Header("퀘스트 메뉴 UI")]
    [SerializeField] Image _imgOngoing;             // 진행 버튼 이미지  
    [SerializeField] Image _imgFinished;            // 완료 버튼 이미지 
    [SerializeField] Sprite _imgHighlightBtn;       // 밝은 버튼 이미지 
    [SerializeField] Sprite _imgDefaultBtn;         // 기본 버튼 이미지 
    [SerializeField] Text _txtTitle;                // 퀘스트 제목 
    [SerializeField] Text _txtNpcName;              // NPC 이름  
    [SerializeField] Text _txtDes;                  // 퀘스트 내용  
    [SerializeField] Text _txtGoal;                 // 퀘스트 목표 
    [SerializeField] Text _txtNoSolotMsg;           // 슬롯 메시지 

    [Header("퀘스트 보상 UI")]
    [SerializeField] GameObject _rewardPrefab;      // 퀘스트 리워드 프리팹
    [SerializeField] Sprite _imgGold;               // 골드 보상 이미지 
    [SerializeField] Sprite _imgExp;                // 경험치 보상 이미지 
    [SerializeField] Sprite _imgKeyword;            // 키워드 보상 이미지 

    BlockManager _blockManager;
    QuestSort _questSort;                           // 퀘스트 분류(진행/완료)

    List<QuestSlot> _ongoingSlots = new List<QuestSlot>();      // 진행 퀘스트 슬롯 리스트 
    List<QuestSlot> _finishedSlots = new List<QuestSlot>();     // 완료 퀘스트 슬롯 리스트 


    void Start()
    {
        SetMenuOnStart();
        _blockManager = FindObjectOfType<BlockManager>();
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
        // 진행중인 퀘스트를 슬롯에 추가 
        for (int i = 0; i < QuestManager.instance.GetOngoingQuestList().Count; i++)
        {
            QuestSlot slot = Instantiate(_ongoingSlotPrefab, _ongoingContent).GetComponent<QuestSlot>();

            Quest quest = QuestManager.instance.GetOngoingQuestByIdx(i);
            if (quest.GetState() == QuestState.QUEST_COMPLETABLE) slot.TurnOnCompletableIcon();

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
        _questSort = QuestSort.ONGOING;

        _scrollRect.content = _ongoingContent.GetComponent<RectTransform>();
        _ongoingContent.gameObject.SetActive(true);
        _finishedContent.gameObject.SetActive(false);

        _imgOngoing.sprite = _imgDefaultBtn;
        _imgFinished.sprite = _imgHighlightBtn;

        // 진행 중인 퀘스트가 있을 경우 정보 출력 
        if (_ongoingSlots.Count > 0)
        {
            _infoPanel.SetActive(true);
            _txtNoSolotMsg.gameObject.SetActive(false);
            SetQuestInfo(_ongoingSlots[0].GetQuest());
            ShowQuestRewards(_ongoingSlots[0].GetQuest());
        }
        else // 진행 중인 퀘스트가 없을 경우 메시지 출력 
        {
            _infoPanel.SetActive(false);
            _txtNoSolotMsg.gameObject.SetActive(true);
            _txtNoSolotMsg.text = "진행 중인 퀘스트가 없습니다.";
        }
    }

    /// <summary>
    /// 완료 퀘스트 메뉴 출력 
    /// </summary>
    public void ShowFinishedQuest()
    {
        _questSort = QuestSort.FINISHED;

        _scrollRect.content = _finishedContent.GetComponent<RectTransform>();
        _finishedContent.gameObject.SetActive(true);
        _ongoingContent.gameObject.SetActive(false);

        _imgOngoing.sprite = _imgHighlightBtn;
        _imgFinished.sprite = _imgDefaultBtn;

        // 진행 중인 퀘스트가 있을 경우 정보 출력 
        if (_finishedSlots.Count > 0)
        {
            _infoPanel.SetActive(true);
            _txtNoSolotMsg.gameObject.SetActive(false);
            SetQuestInfo(_finishedSlots[0].GetQuest());
            ShowQuestRewards(_finishedSlots[0].GetQuest());

            // 슬롯이 2개 이상일 경우 선택/비선택 슬롯의 색상을 나누어 설정 
            if (_finishedSlots.Count > 1)
            {
                SetFinishedSlotsDefault();
                _finishedSlots[0].TurnOffColor();
            }
        }
        else // 진행 중인 퀘스트가 없을 경우 메시지 출력 
        {
            _infoPanel.SetActive(false);
            _txtNoSolotMsg.gameObject.SetActive(true);
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
    /// 퀘스트 슬롯으로부터 넘겨받은 리워드 데이터를 화면에 출력 
    /// </summary>
    /// <param name="rewards"></param>
    public void ShowQuestRewards(Quest quest)
    {
        // 기존의 모든 보상 삭제 
        DeleteAllRewards();

        AddGoldReward(quest);
        AddExpReward(quest);

        // 아이템 보상이 존재하는 경우 보상 추가 
        if (quest.GetItemID() != 0) AddItemReward(quest);

        // 블록 보상이 존재하는 경우 보상 추가 
        if (quest.GetBlockList().Count > 0) AddBlockReward(quest);

        // 키워드 보상이 존재하는 경우 보상 추가 
        if (quest.GetKeywordList().Count > 0) AddKeywordReward(quest);
    }

    /// <summary>
    /// 리워드 패널의 자식 객체 모두 삭제 
    /// </summary>
    public void DeleteAllRewards()
    {
        foreach (Transform child in _rewardPanel)
        {
            Destroy(child.gameObject);
        }
    }

    // 골드 보상을 리워드 패널에 추가 
    void AddGoldReward(Quest quest)
    {
        QuestReward reward = Instantiate(_rewardPrefab, _rewardPanel).GetComponent<QuestReward>();

        reward.SetImg(_imgGold);
        reward.SetCount(quest.GetGold());
        reward.SetName("골드");
    }

    // 경험치 보상을 리워드 패널에 추가 
    void AddExpReward(Quest quest)
    {
        QuestReward reward = Instantiate(_rewardPrefab, _rewardPanel).GetComponent<QuestReward>();

        reward.SetImg(_imgExp);
        reward.SetCount(quest.GetExp());
        reward.SetName("경험치");
    }

    // 아이템 보상을 리워드 패널에 추가 
    void AddItemReward(Quest quest)
    {
        QuestReward reward = Instantiate(_rewardPrefab, _rewardPanel).GetComponent<QuestReward>();

        reward.SetImg(SpriteManager.instance.GetItemSprite(quest.GetItemID()));
        reward.SetCount(1);
        reward.SetName(ItemDatabase.instance.GetItem(quest.GetItemID()).name);
    }

    // 블록 보상을 리워드 패널에 추가 
    void AddBlockReward(Quest quest)
    {
        for (int i = 0; i < quest.GetBlockList().Count; i++)
        {
            QuestReward reward = Instantiate(_rewardPrefab, _rewardPanel).GetComponent<QuestReward>();

            reward.SetImg(SpriteManager.instance.GetBlockSprite(quest.GetBlock(i).GetBlockID()));
            reward.GetImg().GetComponent<RectTransform>().sizeDelta = new Vector2(100f, 100f);
            reward.SetCount(quest.GetBlock(i).GetCount());
            reward.SetName(_blockManager.GetBlockNameID(quest.GetBlock(i).GetBlockID()));
        }
    }

    // 키워드 보상을 리워드 패널에 추가 
    void AddKeywordReward(Quest quest)
    {
        QuestReward reward = Instantiate(_rewardPrefab, _rewardPanel).GetComponent<QuestReward>();

        reward.SetImg(_imgKeyword);
        reward.TurnOffCount();

        string name = "키워드 (";
        name += (KeywordData.instance.GetKeyword(quest.GetKeywordList()[0]).keyword + ")");
        reward.SetName(name);
    }

    /// <summary>
    /// 모든 완료 슬롯을 기본 이미지 색상으로 설정 
    /// </summary>
    public void SetFinishedSlotsDefault()
    {
        switch (_questSort)
        {
            case QuestSort.ONGOING:
                //for (int i = 0; i < _ongoingSlots.Count; i++)
                //{
                //    _ongoingSlots[i].TurnOnColor();
                //}
                break;

            case QuestSort.FINISHED:
                for (int i = 0; i < _finishedSlots.Count; i++)
                {
                    _finishedSlots[i].TurnOnColor();
                }
                break;
        }
    }

    /// <summary>
    /// 현재 분류에 맞는 퀘스트 슬롯의 개수를 반환 
    /// </summary>
    public int CheckSlotsCount()
    {
        int count = 0;

        switch (_questSort)
        {
            case QuestSort.ONGOING:
                count = _ongoingSlots.Count;
                break;

            case QuestSort.FINISHED:
                count = _finishedSlots.Count;
                break;
        }

        return count;
    }

    /// <summary>
    /// 완료 가능한 퀘스트 슬롯에 완료 가능 아이콘 활성화 
    /// </summary>
    /// <param name="quest"></param>
    public void TurnOnCompletableIcon(int questID)
    {
        for (int i = 0; i < _ongoingSlots.Count; i++)
        {
            if (_ongoingSlots[i].GetQuest().GetQuestID() != questID) continue;

            _ongoingSlots[i].TurnOnCompletableIcon();
        }
    }

    /// <summary>
    /// 퀘스트 완료 조건 미충족 시 퀘스트 슬롯에 완료 가능 아이콘 비활성화 
    /// </summary>
    /// <param name="questID"></param>
    public void TurnOffCompletableIcon(int questID)
    {
        for (int i = 0; i < _ongoingSlots.Count; i++)
        {
            if (_ongoingSlots[i].GetQuest().GetQuestID() != questID) continue;

            _ongoingSlots[i].TurnOffCompletableIcon();
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
