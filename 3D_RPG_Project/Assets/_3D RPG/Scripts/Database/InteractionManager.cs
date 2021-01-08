using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.EventSystems;

public class InteractionManager : MonoBehaviour
{
    public static bool _isOpen = false;

    [SerializeField] string _keywordEffectName = "키워드 획득";
    [SerializeField] float _interactionRange = 1.5f;

    [SerializeField] GameObject _goTouchBoard = null;
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

                //if (EventSystem.current.IsPointerOverGameObject() == true) return;

                // 1.5블록 이내일 경우
                if (Vector3.SqrMagnitude(_playerPos.position - hit.transform.position) < Mathf.Pow(_interactionRange, 2))
                {

                    // Enemy - 루팅
                    if (hit.transform.CompareTag(StringManager.enemyTag))
                    {
                        _isOpen = _rootingSystem.TryRooting(hit.transform.GetComponent<EnemyStatus>());
                    }

                    // Shop NPC - 상점 호출
                    else if (hit.transform.CompareTag(StringManager.shopNpcTag))
                    {
                        _shop.CallMenu();
                        _isOpen = true;
                    }

                    // Keyword - 필드 키워드 획득
                    else if (hit.transform.CompareTag(StringManager.keywordTag))
                    {
                        ObjectPooling.instance.GetObjectFromPool(_keywordEffectName, _playerPos.position);

                        KeywordData.instance.AcquireKeyword(1);
                        _playerPos.GetComponent<PlayerMove>().Victory();
                        hit.transform.gameObject.SetActive(false);
                    }

                    // Quest NPC - 다이얼로그 실행 
                    else if (hit.transform.CompareTag(StringManager.questNPCTag))
                    {
                        QuestNPC npc = hit.transform.GetComponent<QuestNPC>();

                        // 퀘스트 진행중일 시 해당 상태에 대한 대사 유무를 체크해서 함수 실행 
                        if(hit.transform.GetComponent<QuestNPC>().GetQuestState() == QuestState.QUEST_ONGOING)
                        {
                            if (!hit.transform.GetComponent<QuestNPC>().CheckOngoingQuestDialogue()) return; 
                        }

                        hit.transform.GetComponent<QuestNPC>().TurnOffNameTag();
                        hit.transform.GetComponent<ZoomNPC>().ZoomInNPC();
                    }
                }

                // 거리가 너무 멀 때
                else
                {
                    if (hit.transform.CompareTag(StringManager.enemyTag))
                    {
                        if (hit.transform.GetComponent<EnemyStatus>().IsDead())
                        {
                            Notification.instance.ShowFloatingMessage(StringManager.msgLongDistance);
                        }
                    }
                    else if (hit.transform.CompareTag(StringManager.shopNpcTag))
                    {
                        Notification.instance.ShowFloatingMessage(StringManager.msgLongDistance);
                    }
                    else if (hit.transform.CompareTag(StringManager.keywordTag))
                    {
                        Notification.instance.ShowFloatingMessage(StringManager.msgLongDistance);
                    }
                    else if (hit.transform.CompareTag(StringManager.questNPCTag))
                    {
                        Notification.instance.ShowFloatingMessage(StringManager.msgLongDistance);
                    }
                }
            }
        }
    }
}
