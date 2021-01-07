﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        _playerPos = FindObjectOfType<Player>().transform;
    }



    // Update is called once per frame
    void Update()
    {
        if (_isOpen) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // 터치 상호작용 시도
            if (Physics.Raycast(ray, out RaycastHit hit, 1000, _layerMask))
            {

                // 1.5블록 이내일 경우
                if (Vector3.SqrMagnitude(_playerPos.position - hit.transform.position) < Mathf.Pow(_interactionRange, 2)) {

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
                        hit.transform.gameObject.SetActive(false);
                    }

                    else if (hit.transform.CompareTag(StringManager.questNPCTag))
                    {
                        //hit.transform.GetComponent<QuestNPC>().ClickNPC();
                        hit.transform.GetComponent<ZoomNPC>().ZoomInNPC();
                    }
                }

                // 거리가 너무 멀 때
                else
                {
                    Notification.instance.ShowFloatingMessage(StringManager.msgLongDistance);
                }
            }
        }
    }
}
