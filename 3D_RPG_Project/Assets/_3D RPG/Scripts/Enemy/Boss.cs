using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boss : MonoBehaviour
{

    public enum state
    {
        idle, Attack, skill, die
    }

    public state bossState;



    public float attTime;                               //몬스터 공격속도
    public float skillTime;                             //몬스터 스킬속도
    public float dmgApplyTime;                          //실제 데미지 적용 시간.
    public float timer;                                 //공격속도 조절값
    Vector3 startPoint;                                 //최초 생성 값
    Transform player;                                   //공격 목표 (플레이어)
    PlayerMove playerMove;
    Transform AttackTransform;                          //공격 장소
    public float speed;                    

    public int maxFindRange;                            //보스 인식 범위
    public int maxAttackRange;                          //보스 공격 범위

    Animator enemyAnimator;                             //몬스터 애니메이터
    BoxCollider myColider;
    Rigidbody myRigid;
    EnemyStatus status;

    public GameObject AttackLocation;                   //공격 위치
    public GameObject AttackEffect;                     //보스 공격 이펙트
    public GameObject SkillEffect;                      //토네이도 리프 이펙트
    public GameObject SkillEffect2;                     //벚꽃 마안 이펙트
    private bool isAttack = false;                      //공격 한번
    private bool isSkillOne = false;                    //토네이도 리프 스킬!
    private bool isSkillTwo = false;                    //벚꽃 마안 스킬!
    GameObject skill;
    GameObject skill2;
    private bool sktornado;                             //스킬 토네이도 실행 불값
    private bool skCheerySome = false;                  //스킬 벚꽃 마안 실행 불값
    private bool isDamage = false;                      //대미지 받는 변수
    private bool skillAttack = false;
    private float skillAttackTime = 0;
    private int skillUpcount = 0;                       //스킬 증가 횟수
    private int skillUseTornadoCount = 0;               //토네이도 리프 사용 횟수
    private int skillUseCherryCount = 0;                //벚꽃 마안 사용 횟수
    private bool isDie = false;


    public bool getIsAttackStart() { return skillAttack; }
    public void setIsAttackStart(bool attack) { skillAttack = attack; }
    public bool getIsDamage() { return isDamage; }
    public bool getIsDie() { return isDie; }


    private void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        status = GetComponent<EnemyStatus>();
        myColider = GetComponent<BoxCollider>();
        myRigid = GetComponent<Rigidbody>();
        status.SetCurrentHp(status.GetMaxHp());
    }

    private void OnEnable()
    {
        Initialized();
        
    }

    private void Initialized()
    {
        bossState = state.idle;
        isDie = false;

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        if (playerMove == null)
            playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();

        skillAttack = false;
        isAttack = false;
        if (status == null) status = GetComponent<EnemyStatus>();
        if (status.GetCurrentHp() != status.GetMaxHp()) status.SetCurrentHp(status.GetMaxHp());
    }
    private void Update()
    {
        if(playerMove.GetIsDie()) return;
        if (skillAttack)
        {
            SkillAttackUpdate();
        }
        switch (bossState)
        {
            case state.idle:
                IdleUpdate();
                break;
            case state.Attack:
                AttackUpdate();
                break;
            case state.skill:
                SkillUpdate();
                break;
            case state.die:
                DieUpdate();
                break;
        }
        if(status.GetCurrentHp() <= 0)
        {
            bossState = state.die;
        }


    }
    private void SkillAttackUpdate()
    {
        if(skillAttack)
        {
            skillAttackTime += Time.deltaTime;
            if (skillAttackTime > 0.5)
            {
                skill.transform.localScale += new Vector3(0.2f, 0, 0.2f);
                skillUpcount++;
                skillAttackTime = 0;
                isDamage = true;
            }
            if (skillUpcount > 15)
            {
                Destroy(skill);
                skillAttack = false;
                skillAttackTime = 0;
                skillUpcount = 0;
                isDamage = false;
            }
        }

    }
    private void IdleUpdate()
    {
        if (Vector3.SqrMagnitude(transform.position - player.position) < Mathf.Pow(maxFindRange, 2))
        {
            if(maxFindRange != maxAttackRange)maxFindRange = maxAttackRange;
            timer += Time.deltaTime;

            Vector3 dir = player.transform.position - transform.position;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * speed);

            if (timer > attTime)
            {
                bossState = state.Attack;
                enemyAnimator.SetTrigger("Attack 0");
                isAttack = true;
                timer = 0;
            }
        }
    }

    private void AttackUpdate()
    {

        Vector3 dir = player.transform.position - transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * speed);

        if (isAttack)
        {
            SoundManager.instance.PlayEffectSound("BossAttack");
            Vector3 attackDir = player.position - AttackLocation.transform.position;
        
            GameObject attack = Instantiate(AttackEffect, AttackLocation.transform.position, transform.rotation);
            attack.GetComponent<ProjectileMover>().Pushinfo(player, status, this);
            Rigidbody attackRigid = attack.GetComponent<Rigidbody>();
            attack.transform.rotation = Quaternion.LookRotation(attackDir);
            isAttack = false;
        }

        if (enemyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9)
        {
            bossState = state.idle;
        }

        //1차 토네이도 리프
        if (((float)status.GetCurrentHp() / (float)status.GetMaxHp()) < 0.9f && skillUseTornadoCount == 0)
        {
            SoundManager.instance.PlayEffectSound("BossSkill");
            Debug.Log("1차 토네이도 리프");
            isSkillOne = true;
            bossState = state.skill;
            enemyAnimator.SetTrigger("Skill 0");
            skillUseTornadoCount = 1;
        }
        //2차  토네이도 리프
        else if (((float)status.GetCurrentHp() / (float)status.GetMaxHp()) < 0.6f && skillUseTornadoCount == 1)
        {
            SoundManager.instance.PlayEffectSound("BossSkill");
            Debug.Log("2차 토네이도 리프");
            isSkillOne = true;
            bossState = state.skill;
            enemyAnimator.SetTrigger("Skill 0");
            skillUseTornadoCount = 2;
        }
        //3차  토네이도 리프
        else if (((float)status.GetCurrentHp() / (float)status.GetMaxHp()) < 0.3f && skillUseTornadoCount == 2)
        {
            SoundManager.instance.PlayEffectSound("BossSkill");
            Debug.Log("3차 토네이도 리프");
            isSkillOne = true;
            bossState = state.skill;
            enemyAnimator.SetTrigger("Skill 0");
            skillUseTornadoCount = 3;
        }

        if(((float)status.GetCurrentHp() / (float)status.GetMaxHp()) < 0.5f && skillUseCherryCount == 0)
        {
            Debug.Log("1차 벚꽃 마안");
            isSkillTwo = true;
            bossState = state.skill;
            enemyAnimator.SetTrigger("Skill 0");
            skillUseCherryCount = 1;
        }
        else if (((float)status.GetCurrentHp() / (float)status.GetMaxHp()) < 0.1f && skillUseCherryCount == 1)
        {
            Debug.Log("2차 벚꽃 마안");
            isSkillTwo = true;
            bossState = state.skill;
            enemyAnimator.SetTrigger("Skill 0");
            skillUseCherryCount = 2;
        }
    }
    private void SkillUpdate()
    {
        if(isSkillOne)
        {
            if (!sktornado) // 토네이도 스킬 업데이트 시작
            {

                Vector3 dir = player.transform.position - transform.position;
                skill = Instantiate(SkillEffect, new Vector3(player.transform.position.x, player.transform.position.y + 1.2f, player.transform.position.z), player.transform.rotation);
                skill.transform.rotation = Quaternion.LookRotation(dir);
                sktornado = true;
            }
            else if (sktornado)
            {
                timer += Time.deltaTime;
                if (timer > 3)
                {
                    skillAttack = true;
                    timer = 0;
                    bossState = state.idle;
                    skill.GetComponent<ProjectileMover>().Pushinfo(player, status, this);
                    isSkillOne = false;
                    sktornado = false;
                }
                skill.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1.5f, player.transform.position.z);
            } // 토네이도 스킬 업데이트 끝

        }
        if(isSkillTwo)
        {
            skill2 = Instantiate(SkillEffect2, new Vector3(transform.position.x, transform.position.y + 2.5f, transform.position.z), transform.rotation);
            isSkillTwo = false;
            bossState = state.idle;
        }
    }
   
    private void DieUpdate()
    {
        if (!isDie)
        {
            enemyAnimator.SetTrigger("Die 0");
            isDie = true;
        }
    }

}
