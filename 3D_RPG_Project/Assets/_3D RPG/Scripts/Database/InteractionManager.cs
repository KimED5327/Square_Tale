using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.EventSystems;

public class InteractionManager : MonoBehaviour
{
    public static bool _isOpen = false;

    [SerializeField] string _keywordEffectName = "키워드 획득";
    [SerializeField] float _interactionRange = 1.5f;

    [SerializeField] LayerMask _layerMask = 0;

    Shop _shop;
    Rooting _rootingSystem;
    Transform _playerPos;

    // Start is called before the first frame update
    void Start()
    {
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // 터치 상호작용 시도
            if (Physics.Raycast(ray, out RaycastHit hit, 1000, _layerMask))
            {
                Transform target = hit.transform;
                //if (EventSystem.current.IsPointerOverGameObject() == true) return;

                // 1.5블록 이내일 경우
                if (Vector3.SqrMagnitude(_playerPos.position - target.position) < Mathf.Pow(_interactionRange, 2))
                {
                    // Enemy - 루팅
                    if (target.CompareTag(StringManager.enemyTag))
                    {
                        _isOpen = _rootingSystem.TryRooting(target.GetComponent<EnemyStatus>());
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
                        CheckTreausreChest(target.GetComponent<Chest>());
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
                        target.GetComponent<Trigger>().ActiveTrigger();
                    }

                    // 거리가 너무 멀 때
                    else
                    {
                        if (target.CompareTag(StringManager.enemyTag))
                        {
                            if (target.GetComponent<EnemyStatus>().IsDead())
                            {
                                Notification.instance.ShowFloatingMessage(StringManager.msgLongDistance);
                            }
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
        void CheckTreausreChest(Chest chest)
        {
            Reward reward = chest.GetReward();

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
                    int damage = playerStatus.GetMaxHp() * (reward.count / 100);
                    int playerHp = playerStatus.GetCurrentHp() - damage;
                    if (playerHp <= 0)
                        playerHp = 1;
                    playerStatus.SetCurrentHp(playerHp);
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
