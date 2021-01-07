using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public GameObject attackEffect1;
    public GameObject attackEffect2;
    public GameObject attackEffect3;
    public GameObject skill1Effect;
    public GameObject skill2Effect;
    public GameObject skill3Effect;
    public GameObject skill4Effect;
    public GameObject effect1;
    public GameObject effect2;
    public GameObject effect3;
    public GameObject effect4;

    public GameObject sword;
    public GameObject mage;
    public Transform magicAttack1Pos;
    public Transform magicAttack2Pos;
    public Transform magicAttack3Pos;
    public Transform blackholePos;
    public GameObject iceMissile;
    public GameObject bubble;
    public GameObject blood;
    public GameObject startLightning;
    public GameObject lightning;
    public GameObject startRain;
    public GameObject rain;
    public GameObject blackhole;
    public GameObject meteo;

    public GameObject swordButton;
    public GameObject mageButton;

    public GameObject swordAttack1Img;
    public GameObject swordAttack2Img;
    public GameObject swordAttack3Img;

    public GameObject mageAttack1Img;
    public GameObject mageAttack2Img;
    public GameObject mageAttack3Img;

    float hAxis;
    float vAxis;
    float attackDelay;
    float comboDelay;
    float comboCountDelay;
    float skill1Cooldown;
    float skill2Cooldown;
    float skill3Cooldown;
    float skill4Cooldown;

    int comboCount = 0;

    bool isJump;
    bool isDodge;
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
    bool isSword;
    bool isMage;
    bool isCasting;

    Vector3 moveVec;
    Vector3 dodgeVec;
    Vector3 realMoveVec;

    Rigidbody myRigid;
    Weapon equipWeapon;
    PlayerStatus myStatus;

    public Animator anim;
    public Animator[] anims;

    // Start is called before the first frame update
    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        equipWeapon = GetComponentInChildren<Weapon>();
        myStatus = GetComponent<PlayerStatus>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isDie)
        {
            attackDelay += Time.deltaTime;
            isAttackReady = equipWeapon.GetWeaponRate() < attackDelay;

            Move();
            Jump();
            Dodge();
            Cooldown();
            Victory();

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
                mageButton.SetActive(false);
            }
            else
            {
                isSword = false;
                isMage = true;
                swordButton.SetActive(false);
                mageButton.SetActive(true);
            }
        }
        else
        {
            Die();
        }
    }

    void Move()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        realMoveVec = (moveVec.z * Camera.main.transform.forward + Camera.main.transform.right * moveVec.x);

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

        transform.position += realMoveVec * speed * Time.deltaTime;

        transform.LookAt(transform.position + realMoveVec);

        anim.SetBool("isRun", realMoveVec != Vector3.zero);
    }

    void Jump()
    {
        if (Input.GetKeyDown("space") && !isJump && !isDodge)
        {
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
            dodgeVec = realMoveVec;
            speed *= 2;
            anim.SetTrigger("doDodge");
            isDodge = true;

            Invoke("DodgeOut", 0.5f);
        }
    }

    public void MDodge()
    {
        if (!isJump && moveVec != Vector3.zero && !isDodge)
        {
            dodgeVec = realMoveVec;
            speed *= 2;
            anim.SetTrigger("doDodge");
            isDodge = true;

            Invoke("DodgeOut", 0.5f);
        }
    }

    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }

    public void Attack()
    {
        if (isSword)
        {
            equipWeapon.Use();

            if (isAttackReady && !isDodge && !isJump && !isCombo && !isDie)
            {
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
            GameObject instantMagic = Instantiate(iceMissile, magicAttack1Pos.position, magicAttack1Pos.rotation);
            Rigidbody rigid = instantMagic.GetComponent<Rigidbody>();
            Destroy(instantMagic, 1f);
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
            GameObject instantMagic = Instantiate(bubble, magicAttack2Pos.position, magicAttack2Pos.rotation);
            Rigidbody rigid = instantMagic.GetComponent<Rigidbody>();
            Destroy(instantMagic, 1f);
        }
        yield return new WaitForSeconds(0.5f);
        attackEffect2.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        swordAttack3Img.SetActive(false);
        mageAttack3Img.SetActive(false);
    }

    IEnumerator Effect3()
    {
        yield return new WaitForSeconds(0.6f);
        if (isSword)
        {
            myRigid.velocity = transform.forward * 3f;
            swordAttack3Img.SetActive(false);
            swordAttack1Img.SetActive(true);
            attackEffect3.SetActive(true);
        }
        if (isMage)
        {
            mageAttack3Img.SetActive(false);
            mageAttack1Img.SetActive(true);
            GameObject instantMagic = Instantiate(blood, magicAttack3Pos.position, magicAttack3Pos.rotation);
            Rigidbody rigid = instantMagic.GetComponent<Rigidbody>();
            Destroy(instantMagic, 1f);
        }
        yield return new WaitForSeconds(0.5f);
        attackEffect3.SetActive(false);
    }

    public void Skill1()
    {
        if (!isSkill1 && !isDie && !isCasting)
        {
            isMove = true;
            isSkill1 = true;
            anim.SetTrigger("doSkill1");
            StopCoroutine("Skill1Effect");
            StartCoroutine("Skill1Effect");
        }
    }

    IEnumerator Skill1Effect()
    {
        startLightning.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        if (isSword)
        {
            GameObject instantEffect = Instantiate(effect1, transform.position, transform.rotation);
            Rigidbody rigid = instantEffect.GetComponent<Rigidbody>();
            Destroy(instantEffect, 0.5f);
            skill1Effect.SetActive(true);
        }
        if (isMage)
        {
            List<Transform> enemyPos = new List<Transform>();
            Collider[] checkColliders = Physics.OverlapSphere(transform.position, 5, LayerMask.GetMask("Enemy"));
            int count = 0;
            Debug.Log("발동");
            for (int i = 0; i < checkColliders.Length; i++)
            {
                if (count >= 5) break;
                count++;
                enemyPos.Add(checkColliders[i].transform);
                Debug.Log(count);
            }

            for (int i = 0; i < enemyPos.Count; i++)
            {
                GameObject instantMagic = Instantiate(lightning, enemyPos[i].position, enemyPos[i].rotation);
                Destroy(instantMagic, 0.5f);
            }
        }
        yield return new WaitForSeconds(0.5f);
        skill1Effect.SetActive(false);
        startLightning.SetActive(false);
        isMove = false;
    }

    public void Skill2()
    {
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
                GameObject instantEffect = Instantiate(effect2, transform.position, transform.rotation);

                Rigidbody rigid = instantEffect.GetComponent<Rigidbody>();
                Vector3 dirVec = new Vector3(Mathf.Cos(Mathf.PI * 2 * i / 5), 0, Mathf.Sin(Mathf.PI * 2 * i / 5));
                rigid.AddForce(dirVec.normalized * 4, ForceMode.Impulse);

                Destroy(instantEffect, 0.5f);
            }
            skill2Effect.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            skill2Effect.SetActive(false);
            isMove = false;
        }
        if (isMage)
        {
            startRain.SetActive(true);
            yield return new WaitForSeconds(3.0f);
            isJump = false;
            isCasting = false;
            anim.SetTrigger("doAttack2");
            startRain.SetActive(false);
            rain.SetActive(true);
            isMove = false;
            yield return new WaitForSeconds(3.0f);
            rain.SetActive(false);
        }
    }

    public void Skill3()
    {
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
            GameObject instantMagic = Instantiate(blackhole, blackholePos.position, blackholePos.rotation);
            List<Transform> enemyPos = new List<Transform>();
            Collider[] checkColliders = Physics.OverlapSphere(blackholePos.position, 5, LayerMask.GetMask("Enemy"));
            Vector3 blackholePoint = blackholePos.position;
            for (int i = 0; i < checkColliders.Length; i++)
            {
                enemyPos.Add(checkColliders[i].transform);
            }

            isMove = false;
            Destroy(instantMagic, 4.0f);

            while (elapseTime <= finishTime)
            {
                yield return null;
                for (int i = 0; i < enemyPos.Count; i++)
                {
                    enemyPos[i].position = Vector3.MoveTowards(enemyPos[i].position, blackholePoint, 10f * Time.deltaTime);
                }
                elapseTime += Time.deltaTime;
            }
            enemyPos.Clear();
        }
    }

    public void Skill4()
    {
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
                meteoPos.Add(transform.position + new Vector3(Random.Range(-2, 2), 0f, Random.Range(-2, 2)));
            }
            isMove = false;
            for (int i = 0; i < meteoPos.Count; i++)
            {
                GameObject instantMagic = Instantiate(meteo, meteoPos[i], Quaternion.identity);
                Destroy(instantMagic, 1.5f);
                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    public void Cooldown()
    {
        if (isSkill1)
        {
            skill1Cooldown += 1 * Time.deltaTime;
            if (skill1Cooldown > 5)
            {
                skill1Cooldown = 0;
            }
                isSkill1 = false;
        }
        if (isSkill2)
        {
            skill2Cooldown += 1 * Time.deltaTime;
            if (skill2Cooldown > 5)
            {
                skill2Cooldown = 0;
            }
                isSkill2 = false;
        }
        if (isSkill3)
        {
            skill3Cooldown += 1 * Time.deltaTime;
            if (skill3Cooldown > 5)
            {
                skill3Cooldown = 0;
            }
                isSkill3 = false;
        }
        if (isSkill4)
        {
            skill4Cooldown += 1 * Time.deltaTime;
            if (skill4Cooldown > 5)
            {
                skill4Cooldown = 0;
            }
                isSkill4 = false;
        }
    }

    public void Die()
    {
        if (myStatus.GetCurrentHp() <= 0)
        {
            anim.SetTrigger("doDie");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Floor"))
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

    void Victory()
    {
        if (Input.GetKeyDown("i")) 
        {
            anim.SetTrigger("doWin");
        }
    }
}
