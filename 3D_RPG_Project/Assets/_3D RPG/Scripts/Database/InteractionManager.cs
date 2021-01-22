using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionManager : MonoBehaviour
{
    public static bool _isOpen = false;

    [SerializeField] string _keywordEffectName = "키워드 획득";
    [SerializeField] float _interactionRange = 1.5f;

    [SerializeField] LayerMask _layerMask = 0;

    Shop _shop;
    Rooting _rootingSystem;
    Inventory _inven;
    Transform _playerPos;

    // Start is called before the first frame update
    void Start()
    {
        _inven = FindObjectOfType<Inventory>();
        _rootingSystem = FindObjectOfType<Rooting>();
        _shop = FindObjectOfType<Shop>();
        _playerPos = FindObjectOfType<PlayerMove>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isOpen) return;

        if (Input.GetMouseButtonUp(0))
        {
            TouchBoard.isPress = false; // 카메라 드래그 취소

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // 터치 상호작용 시도
            if (Physics.Raycast(ray, out RaycastHit hit, 1000, _layerMask))
            {
                // UI에 부딪친 경우 취소
                if (EventSystem.current.IsPointerOverGameObject() == true) return;

                Transform target = hit.transform;

                // 1.5블록 이내일 경우
                if (Vector3.SqrMagnitude(_playerPos.position - target.position) < Mathf.Pow(_interactionRange, 2))
                {
                    // Enemy - 루팅
                    if (target.CompareTag(StringManager.fieldItemTag))
                    {
                        _isOpen = _rootingSystem.TryRooting(target.GetComponent<FieldItem>());
                    }

                    // Shop NPC - 상점 호출
                    else if (target.CompareTag(StringManager.shopNpcTag))
                    {
                        _shop.CallMenu();
                        _isOpen = true;
                    }

                    // Treasure - 상자 오픈
                    else if (target.CompareTag(StringManager.keywordTag))
                    {
                        StartCoroutine(CheckTreausreChest(target.GetComponent<Chest>()));
                    }


                    // Quest NPC - 다이얼로그 실행 
                    else if (target.CompareTag(StringManager.questNPCTag))
                    {
                        QuestNPC npc = target.GetComponent<QuestNPC>();
                        ZoomNPC zoom = target.GetComponent<ZoomNPC>();

                        // 퀘스트 진행중일 시 해당 상태에 대한 대사 유무를 체크해서 대화 실행 
                        if (npc.GetQuestState() == QuestState.QUEST_ONGOING)
                        {
                            if (!hit.transform.GetComponent<QuestNPC>().CheckOngoingQuestDialogue()) return;
                        }
                        npc.TurnOffNameTag();
                        zoom.ZoomInNPC();
                    }

                    // 트리거 상호작용 액티브.
                    else if (target.CompareTag(StringManager.TriggerTag))
                    {
                        Trigger trigger = target.GetComponent<Trigger>();
                        if(trigger.CanTouchActive())
                            trigger.ActiveTrigger();
                    }

                    // 거리가 너무 멀 때
                    else
                    {
                        if (target.CompareTag(StringManager.fieldItemTag))
                        {
                            Notification.instance.ShowFloatingMessage(StringManager.msgLongDistance);
                        }
                        else if (target.CompareTag(StringManager.shopNpcTag))
                        {
                            Notification.instance.ShowFloatingMessage(StringManager.msgLongDistance);
                        }
                        else if (target.CompareTag(StringManager.keywordTag))
                        {
                            Notification.instance.ShowFloatingMessage(StringManager.msgLongDistance);
                        }
                        else if (target.CompareTag(StringManager.questNPCTag))
                        {
                            Notification.instance.ShowFloatingMessage(StringManager.msgLongDistance);
                        }
                    }
                }
            }
        }

        // 보물 상자 체크
        IEnumerator CheckTreausreChest(Chest chest)
        {
            Reward reward = chest.GetReward();
            if (reward == null) yield break;

            yield return new WaitForSeconds(1.25f);

            switch (reward.rewardType)
            {
                // 키워드 획득 보상
                case RewardType.KEYWORD:
                    ObjectPooling.instance.GetObjectFromPool(_keywordEffectName, _playerPos.position);
                    KeywordData.instance.AcquireKeyword(reward.id);
                    _playerPos.GetComponent<PlayerMove>().Victory();
                    break;


                // 모든 블록 이용권 획득
                case RewardType.ALLBLOCK:
                    BlockManager.AllIncreaseBlockCount(reward.count);
                    Notification.instance.ShowBlockText();
                    break;


                // 특정 블록 이용권 획득
                case RewardType.BLOCK:
                    Notification.instance.ShowBlockText();
                    BlockManager.IncreaseBlockCount(reward.id, reward.count);
                    break;


                // 데미지 함정
                case RewardType.TRAP:
                    Status playerStatus = _playerPos.GetComponent<Status>();

                    int playerHp = playerStatus.GetMaxHp();
                    float dmgRate = (reward.count / (float)100);
                    int damage = (int)(playerHp * dmgRate);

                    playerStatus.Damage(damage, Vector3.zero);
                    break;


                // 골드 획득 (안쓰임)
                case RewardType.GOLD:
                    Item item = ItemDatabase.instance.GetItem(reward.id);
                    int gold = reward.count;
                    _inven.TryToPushInventory(item, gold);
                    Notification.instance.ShowAllItemNotice(gold);
                    break;

                // 뒤늦게 추가된 부분이라 귀찮아서 하드코딩
                case RewardType.All:
                    _inven.TryToPushInventory(1, 600); // 600 골드
                    PlayerStatus ps = _playerPos.GetComponent<PlayerStatus>();
                    ps.IncreaseExp(50); // 50 경험치
                    BlockManager.AllIncreaseBlockCount(10); // 10 All 블록 추가
                    Notification.instance.ShowAllItemNotice(0);
                    break;

                // 미처리
                default:
                    Debug.LogError("정의되지 않은 보물상자 유형!!");
                    break;
            }

            chest.RemoveChest();
        }
    }
}
