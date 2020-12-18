using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string name;                         //몬스터 이름
    public int hp;                              //몬스터 hp
    public int attDamage;                       //몬스터 대미지
    public int speed;                           //몬스터 속도

    public float attTime;                       //몬스터 공격속도
    public float timer;                         //공격속도 조절값

    Vector3 startPoint;                         //최초 생성 값
    Transform player;                           //공격 목표 (플레이어)
    
    CharacterController characterController;    //몬스터 컨트롤러

    public int findRange;                       //몬스터 인식 범위
    public int moveRange;                       //몬스터 추적 범위
    public int attackRange;                     //몬스터 공격 범위 

    public enum State
    {
        Die, Move, Idle, Attack, Return, Damaged
    }
    State enemyState;

    private void Start()
    {
        enemyState = State.Idle;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        characterController = GetComponent<CharacterController>();
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
        }
    } 

    //기본 상태
    private void UpdateIdle()
    {
        if(Vector3.Distance(transform.position, player.position) < findRange)
        {
            enemyState = State.Move;
            Debug.Log("Move상태 전환");
        }
    }
    //무브 상태
    private void UpdateMove()
    {
        if (Vector3.Distance(transform.position, startPoint) > moveRange)
        {
            enemyState = State.Return;
            Debug.Log("Return상태 전환");
        }

        else if(Vector3.Distance(transform.position, player.position) > attackRange)
        {
            Vector3 dir = (player.position - transform.position).normalized;

            transform.rotation = Quaternion.Lerp(transform.rotation
                , Quaternion.LookRotation(dir), 10 * Time.deltaTime);

            characterController.SimpleMove(dir * speed);
        }

        else
        {
        
            enemyState = State.Attack;
            Debug.Log("attack상태 전환");
        }
    }
    //공격 상태
    private void UpdateAttack()
    {
        if (Vector3.Distance(transform.position, player.position) < attackRange)
        {
            timer += Time.deltaTime;
            if (timer > attTime)
            {
                Debug.Log("공격");

                timer = 0.0f;
            }
        }

        else
        {
            enemyState = State.Move;
            Debug.Log("move상태 전환");

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
        characterController.enabled = false;

        yield return new WaitForSeconds(2.0f);

        gameObject.SetActive(false); // 삭제하지않고 false 처리 한다.
    }
    //복귀 상태
    private void UpdateReturn()
    {
        if(Vector3.Distance(transform.position, startPoint) > 0.1)
        {
            Vector3 dir = (startPoint - transform.position).normalized;

            characterController.SimpleMove(dir * speed);
        }
        else
        {
            transform.position = startPoint;
            enemyState = State.Idle;
            Debug.Log("Idle상태 전환");
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
        //공격가능 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        //인식 범위
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, findRange);
        //최대 추적 범위
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(startPoint, moveRange);
    }
}
