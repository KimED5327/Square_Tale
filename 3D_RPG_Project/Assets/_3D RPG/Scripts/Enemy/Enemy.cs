using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum State
{
    Idle, Move, Die, Attack, Return, Search
}

public class Enemy : MonoBehaviour
{
    public int Id;                              //몬스터 idNumber;
    public float attTime;                       //몬스터 공격속도
    public float dmgApplyTime;                  //실제 데미지 적용 시간.
    public float timer;                         //공격속도 조절값
    public float reconTime;                     //정찰 시작 시간
    public float reconTimer;                    //정찰 체크 시간
    private float dieTime;                      //죽고 난 뒤 시간
    public float motionTime;                   //공격 모션 시간
    private bool searchCount = true;                    

    Vector3 startPoint;                         //최초 생성 값
    Transform player;                           //공격 목표 (플레이어)
    NavMeshAgent agent;                         //AI컨트롤러


    public int maxFindRange;                    //몬스터 인식 범위
    public int maxMoveRange;                    //몬스터 추적 범위
    public float maxAttackRange;                  //몬스터 근접 공격 범위
    public int reconRange;                      //정찰 범위

    private bool _isDie = false;
    private bool _canApplyDamage = false;
    private bool _isChasing = false;
    private bool jump;
    private int jumpCount;
    Animator enemyAnimator;                     //몬스터 애니메이터
    BoxCollider myColider;
    Rigidbody myRigid;
    PlayerMove playerMove;

    EnemyStatus status;
    Vector3 _offset;
    Vector3 _rayPos;
    Vector3 _rayPos2;

    public State enemyState;



    bool IsPlaying(string stateName)
    {
        if (enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName(stateName) &&
                enemyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            return true;
        else
            return false;
    }

    private void OnEnable()
    {
        Initialized();
    }

    private void Initialized()
    {
        enemyState = State.Idle;
        _isDie = false;

        if (agent != null)
            agent.enabled = true;

        transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
        startPoint = transform.position;

        if (playerMove == null)
            playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
    }

    private void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        status = GetComponent<EnemyStatus>();
        myColider = GetComponent<BoxCollider>();
        myRigid = GetComponent<Rigidbody>();
        status.SetCurrentHp(status.GetMaxHp());

    }

    public void LinkPlayer(Transform tfPlayer)
    {
        player = tfPlayer;
    }

    //에너미 업데이트


    private void Update()
    {
        if (!status.IsDead() && !jump)
        {
            // 플레이어가 비활성화 상태라면 리턴 
            if (!player.gameObject.activeInHierarchy) return;

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
                case State.Search:
                    UpdateSearch();
                    break;
            }

        }
        else if (jump)
        {
            if (jumpCount == 0)
            {
                agent.enabled = false;
                myRigid.AddForce(Vector3.up * 6f, ForceMode.Impulse);
                myRigid.AddForce(transform.forward * 2f, ForceMode.Impulse);
                jumpCount++;
            }
            if (myRigid.velocity.y < 0)
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
            if (jump)
            {
                agent.enabled = true;
                jump = false;
                jumpCount = 0;
            }
        }
    }

    private void UpdateSearch()
    {
        if(searchCount)
        {            Vector3 dir = player.transform.position - transform.position;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 5);
            _offset = new Vector3(0f, transform.position.y + 1, 0f);
            Vector3 move = Vector3.forward;

            float x = Random.Range((float)startPoint.x - maxFindRange, (float)startPoint.x + maxFindRange);
            float z = Random.Range((float)startPoint.z - maxFindRange, (float)startPoint.z + maxFindRange);
            move = new Vector3(x, 0f, z);

            Debug.DrawRay(move + _offset, Vector3.down, Color.yellow, 100);
             if (Physics.Raycast(move + _offset, Vector3.down, out RaycastHit hit, 2f))
             {
                 if (hit.transform.CompareTag("Floor"))
                {
                   
                    enemyAnimator.SetBool("Move", true);
                     _rayPos = hit.point;
                     agent.SetDestination(_rayPos);
                    searchCount = false;
                    Vector3 dir2 = transform.position - hit.point;
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir2), Time.deltaTime * 5);
                }
             }

               
        }
        if(!searchCount)
        {
            if (Vector3.SqrMagnitude(transform.position - _rayPos) < 0.5f)
            {
                enemyAnimator.SetBool("Move", false);

                reconTimer = 0;
                enemyState = State.Idle;
                searchCount = true;
            }
        }

    }

    //기본 상태
    private void UpdateIdle()
    {
        if (Vector3.SqrMagnitude(transform.position - player.position) < Mathf.Pow(maxFindRange, 2))
        {
            enemyState = State.Move;
        }

        if (Vector3.SqrMagnitude(transform.position - player.position) < Mathf.Pow(maxAttackRange, 2))
        {
            timer = 0.0f;
       
            enemyAnimator.SetBool("Move", false);
            enemyState = State.Attack;
        }
        else if (Vector3.SqrMagnitude(transform.position - player.position) > Mathf.Pow(maxAttackRange, 2))
        {
            reconTimer += Time.deltaTime;

            if (reconTimer > reconTime)
            {
                enemyState = State.Search;
            }
        }
        if (Vector3.SqrMagnitude(transform.position - startPoint) > Mathf.Pow(maxMoveRange, 2))
        {
            enemyState = State.Return;

        }
    }
    //무브 상태
    private void UpdateMove()
    {
        if (Vector3.SqrMagnitude(transform.position - startPoint) > Mathf.Pow(maxMoveRange, 2))
        {
            enemyState = State.Return;

        }

        else if (Vector3.SqrMagnitude(transform.position - player.position) < Mathf.Pow(maxFindRange, 2))
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
        }

        if (Vector3.SqrMagnitude(transform.position - player.position) < Mathf.Pow(maxAttackRange, 2))
        {
            enemyAnimator.SetBool("Move", false);
            enemyState = State.Idle;
        }
        if (!_isChasing)
        {
            if (Vector3.SqrMagnitude(transform.position - agent.destination) < 0.01f)
            {
                enemyAnimator.SetBool("Move", false);
                enemyState = State.Idle;
            }
        }

    }
    //공격 상태
    private void UpdateAttack()
    {
        if (Vector3.SqrMagnitude(transform.position - player.position) < Mathf.Pow(maxAttackRange, 2))
        {
            Vector3 dir = player.transform.position - transform.position;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 5);

            timer += Time.deltaTime;
            motionTime += Time.deltaTime;

            if (motionTime > attTime - 0.1f)
            {
                enemyAnimator.SetTrigger("Attack 0");
                motionTime = 0.0f;
            }
            if (_canApplyDamage && enemyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= dmgApplyTime)
            {
                ApplyDamage();
            }

            if (timer > attTime)
            {
                timer = 0.0f;
                _canApplyDamage = true;
            }
        }

        else
        {
            enemyState = State.Idle;
            timer = 0.0f;
        }
    }

    void ApplyDamage()
    {
        _canApplyDamage = false;
        player.GetComponent<Status>().Damage(GetComponent<Status>().GetAtk(), transform.position);
        enemyState = State.Idle;
    }

    //사망 상태
    private void UpdateDie()
    {
        if (!_isDie)
        {
            _isDie = true;
            enemyAnimator.SetTrigger("Die 0");
        }
        myRigid.isKinematic = true;
        myColider.isTrigger = true;
        agent.enabled = false;

        dieTime += Time.deltaTime;
        if (dieTime >= 60)
        {
            string name = GetComponent<EnemyStatus>().GetName();
            ObjectPooling.instance.PushObjectToPool(name, this.gameObject);
        }
    }


    //복귀 상태
    private void UpdateReturn()
    {
        if (Vector3.SqrMagnitude(transform.position - startPoint) > 0.1f)
        {
            agent.SetDestination(startPoint);
        }
        else
        {
            status.SetCurrentHp(status.GetMaxHp());
            //enemyAnimator.SetInteger("animation", 1);
            enemyAnimator.SetBool("Move", false);
            agent.ResetPath();
            _isChasing = false;
            transform.position = startPoint;
            enemyState = State.Idle;
        }
    }

    private void OnDrawGizmos()
    {

        //근접 공격 가능 범위
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxAttackRange);
        //인식 범위
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, maxFindRange);
        //최대 추적 범위
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(startPoint, maxMoveRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(startPoint, reconRange);
    }

    public PlayerStatus GetPlayerStatus() { return player.GetComponent<PlayerStatus>(); }
}
