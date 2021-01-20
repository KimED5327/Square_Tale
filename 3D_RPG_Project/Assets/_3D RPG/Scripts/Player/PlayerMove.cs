using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{ 
    public float speed;
    public float applySpeed;
    public float jumpForce;

    public GameObject attackEffect1;
    public GameObject attackEffect2;
    public GameObject attackEffect3; // 검사 기본공격 이펙트
    public GameObject skill1Effect;
    public GameObject skill2Effect;
    public GameObject skill3Effect;
    public GameObject skill4Effect; // 스킬 이펙트
    public GameObject effect1;
    public GameObject effect2;
    public GameObject effect3;
    public GameObject effect4; // 검사의 실제 데미지 콜라이더

    public GameObject sword;
    public GameObject mage; // 캐릭터

    public Transform magicAttack1Pos;
    public Transform magicAttack2Pos;
    public Transform magicAttack3Pos; // 마법사 기본공격 위치
    public Transform blackholePos; // 블랙홀의 시작위치
    public GameObject iceMissile;
    public GameObject bubble;
    public GameObject blood; // 마법사 기본공격 이펙트
    public GameObject startLightning; // 마법사 선더볼트 지팡이 이펙트
    public GameObject lightning; // 선더볼트 이펙트
    public GameObject startRain; // 아이스 레인의 캐스팅 이펙트
    public GameObject rain; // 아이스레인 이펙트
    public GameObject blackhole; // 블랙홀 이펙트
    public GameObject meteo; // 익스플로전 이펙트

    public GameObject swordButton;
    public GameObject mageButton;
    public GameObject swordSkillUI;
    public GameObject mageSkillUI;
    public GameObject swordUseSkill;
    public GameObject mageUseSkill;

    public GameObject swordAttack1Img;
    public GameObject swordAttack2Img;
    public GameObject swordAttack3Img;

    public GameObject mageAttack1Img;
    public GameObject mageAttack2Img;
    public GameObject mageAttack3Img;

    public GameObject resurrectionUI;
    public GameObject swapBtn;

    public GameObject resurrectionEffect; // 부활 이펙트

    public GameObject[] _skillCoolImg = null; // 스킬 쿨타임 이미지 받는곳

    float hAxis;
    float vAxis;
    float attackDelay;
    float comboDelay;
    float comboCountDelay;
    // 스킬 쿨타임
    float curSkill1Cooltime;
    float curSkill2Cooltime;
    float curSkill3Cooltime;
    float curSkill4Cooltime;
    // 스킬 쿨타임 수정 필요 !!
    float swordSkill1Cooltime = 1f; //15
    float swordSkill2Cooltime = 2f; //20
    float swordSkill3Cooltime = 1f; //15
    float swordSkill4Cooltime = 3f; //30

    float mageSkill1Cooltime = 2f; //20
    float mageSkill2Cooltime = 3f; //30
    float mageSkill3Cooltime = 3f; //30
    float mageSkill4Cooltime = 4f; //45

    float dodgeCooltime = 1.0f;

    int comboCount = 0;

    bool isVictory = false;

    bool isJump;
    bool isDodge;
    bool isDodgeCooltime;
    bool isAttackReady = true;
    bool isCombo;
    bool isComboCount;
    bool isAttack2;
    bool isAttack3;
    bool isSkill1;
    bool isSkill2;
    bool isSkill3;
    bool isSkill4;
    bool isMove;
    bool isDie;
    bool isDieTrigger;
    bool isSword;
    bool isMage;
    bool isCasting;
    bool isBorder; // 벽 감지하는 변수

    public static bool s_canMove = true;

    Vector3 moveVec;
    Vector3 dodgeVec;
    Vector3 realMoveVec;

    [SerializeField] BoxCollider _colFace = null;


    Rigidbody myRigid;
    Weapon equipWeapon; // 검사 평타
    MageAttack mageAttack; // 법사 평타
    PlayerStatus myStatus;
    SkillManager skillButton;
    MageSkillManager mageSkillButton;
    JoyStick joystick;
    BlockController blockCon;
    SkillCooltime skillCool;

    public Animator anim;
    public Animator[] anims;

    // Start is called before the first frame update
    void Start()
    {
        Resurrection();
        myRigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        equipWeapon = GetComponentInChildren<Weapon>();
        myStatus = GetComponent<PlayerStatus>();
        blockCon = GetComponent<BlockController>();
        skillButton = FindObjectOfType<SkillManager>();
        mageSkillButton = FindObjectOfType<MageSkillManager>();
        joystick = FindObjectOfType<JoyStick>();
        SaveManager.instance.Load();
        PlayerType();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Die();
        OutofMap();
        if (!isDie)
        {
            attackDelay += Time.deltaTime;
            isAttackReady = equipWeapon.GetWeaponRate() < attackDelay;

            if (!isVictory)
            {
                if (s_canMove)
                {
                    Move();
                    MMove();
                    Jump();
                    Dodge();
                    StopWall();
                }
            }

            Cooldown();
            LevelUp();

            if (isCombo)
            {
                comboDelay += Time.deltaTime;
                if (comboDelay > 1)
                {
                    comboCount = 0;
                    comboDelay = 0;
                    isCombo = false;
                    isAttack2 = false;
                    isAttack3 = false;
                }
            }

            if (isComboCount)
            {
                comboCountDelay++;
                if (comboCountDelay > 25)
                {
                    comboCountDelay = 0;
                    comboCount++;
                    isComboCount = false;
                    if (comboCount == 3)
                    {
                        comboCount = 0;
                        isAttack2 = false;
                        isAttack3 = false;
                    }
                }
            }

            if (sword.activeSelf)
            {
                isSword = true;
                isMage = false;
                swordButton.SetActive(true);
                swordSkillUI.SetActive(true);
                swordUseSkill.SetActive(true);
                mageButton.SetActive(false);
                mageSkillUI.SetActive(false);
                mageUseSkill.SetActive(false);
            }
            else
            {
                isSword = false;
                isMage = true;
                swordButton.SetActive(false);
                swordSkillUI.SetActive(false);
                swordUseSkill.SetActive(false);
                mageButton.SetActive(true);
                mageSkillUI.SetActive(true);
                mageUseSkill.SetActive(true);
            }
        }
    }

    void PlayerType() //로드된 플레이어 타입
    {
        if (isSword)
        {
            anim = anims[0];
            isMage = false;
            sword.SetActive(true);
            mage.SetActive(false);
        }
        if (isMage)
        {
            anim = anims[1];
            isSword = false;
            sword.SetActive(false);
            mage.SetActive(true);
        }
    }

    void Move()
    {
        if (!Camera.main.gameObject.activeSelf) return;

        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        if (isSword)
        {
            realMoveVec = (moveVec.z * Camera.main.transform.forward + Camera.main.transform.right * moveVec.x);
        }
        if (isMage)
        {
            realMoveVec = (moveVec.z * Camera.main.transform.forward + Camera.main.transform.right * moveVec.x) * 1.2f;
        }

        realMoveVec.y = 0;

        if (isDodge)
        {
            realMoveVec = dodgeVec;
        }

        if (!isAttackReady || isMove)
        {
            realMoveVec = Vector3.zero;
            if (isDodge)
            {
                realMoveVec = (moveVec.z * Camera.main.transform.forward + Camera.main.transform.right * moveVec.x);
                isAttackReady = true;
            }
        }

        if (!isBorder) transform.position += realMoveVec * speed * Time.deltaTime;

        transform.LookAt(transform.position + realMoveVec);

        anim.SetBool("isRun", realMoveVec != Vector3.zero);
    }

    void MMove()
    {
        if (joystick.isTouch && joystick.moveVec != Vector3.zero)
        {
            moveVec = joystick.moveVec;

            if (isSword)
            {
                realMoveVec = (moveVec.z * Camera.main.transform.forward + Camera.main.transform.right * moveVec.x);
            }
            if (isMage)
            {
                realMoveVec = (moveVec.z * Camera.main.transform.forward + Camera.main.transform.right * moveVec.x) * 1.2f;
            }

            realMoveVec.y = 0;

            if (isDodge)
            {
                realMoveVec = dodgeVec;
            }

            if (!isAttackReady || isMove)
            {
                realMoveVec = Vector3.zero;
                if (isDodge)
                {
                    realMoveVec = (moveVec.z * Camera.main.transform.forward + Camera.main.transform.right * moveVec.x);
                    isAttackReady = true;
                }
            }

            if (!isBorder) transform.position += realMoveVec * speed * Time.deltaTime;

            transform.LookAt(transform.position + realMoveVec);

            anim.SetBool("isRun", realMoveVec != Vector3.zero);
        }
    }

    void StopWall()
    {
        isBorder = Physics.Raycast(transform.position, transform.forward, 0.5f, LayerMask.GetMask("Floor"));
    }

    void Jump()
    {
        if (Input.GetKeyDown("space") && !isJump && !isDodge)
        {
            SoundManager.instance.PlayEffectSound("Jump");
            SaveManager.instance.Save();
            isJump = true;
            myRigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
        }
    }

    public void MJump()
    {
        if (!isJump && !isDodge)
        {
            SaveManager.instance.Save();
            SoundManager.instance.PlayEffectSound("Jump");
            isJump = true;
            myRigid.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
        }
    }

    void Dodge()
    {
        if (Input.GetKeyDown("left shift") && !isJump && moveVec != Vector3.zero && !isDodge)
        {
            if (!isDodgeCooltime)
            {
                isDodgeCooltime = true;
                SoundManager.instance.PlayEffectSound("Shout2");
                dodgeVec = realMoveVec;
                speed *= 1.5f;
                anim.SetTrigger("doDodge");
                isDodge = true;
                Invoke("DodgeOut", 0.3f);
                Invoke("DodgeCooltime", dodgeCooltime);
                DodgeCooltimeImg();
            }
            else
            {
                Notification.instance.ShowFloatingMessage(StringManager.msgCanNotSkill);
            }
        }
    }

    public void MDodge()
    {
        if (!isJump && moveVec != Vector3.zero && !isDodge)
        {
            if (!isDodgeCooltime)
            {
                isDodgeCooltime = true;
                SoundManager.instance.PlayEffectSound("Shout2");
                dodgeVec = realMoveVec;
                speed *= 1.5f;
                anim.SetTrigger("doDodge");
                isDodge = true;
                Invoke("DodgeOut", 0.3f);
                Invoke("DodgeCooltime", dodgeCooltime);
                DodgeCooltimeImg();
            }
            else
            {
                Notification.instance.ShowFloatingMessage(StringManager.msgCanNotSkill);
            }
        }
    }

    void DodgeOut()
    {
        speed = 3;
        isDodge = false;
    }

    void DodgeCooltime()
    {
        isDodgeCooltime = false;
    }

    void DodgeCooltimeImg()
    {
        if (isSword)
        {
            skillCool = _skillCoolImg[4].GetComponent<SkillCooltime>();
            skillCool.SetSkillCooltime(dodgeCooltime, true);
        }
        if (isMage)
        {
            skillCool = _skillCoolImg[5].GetComponent<SkillCooltime>();
            skillCool.SetSkillCooltime(dodgeCooltime, true);
        }
    }

    void MageFire(string name, Vector3 dir, int combo)
    {
        MageAttack mageAttack = ObjectPooling.instance.GetObjectFromPool(name, dir).GetComponent<MageAttack>();
        mageAttack.SetDir(transform.forward);
        mageAttack.setCombo(combo);
    }

    public void Attack()
    {
        if (isVictory) return;

        if (isSword)
        {

            if (isAttackReady && !isDodge && !isJump && !isCombo && !isDie)
            {
                SoundManager.instance.PlayEffectSound("Shout1");
                SoundManager.instance.PlayEffectSound("Slash_S");
                equipWeapon.Use(1);
                isCombo = true;
                isComboCount = true;
                anim.SetTrigger("doAttack1");
                StopCoroutine("Effect1");
                StartCoroutine("Effect1");
                attackDelay = 0;
            }

            if (!isAttackReady && isCombo)
            {
                if (comboCount == 1 && !isAttack2)
                {
                    SoundManager.instance.PlayEffectSound("Shout2");
                    SoundManager.instance.PlayEffectSound("Slash_M");
                    equipWeapon.Use(2);
                    isAttack2 = true;
                    isComboCount = true;
                    comboDelay = 0;
                    anim.SetTrigger("doAttack2");
                    StopCoroutine("Effect2");
                    StartCoroutine("Effect2");
                    attackDelay = 0;
                }
                else if (comboCount == 2 && !isAttack3)
                {
                    SoundManager.instance.PlayEffectSound("Slash_L");
                    equipWeapon.Use(3);
                    isAttack3 = true;
                    isComboCount = true;
                    StopCoroutine("Effect3");
                    StartCoroutine("Effect3");
                    comboDelay = 0;
                    anim.SetTrigger("doAttack3");
                    attackDelay = 0;
                }
            }
        }
        if (isMage)
        {
            if (isAttackReady && !isDodge && !isJump && !isCombo && !isDie)
            {
                isCombo = true;
                isComboCount = true;
                SoundManager.instance.PlayEffectSound("Shout1");
                SoundManager.instance.PlayEffectSound("MageAttack1");
                anim.SetTrigger("doAttack1");
                StopCoroutine("Effect1");
                StartCoroutine("Effect1");
                attackDelay = 0;
            }

            if (!isAttackReady && isCombo)
            {
                if (comboCount == 1 && !isAttack2)
                {
                    isAttack2 = true;
                    isComboCount = true;
                    comboDelay = 0;
                    SoundManager.instance.PlayEffectSound("Shout2");
                    SoundManager.instance.PlayEffectSound("MageAttack2");
                    anim.SetTrigger("doAttack2");
                    StopCoroutine("Effect2");
                    StartCoroutine("Effect2");
                    attackDelay = 0;
                }
                else if (comboCount == 2 && !isAttack3)
                {
                    isAttack3 = true;
                    isComboCount = true;
                    comboDelay = 0;
                    anim.SetTrigger("doAttack3");
                    StopCoroutine("Effect3");
                    StartCoroutine("Effect3");
                    attackDelay = 0;
                }
            }
        }
    }

    IEnumerator Effect1()
    {
        yield return new WaitForSeconds(0.1f);
        if (isSword)
        {
            myRigid.velocity = transform.forward * 3f;
            attackEffect1.SetActive(true);
            swordAttack1Img.SetActive(false);
            swordAttack2Img.SetActive(true);
        }
        if (isMage)
        {
            mageAttack1Img.SetActive(false);
            mageAttack2Img.SetActive(true);
            MageFire("아이스 미사일", magicAttack3Pos.position, 1);
        }
        yield return new WaitForSeconds(0.3f);
        attackEffect1.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        swordAttack1Img.SetActive(true);
        swordAttack2Img.SetActive(false);
        mageAttack1Img.SetActive(true);
        mageAttack2Img.SetActive(false);
    }

    IEnumerator Effect2()
    {
        yield return new WaitForSeconds(0.2f);
        if (isSword)
        {
            myRigid.velocity = transform.forward * 3f;
            swordAttack3Img.SetActive(true);
            attackEffect2.SetActive(true);
        }
        if (isMage)
        {
            mageAttack3Img.SetActive(true);
            MageFire("버블", magicAttack1Pos.position, 2);
            MageFire("버블", magicAttack2Pos.position, 2);
        }
        yield return new WaitForSeconds(0.5f);
        attackEffect2.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        swordAttack3Img.SetActive(false);
        mageAttack3Img.SetActive(false);
    }

    IEnumerator Effect3()
    {
        yield return new WaitForSeconds(0.3f);
        SoundManager.instance.PlayEffectSound("Shout3");

        yield return new WaitForSeconds(0.3f);
        if (isSword)
        {
            SoundManager.instance.PlayEffectSound("Slash_L");
            myRigid.velocity = transform.forward * 3f;
            swordAttack3Img.SetActive(false);
            swordAttack1Img.SetActive(true);
            attackEffect3.SetActive(true);
        }
        if (isMage)
        {
            SoundManager.instance.PlayEffectSound("MageAttack3");
            mageAttack3Img.SetActive(false);
            mageAttack1Img.SetActive(true);
            MageFire("블러드", magicAttack1Pos.position, 3);
            MageFire("블러드", magicAttack2Pos.position, 3);
            MageFire("블러드", magicAttack3Pos.position, 3);
        }
        yield return new WaitForSeconds(0.5f);
        attackEffect3.SetActive(false);
    }

    public void Skill1()
    {
        if (isVictory) return;

        if (isSkill1)
        {
            Notification.instance.ShowFloatingMessage(StringManager.msgCanNotSkill);
        }
        if (!isSkill1 && !isDie && !isCasting)
        {
            isMove = true;
            isSkill1 = true;
            anim.SetTrigger("doSkill1");
            StopCoroutine("Skill1Effect");
            StartCoroutine("Skill1Effect");
            startLightning.SetActive(true);
        }
    }

    IEnumerator Skill1Effect()
    {
        yield return new WaitForSeconds(0.3f);
        if (isSword)
        {
            GameObject instantEffect = Instantiate(effect1, transform.position, transform.rotation);
            Rigidbody rigid = instantEffect.GetComponent<Rigidbody>();
            Destroy(instantEffect, 0.5f);
            SoundManager.instance.PlayEffectSound("Skill1");
            OverlapSkillArea area = FindObjectOfType<OverlapSkillArea>();
            area.SetSkillNum(1);
            skill1Effect.SetActive(true);
        }
        if (isMage)
        {
            List<Transform> enemyPos = new List<Transform>();
            Collider[] checkColliders = Physics.OverlapSphere(transform.position, 5, LayerMask.GetMask("Enemy"));
            int count = 0;
            for (int i = 0; i < checkColliders.Length; i++)
            {
                if (count >= 5) break;
                count++;
                enemyPos.Add(checkColliders[i].transform);
            }

            for (int i = 0; i < enemyPos.Count; i++)
            {
                SkillArea area = ObjectPooling.instance.GetObjectFromPool("라이트닝", enemyPos[i].position).GetComponent<SkillArea>();
                area.SetSkillNum(1);
            }
        }
        yield return new WaitForSeconds(0.5f);
        skill1Effect.SetActive(false);
        startLightning.SetActive(false);
        isMove = false;
    }

    public void Skill2()
    {
        if (isVictory) return;

        if (isSkill2)
        {
            Notification.instance.ShowFloatingMessage(StringManager.msgCanNotSkill);
        }
        if (!isSkill2 && !isDie && !isCasting)
        {
            if (isSword)
            {
                anim.SetTrigger("doSkill1");
            }
            if (isMage)
            {
                isJump = true;
                isCasting = true;
            }
            isMove = true;
            isSkill2 = true;
            StopCoroutine("Skill2Effect");
            StartCoroutine("Skill2Effect");
        }
    }

    IEnumerator Skill2Effect()
    {
        if (isSword)
        {
            yield return new WaitForSeconds(0.3f);
            for (int i = 0; i < 5; i++)
            {

                SoundManager.instance.PlayEffectSound("Skill2");
                GameObject instantEffect = Instantiate(effect2, transform.position, transform.rotation);

                Rigidbody rigid = instantEffect.GetComponent<Rigidbody>();
                Vector3 dirVec = new Vector3(Mathf.Cos(Mathf.PI * 2 * i / 5), 0, Mathf.Sin(Mathf.PI * 2 * i / 5));
                rigid.AddForce(dirVec.normalized * 4, ForceMode.Impulse);

                Destroy(instantEffect, 0.5f);
            }
            OverlapSkillArea area = FindObjectOfType<OverlapSkillArea>();
            area.SetSkillNum(2);
            skill2Effect.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            skill2Effect.SetActive(false);
            isMove = false;
        }
        if (isMage)
        {
            startRain.SetActive(true);
            yield return new WaitForSeconds(2.0f);
            isJump = false;
            isCasting = false;
            anim.SetTrigger("doAttack2");
            startRain.SetActive(false);
            rain.SetActive(true);
            isMove = false;
            SkillArea area = FindObjectOfType<SkillArea>();
            area.SetSkillNum(2);
            SoundManager.instance.PlayEffectSound("IceRain");
            yield return new WaitForSeconds(3.0f);
            rain.SetActive(false);
        }
    }

    public void Skill3()
    {
        if (isVictory) return;

        if (isSkill3)
        {
            Notification.instance.ShowFloatingMessage(StringManager.msgCanNotSkill);
        }
        if (!isSkill3 && !isDie && !isCasting)
        {
            isMove = true;
            isSkill3 = true;
            anim.SetTrigger("doSkill1");
            StopCoroutine("Skill3Effect");
            StartCoroutine("Skill3Effect");
        }
    }

    IEnumerator Skill3Effect()
    {
        if (isSword)
        {
            yield return new WaitForSeconds(0.3f);
            GameObject instantEffect = Instantiate(effect3, transform.position, transform.rotation);
            Rigidbody rigid = instantEffect.GetComponent<Rigidbody>();
            rigid.velocity = transform.forward * 10;
            OverlapSkillArea area = FindObjectOfType<OverlapSkillArea>();
            area.SetSkillNum(3);
            skill3Effect.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            Destroy(instantEffect);
            skill3Effect.SetActive(false);
            isMove = false;
        }
        if (isMage)
        {
            float elapseTime = 0f;
            float finishTime = 4f;
            SkillArea area = ObjectPooling.instance.GetObjectFromPool("블랙홀", blackholePos.position).GetComponent<SkillArea>();
            area.SetSkillNum(3);
            List<Transform> enemyPos = new List<Transform>();
            Collider[] checkColliders = Physics.OverlapSphere(blackholePos.position, 5, LayerMask.GetMask("Enemy"));
            Vector3 blackholePoint = blackholePos.position + Vector3.up * 1.3f;
            for (int i = 0; i < checkColliders.Length; i++)
            {
                enemyPos.Add(checkColliders[i].transform);
            }
            SoundManager.instance.PlayEffectSound("BlackHole");
            isMove = false;

            while (elapseTime <= finishTime)
            {
                yield return null;
                for (int i = 0; i < enemyPos.Count; i++)
                {
                    if (checkColliders[i].transform.name.Equals("릴리(Clone)")) continue;
                    enemyPos[i].position = Vector3.MoveTowards(enemyPos[i].position, blackholePoint, 5f * Time.deltaTime);
                }
                elapseTime += Time.deltaTime;
            }
            enemyPos.Clear();
        }
    }

    public void Skill4()
    {
        if (isVictory) return;

        if (isSkill4)
        {
            Notification.instance.ShowFloatingMessage(StringManager.msgCanNotSkill);
        }
        if (!isSkill4 && !isDie && !isCasting)
        {
            isMove = true;
            isSkill4 = true;
            anim.SetTrigger("doAttack2");
            StopCoroutine("Skill4Effect");
            StartCoroutine("Skill4Effect");
        }
    }

    IEnumerator Skill4Effect()
    {
        if (isSword)
        {
            yield return new WaitForSeconds(0.3f);
            for (int i = 0; i < 8; i++)
            {
                GameObject instantEffect = Instantiate(effect4, transform.position, transform.rotation);

                Rigidbody rigid = instantEffect.GetComponent<Rigidbody>();
                Vector3 dirVec = new Vector3(Mathf.Cos(Mathf.PI * 2 * i / 8), 0, Mathf.Sin(Mathf.PI * 2 * i / 8));
                rigid.AddForce(dirVec.normalized * 30, ForceMode.Impulse);

                Destroy(instantEffect, 0.3f);
            }
            OverlapSkillArea area = FindObjectOfType<OverlapSkillArea>();
            area.SetSkillNum(4);
            skill4Effect.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            skill4Effect.SetActive(false);
            isMove = false;
        }
        if (isMage)
        {
            yield return new WaitForSeconds(0.5f);
            List<Vector3> meteoPos = new List<Vector3>();
            for (int i = 0; i < 6; i++)
            {
                meteoPos.Add(transform.position + new Vector3(Random.Range(-3, 3), 0f, Random.Range(-3, 3)));
            }
            isMove = false;
            for (int i = 0; i < meteoPos.Count; i++)
            {
                SkillArea area = ObjectPooling.instance.GetObjectFromPool("익스플로전", meteoPos[i]).GetComponent<SkillArea>();
                area.SetSkillNum(4);
                SoundManager.instance.PlayEffectSound("Explosion");
                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    public void useLeftSkill()
    {
        if (isVictory) return;

        if (isSword)
        {
            skillCool = _skillCoolImg[0].GetComponent<SkillCooltime>();
            if (skillButton.GetSkillButtonName(0).Equals("회전 베기"))
            {
                skillCool.SetSkillCooltime(swordSkill1Cooltime, true);
                Skill1();
            }
            else if (skillButton.GetSkillButtonName(0).Equals("회오리 베기"))
            {
                skillCool.SetSkillCooltime(swordSkill2Cooltime, true);
                Skill2();
            }
            else if (skillButton.GetSkillButtonName(0).Equals("블러드 슬래시"))
            {
                skillCool.SetSkillCooltime(swordSkill3Cooltime, true);
                Skill3();
            }
            else if (skillButton.GetSkillButtonName(0).Equals("옥타곤 슬래시"))
            {
                skillCool.SetSkillCooltime(swordSkill4Cooltime, true);
                Skill4();
            }
        }
        if (isMage)
        {
            skillCool = _skillCoolImg[2].GetComponent<SkillCooltime>();
            if (mageSkillButton.GetSkillButtonName(0).Equals("썬더 볼트"))
            {
                skillCool.SetSkillCooltime(mageSkill1Cooltime, true);
                Skill1();
            }
            else if (mageSkillButton.GetSkillButtonName(0).Equals("아이스 레인"))
            {
                skillCool.SetSkillCooltime(mageSkill2Cooltime, true);
                Skill2();
            }
            else if (mageSkillButton.GetSkillButtonName(0).Equals("타이니 블랙홀"))
            {
                skillCool.SetSkillCooltime(mageSkill3Cooltime, true);
                Skill3();
            }
            else if (mageSkillButton.GetSkillButtonName(0).Equals("익스플로전"))
            {
                skillCool.SetSkillCooltime(mageSkill4Cooltime, true);
                Skill4();
            }
        }
    }

    public void useRightSkill()
    {
        if (isVictory) return;

        if (isSword)
        {
            skillCool = _skillCoolImg[1].GetComponent<SkillCooltime>();
            if (skillButton.GetSkillButtonName(1).Equals("회전 베기"))
            {
                skillCool.SetSkillCooltime(swordSkill1Cooltime, true);
                Skill1();
            }
            else if (skillButton.GetSkillButtonName(1).Equals("회오리 베기"))
            {
                skillCool.SetSkillCooltime(swordSkill2Cooltime, true);
                Skill2();
            }
            else if (skillButton.GetSkillButtonName(1).Equals("블러드 슬래시"))
            {
                skillCool.SetSkillCooltime(swordSkill2Cooltime, true);
                Skill3();
            }
            else if (skillButton.GetSkillButtonName(1).Equals("옥타곤 슬래시"))
            {
                skillCool.SetSkillCooltime(swordSkill4Cooltime, true);
                Skill4();
            }
        }
        if (isMage)
        {
            skillCool = _skillCoolImg[3].GetComponent<SkillCooltime>();
            if (mageSkillButton.GetSkillButtonName(1).Equals("썬더 볼트"))
            {
                skillCool.SetSkillCooltime(mageSkill1Cooltime, true);
                Skill1();
            }
            else if (mageSkillButton.GetSkillButtonName(1).Equals("아이스 레인"))
            {
                skillCool.SetSkillCooltime(mageSkill2Cooltime, true);
                Skill2();
            }
            else if (mageSkillButton.GetSkillButtonName(1).Equals("타이니 블랙홀"))
            {
                skillCool.SetSkillCooltime(mageSkill3Cooltime, true);
                Skill3();
            }
            else if (mageSkillButton.GetSkillButtonName(1).Equals("익스플로전"))
            {
                skillCool.SetSkillCooltime(mageSkill4Cooltime, true);
                Skill4();
            }
        }
    }

    public void Cooldown()
    {
        if (isSkill1)
        {
            curSkill1Cooltime += Time.deltaTime;
            if (isSword)
            {
                if (curSkill1Cooltime > swordSkill1Cooltime)
                {
                    curSkill1Cooltime = 0;
                    isSkill1 = false;
                }
            }
            if (isMage)
            {
                if (curSkill1Cooltime > mageSkill1Cooltime)
                {
                    curSkill1Cooltime = 0;
                    isSkill1 = false;
                }
            }
        }
        if (isSkill2)
        {
            curSkill2Cooltime += Time.deltaTime;
            if (isSword)
            {
                if (curSkill2Cooltime > swordSkill2Cooltime)
                {
                    curSkill2Cooltime = 0;
                    isSkill2 = false;
                }
            }
            if (isMage)
            {
                if (curSkill2Cooltime > mageSkill2Cooltime)
                {
                    curSkill2Cooltime = 0;
                    isSkill2 = false;
                }
            }
        }
        if (isSkill3)
        {
            curSkill3Cooltime += Time.deltaTime;
            if (isSword)
            {
                if (curSkill3Cooltime > swordSkill3Cooltime)
                {
                    curSkill3Cooltime = 0;
                    isSkill3 = false;
                }
            }
            if (isMage)
            {
                if (curSkill3Cooltime > mageSkill3Cooltime)
                {
                    curSkill3Cooltime = 0;
                    isSkill3 = false;
                }
            }
        }
        if (isSkill4)
        {
            curSkill4Cooltime += Time.deltaTime;
            if (isSword)
            {
                if (curSkill4Cooltime > swordSkill4Cooltime)
                {
                    curSkill4Cooltime = 0;
                    isSkill4 = false;
                }
            }
            if (isMage)
            {
                if (curSkill4Cooltime > mageSkill4Cooltime)
                {
                    curSkill4Cooltime = 0;
                    isSkill4 = false;
                }
            }
        }
    }

    public void Die()
    {
        if (myStatus.GetCurrentHp() <= 0)
        {
            SaveManager.instance.Save();
            GameManager._isDie = true;
            isDie = true;
            resurrectionUI.SetActive(true);
            blockCon.enabled = false;
            if (!isDieTrigger)
            {
                GameHudMenu.instance.HideMenu();
                isDieTrigger = true;
                anim.SetTrigger("doDie");
            }
            PlayerPrefs.SetInt("curHp", myStatus.GetMaxHp());
        }
    }

    public void OutofMap() // 맵밖으로 낙사
    {

        if (transform.position.y < -50)
        {
            SaveManager.instance.Save();
            GameManager._isDie = true;
            isDie = true;
            resurrectionUI.SetActive(true);
            blockCon.enabled = false;
            if (!isDieTrigger)
            {
                GameHudMenu.instance.HideMenu();
                isDieTrigger = true;
                anim.SetTrigger("doDie");
            }
            PlayerPrefs.SetInt("curHp", myStatus.GetMaxHp());
        }
    }

    public void ResurrectionToRespawn()
    {
        // 리스폰 지점에서 부활

        GameHudMenu.instance.ShowMenu();
        //myStatus.Initialized();
        MapManager.instance.ReLoadCurrentMap();
    }

    public void ResurrectionToTown()
    {
        // 마을에서 부활
       // myStatus.Initialized();
        MapManager.instance.ChangeMap("Town");
    }

    public void Resurrection()
    {
        if (GameManager._isDie)
        {
            StartCoroutine("ResurrectionEffect");
            GameManager._isDie = false;
        }
    }

    IEnumerator ResurrectionEffect()
    {
        GameObject instantEffect = Instantiate(resurrectionEffect, transform.position, transform.rotation);
        Destroy(instantEffect, 3.0f);
        resurrectionUI.SetActive(false);
        myStatus.SetCurrentHp(myStatus.GetMaxHp());
        anim.SetTrigger("doIdle");
        yield return new WaitForSeconds(0.5f);
        blockCon.enabled = true;
        isDie = false;
        isDieTrigger = false;
    }

    public void Swap() // 버튼이 1개일떄 전환
    {
        if (!isDie && (!isSkill1 && !isSkill2  && !isSkill3  && !isSkill4))
        {
            if (isSword)
            {
                anim = anims[1];
                sword.SetActive(false);
                mage.SetActive(true);
            }
            if (isMage)
            {
                anim = anims[0];
                sword.SetActive(true);
                mage.SetActive(false);
            }
        }
        else
        {
            Notification.instance.ShowFloatingMessage(StringManager.msgCanNotSwap);
        }
    }

    public void SkillSwap() // 버튼이 1개일떄 전환
    {
        if (!isDie && (!isSkill1 && !isSkill2 && !isSkill3 && !isSkill4))
        {
            if (isSword)
            {
                anim = anims[1];
                sword.SetActive(false);
                mage.SetActive(true);
            }
            if (isMage)
            {
                anim = anims[0];
                sword.SetActive(true);
                mage.SetActive(false);
            }
        }
        else
        {
            Notification.instance.ShowFloatingMessage(StringManager.msgCanNotSwap);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag(StringManager.groundTag) ||
            collision.transform.CompareTag(StringManager.blockTag))
        {
            if (isJump)
            {
                anim.SetTrigger("forceRun");
                anim.SetBool("isJump", false);
                isJump = false;
            }
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }

    public void Victory()
    {
        isVictory = true;
        anim.SetTrigger("doWin");
        StartCoroutine(VictoryFinish());
    }


    IEnumerator VictoryFinish()
    {
        yield return new WaitForSeconds(2.5f);
        anim.SetTrigger("forceRun");
        anim.SetBool("isJump", false);
        isJump = false;
        isVictory = false;
    }

    void LevelUp()
    {
        if (myStatus.GetIsLevelUp())
        {
            ObjectPooling.instance.GetObjectFromPool("키워드 획득", transform.position);
            myStatus.SetIsLevelUp(false);
        }
    }

    public bool GetDodge() // get 대쉬값
    {
        return isDodge;
    }
    public bool GetIsSword() { return isSword; }
    public bool GetIsMage() { return isMage; }
    public void SetIsSword(bool _isSword) { isSword = _isSword; }
    public void SetIsMage(bool _isMage) { isMage = _isMage; }
    public bool GetIsSkill1() { return isSkill1; }
    public bool GetIsSkill2() { return isSkill2; }
    public bool GetIsSkill3() { return isSkill3; }
    public bool GetIsSkill4() { return isSkill4; }

    public BoxCollider GetFaceCol() { return _colFace; } 
    public bool GetIsDie() { return isDie; }
}
