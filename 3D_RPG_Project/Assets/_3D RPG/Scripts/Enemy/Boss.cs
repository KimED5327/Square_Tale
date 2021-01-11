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


    GameObject rangedWeapon;                    //몬스터 원거리 구체
    GameObject skillObject;
    public float attTime;                       //몬스터 공격속도
    public float skillTime;                     //몬스터 스킬속도
    public float dmgApplyTime;                  //실제 데미지 적용 시간.
    public float timer;                         //공격속도 조절값
    Vector3 startPoint;                         //최초 생성 값
    Transform player;                           //공격 목표 (플레이어)
    Transform AttackTransform;                  //공격 장소
    public float speed;                    

    public int maxFindRange;                    //보스 인식 범위
    public int maxAttackRange;                  //보스 공격 범위

    Animator enemyAnimator;                     //몬스터 애니메이터
    BoxCollider myColider;
    Rigidbody myRigid;
    EnemyStatus status;

    public GameObject AttackLocation;           //공격 위치
    public GameObject AttackEffect;             //몬스터 공격 이펙트
    public GameObject SkillEffect;              //몬스터 스킬 이펙트
    bool isAttack = false;                      //공격 한번
    bool isSkillOne = false;
    bool isSkillTwo = false;
    GameObject skill;
    bool skillCount;
    bool isDamage;                              //대미지 받는 변수
    bool skillAttack = false;
    float skillAttackTime = 0;
    int skillUpcount = 0;                           //스킬 크기 조절 값


    public bool getIsDamage() { return isDamage; }


    private void Start()
    {
        bossState = state.idle;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyAnimator = GetComponent<Animator>();
        status = GetComponent<EnemyStatus>();
        myColider = GetComponent<BoxCollider>();
        myRigid = GetComponent<Rigidbody>();
    }

    private void Update()
    {
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
        if(status.GetCurrentHp() < 0)
        {
            bossState = state.die;
        }


    }
    private void SkillAttackUpdate()
    {
        skillAttackTime += Time.deltaTime;
        if(skillAttackTime > 1)
        {
            skill.transform.localScale += new Vector3(0.1f, 0, 0.1f);
            skillUpcount++;
            skillAttackTime = 0;
            isDamage = true;
        }
        if(skillUpcount > 20)
        {
            Destroy(skill);
            skillAttack = false;
            isSkillOne = false;
            skillCount = false;
            skillAttackTime = 0;
            skillUpcount = 0;
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
        if ((status.GetCurrentHp() / (float)status.GetMaxHp()) < 0.8f)
        {
            isSkillOne = true;
            bossState = state.skill;
            enemyAnimator.SetBool("Skill", true);
        }
        //2차  토네이도 리프
        if ((status.GetCurrentHp() / (float)status.GetMaxHp()) < 0.6f)
        {
            isSkillOne = true;
            bossState = state.skill;
            enemyAnimator.SetBool("Skill", true);
        }
        //3차  토네이도 리프
        if ((status.GetCurrentHp() / (float)status.GetMaxHp()) < 0.4f)
        {
            isSkillOne = true;
            bossState = state.skill;
            enemyAnimator.SetBool("Skill", true);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            isSkillOne = true;
            bossState = state.skill;
            timer = 0;
        }
    }
    private void SkillUpdate()
    {

        if (!skillCount)
        {

            Vector3 dir = player.transform.position - transform.position;
            skill = Instantiate(SkillEffect, new Vector3(player.transform.position.x, 1.2f, player.transform.position.z), player.transform.rotation);
            skill.transform.rotation = Quaternion.LookRotation(dir);
            skillCount = true;
        }
        else if(skillCount)
        {
            timer += Time.deltaTime;
            if (timer > 1)
            {
                skillAttack = true;
                timer = 0;
                bossState = state.idle;
                skill.transform.position = new Vector3(player.transform.position.x, 0.1f, player.transform.position.z);
                enemyAnimator.SetBool("Skill", false);
            }
            skill.transform.position = new Vector3(player.transform.position.x, 1.2f, player.transform.position.z);
        }
    }
   
    private void DieUpdate()
    {
        enemyAnimator.SetBool("Die", true);
    }

}
