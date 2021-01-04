using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum State
{
    Idle, Move, Die, Attack, Return, Damaged, Search
}

public class Enemy : MonoBehaviour
{
    GameObject rangedWeapon;                    //몬스터 원거리 구체
    public float attTime;                       //몬스터 공격속도
    public float timer;                         //공격속도 조절값
    public float reconTime;                     //정찰 시작 시간
    public float reconTimer;                    //정찰 체크 시간
    private float dieTime;                      //죽고 난 뒤 시간

    Vector3 startPoint;                         //최초 생성 값
    Transform player;                           //공격 목표 (플레이어)
    NavMeshAgent agent;                         //AI컨트롤러


    public int maxFindRange;                    //몬스터 인식 범위
    public int maxMoveRange;                    //몬스터 추적 범위
    public int maxAttackRange;                  //몬스터 근접 공격 범위
    public bool isLongMonster;                  //원거리 몬스터인가?
    public int maxLongAttackRange;              //몬스터 원거리 공격 범위
    public int reconRange;                      //정찰 범위

    private bool _isChasing = false;
    private bool jump;
    private int jumpCount;
    Animator enemyAnimator;                     //몬스터 애니메이터
    BoxCollider myColider;
    Rigidbody myRigid;


    EnemyStatus status;
    Vector3     _offset;
    Vector3     _rayPos;
    Vector3     _rayPos2;

    public State enemyState;

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
        status = GetComponent<EnemyStatus>();
        myColider = GetComponent<BoxCollider>();
        myRigid = GetComponent<Rigidbody>();
    }

    //에너미 업데이트
    private void Update()
    {
        if(!status.IsDead() && !jump)
        {
            switch (enemyState)
            {
                case State.Idle:
                    UpdateIdle();
                    break;
                case State.Move:
                    UpdateMove();
                    break;
                case State.Attack:
                    UpdateAttack();
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
        else if(jump)
        {
            if(jumpCount == 0)
            {
                agent.enabled = false;
                myRigid.AddForce(Vector3.up * 6f, ForceMode.Impulse);
                myRigid.AddForce(transform.forward * 2f, ForceMode.Impulse);
                jumpCount++;
            }
            if(myRigid.velocity.y < 0)
            {
                myColider.isTrigger = false;
            }
        }
        else
        {
            UpdateDie();
        }    
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Floor"))
        {
            if(jump)
            {
                agent.enabled = true;
                jump = false;
                jumpCount = 0;
            }
        }
    }
    private void UpdateSearch()
    {
        _offset = new Vector3(0f, transform.position.y + 1, 0f);
        Vector3 move = Vector3.forward;

        float x = Random.Range((float)startPoint.x - maxMoveRange, (float)startPoint.x + maxMoveRange);
        float z = Random.Range((float)startPoint.z - maxMoveRange, (float)startPoint.z + maxMoveRange);
        move = new Vector3(x, 0f, z);

        Debug.DrawRay(move + _offset, Vector3.down, Color.yellow, 100);

        if (Physics.Raycast(move + _offset, Vector3.down, out RaycastHit hit, 2f))
        {
            if (hit.transform.CompareTag("Floor"))
            {
                _rayPos = hit.transform.position;

                agent.SetDestination(_rayPos);
                reconTimer = 0;

               
                enemyState = State.Move;


               
     

            }
        }
    }

    //기본 상태
    private void UpdateIdle()
    {
        reconTimer += Time.deltaTime;
        if(reconTimer > reconTime)
        {
            enemyState = State.Search;
        }
        if (Vector3.SqrMagnitude(transform.position - player.position) < Mathf.Pow(maxFindRange, 2))
        {
            enemyState = State.Move;
        }
    }
    //무브 상태
    private void UpdateMove()
    {



        if (Vector3.SqrMagnitude(transform.position - startPoint) > Mathf.Pow(maxMoveRange,2))
        {
            enemyState = State.Return;
          
        }

        else if(Vector3.SqrMagnitude(transform.position - player.position) < Mathf.Pow(maxFindRange,2))
        {


            _offset = new Vector3(0f, 0.3f, 0f);
            enemyAnimator.SetBool("Move", true);
            if (Physics.Raycast(transform.position + _offset, transform.forward, out RaycastHit hit, 1.5f))
            {
                if (hit.transform.CompareTag("Floor") && !jump)
                {
                    
                    myColider.isTrigger = true;
                    jump = true;
                }
            }
            else
            {
                _isChasing = true;
                agent.SetDestination(player.transform.position);

            }

            //enemyAnimator.SetInteger("animation", 2);

        }

        if (Vector3.SqrMagnitude(transform.position - player.position) < Mathf.Pow(maxAttackRange, 2))
        {
            // enemyAnimator.SetInteger("animation", 3);
            enemyState = State.Attack;
            enemyAnimator.SetBool("Attack", true);
        }
        if(!_isChasing)
        {
            if (Vector3.SqrMagnitude(transform.position - agent.destination) < 0.25f)
            {
                enemyState = State.Idle;
            }

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
            //enemyAnimator.SetInteger("animation", 2);

            enemyAnimator.SetBool("Attack", false);
            enemyState = State.Move;

            timer = 0.0f;
        }
    }
    //사망 상태
    private void UpdateDie()
    {
    
       // enemyAnimator.SetInteger("animation", 5);

        enemyAnimator.SetBool("Die", true);

        //myRigid.isKinematic = true;
        myColider.isTrigger = true;
        myRigid.useGravity = false;
        agent.enabled = false;
        dieTime += Time.deltaTime;
        if(dieTime >= 60)
        {
            string name = GetComponent<EnemyStatus>().GetName();

            
            ObjectPooling.instance.PushObjectToPool(name,this.gameObject);
        }
        
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
            //enemyAnimator.SetInteger("animation", 1);

            enemyAnimator.SetBool("Move", false);
            agent.ResetPath();
            _isChasing = false;
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

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(startPoint, reconRange);
    }
}
