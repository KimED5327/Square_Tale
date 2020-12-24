﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public string enemyName;                    //몬스터 이름
    public int hp;                              //몬스터 hp
    public int attDamage;                       //몬스터 대미지
    public int speed;                           //몬스터 속도

    GameObject rangedWeapon;                    //몬스터 원거리 구체
    public float attTime;                       //몬스터 공격속도
    public float timer;                         //공격속도 조절값

    Vector3 startPoint;                         //최초 생성 값
    Transform player;                           //공격 목표 (플레이어)
    
    NavMeshAgent agent;                         //AI컨트롤러


    public int maxFindRange;                    //몬스터 인식 범위
    public int maxMoveRange;                    //몬스터 추적 범위
    public int maxAttackRange;                  //몬스터 근접 공격 범위
    public bool isLongMonster;                  //원거리 몬스터인가?
    public int maxLongAttackRange;              //몬스터 원거리 공격 범위

    Animator enemyAnimator;                     //몬스터 애니메이터
    

    public enum State
    {
        Die, Move, Idle, Attack, Return, Damaged, Search
    }
    State enemyState;

    bool IsPlaying(string stateName)
    {
        if (enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName(stateName) &&
                enemyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            return true;
        else
            return false;
    }


private void Start()
    {
        enemyState = State.Idle;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyAnimator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        startPoint = transform.position;

        
    }

    //에너미 업데이트
    private void Update()
    {
        switch(enemyState)
        {
            case State.Idle :
                UpdateIdle();
                break;
            case State.Move:
                UpdateMove();
                break;
            case State.Attack:
                UpdateAttack();
                break;
            case State.Die :
                UpdateDie();
                break;
            case State.Return:
                UpdateReturn();
                break;
            case State.Damaged:
                UpdateDamaged();
                break;
            case State.Search:
                UpdateSearch();
                break;
               
        }
    } 
    private void UpdateSearch()
    {

    }

    //기본 상태
    private void UpdateIdle()
    {
        if(Vector3.SqrMagnitude(transform.position - player.position) < Mathf.Pow(maxFindRange, 2))
        {
            enemyState = State.Move;
            Debug.Log("Move상태 전환");
        }
    }
    //무브 상태
    private void UpdateMove()
    {
        if (Vector3.SqrMagnitude(transform.position - startPoint) > Mathf.Pow(maxMoveRange,2))
        {
            enemyState = State.Return;
          
        }

        else if(Vector3.SqrMagnitude(transform.position - player.position) > Mathf.Pow(maxAttackRange,2))
        {
            enemyAnimator.SetBool("Move", true);
            agent.SetDestination(player.transform.position);
        }

        else
        {
        
            enemyState = State.Attack;
            enemyAnimator.SetBool("Attack", true);
    
        }
    }
    //공격 상태
    private void UpdateAttack()
    {
        if (Vector3.SqrMagnitude(transform.position - player.position) < Mathf.Pow(maxAttackRange,2))
        {

            agent.ResetPath();
            timer += Time.deltaTime;
            if (timer > attTime)
            {
      

                timer = 0.0f;
            }
        }

        else
        {
            enemyAnimator.SetBool("Attack", false);
            enemyState = State.Move;
 

            timer = 0.0f;
        }
    }
    //사망 상태
    private void UpdateDie()
    {
        StopAllCoroutines();

        StartCoroutine(DieProc());
    }

    IEnumerator DieProc()
    {

        yield return new WaitForSeconds(2.0f);

        gameObject.SetActive(false); // 삭제하지않고 false 처리 한다.
    }
    //복귀 상태
    private void UpdateReturn()
    {
        if(Vector3.SqrMagnitude(transform.position - startPoint) > 0.01)
        {
            agent.SetDestination(startPoint);
        }
        else
        {
            enemyAnimator.SetBool("Move", false);
            agent.ResetPath();
            transform.position = startPoint;
            enemyState = State.Idle;

        }
    }
    //피격 상태
    private void UpdateDamaged()
    {
        StartCoroutine(DamageProc()); //대미지 처리용 코루틴
    }

    IEnumerator DamageProc()
    {
        yield return new WaitForSeconds(1.0f);

        enemyState = State.Move;
    }


    public void hitDamage(int value)
    {
        hp -= value;

        if(hp > 0)
        {
            enemyState = State.Damaged;
        }
        else
        {
            enemyState = State.Die;
            hp = 0;
        }
    }


    private void OnDrawGizmos()
    {
        //원거리 공격 가능 범위
        if(isLongMonster)
        {
          Gizmos.color = Color.yellow;
          Gizmos.DrawWireSphere(transform.position, maxLongAttackRange);
        }

        //근접 공격 가능 범위
        if(!isLongMonster)
        {
          Gizmos.color = Color.red;
          Gizmos.DrawWireSphere(transform.position, maxAttackRange);
        }

        //인식 범위
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, maxFindRange);
        //최대 추적 범위
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(startPoint, maxMoveRange);
    }
}