using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed;
    public float applySpeed;
    public float jumpForce;
    public GameObject effect1;
    public GameObject effect2;
    public GameObject effect3;
    public GameObject skill1Effect;
    public GameObject skill2Effect;
    public GameObject skill3Effect;
    public GameObject skill4Effect;
    public GameObject effect4;

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

    Vector3 moveVec;
    Vector3 dodgeVec;
    Vector3 realMoveVec;

    Rigidbody myRigid;
    Animator anim;
    Weapon equipWeapon;

    // Start is called before the first frame update
    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        equipWeapon = GetComponentInChildren<Weapon>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        attackDelay += Time.deltaTime;
        isAttackReady = equipWeapon.GetWeaponRate() < attackDelay;

        Move();
        Jump();
        Dodge();
        Cooldown();

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
            if (comboCountDelay > 20)
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

        transform.position += realMoveVec * (speed * (1 + applySpeed)) * Time.deltaTime;

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

    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }

    public void Attack()
    {
        equipWeapon.Use();

        if (isAttackReady && !isDodge && !isJump && !isCombo)
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

    IEnumerator Effect1()
    {
        yield return new WaitForSeconds(0.1f);
        effect1.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        effect1.SetActive(false);
    }

    IEnumerator Effect2()
    {
        yield return new WaitForSeconds(0.2f);
        effect2.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        effect2.SetActive(false);
    }

    IEnumerator Effect3()
    {
        yield return new WaitForSeconds(0.6f);
        effect3.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        effect3.SetActive(false);
    }

    public void Skill1()
    {
        if (!isSkill1)
        {
            isMove = true;
            isSkill1 = true;
            equipWeapon.Use();
            anim.SetTrigger("doSkill1");
            StopCoroutine("Skill1Effect");
            StartCoroutine("Skill1Effect");
        }
    }

    IEnumerator Skill1Effect()
    {
        yield return new WaitForSeconds(0.3f);
        skill1Effect.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        skill1Effect.SetActive(false);
        isMove = false;
    }

    public void Skill2()
    {
        if (!isSkill2)
        {
            isMove = true;
            isSkill2 = true;
            equipWeapon.Use();
            anim.SetTrigger("doSkill1");
            StopCoroutine("Skill2Effect");
            StartCoroutine("Skill2Effect");
        }
    }

    IEnumerator Skill2Effect()
    {
        yield return new WaitForSeconds(0.3f);
        skill2Effect.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        skill2Effect.SetActive(false);
        isMove = false;
    }

    public void Skill3()
    {
        if (!isSkill3)
        {
            isMove = true;
            isSkill3 = true;
            equipWeapon.Use();
            anim.SetTrigger("doSkill1");
            StopCoroutine("Skill3Effect");
            StartCoroutine("Skill3Effect");
        }
    }

    IEnumerator Skill3Effect()
    {
        yield return new WaitForSeconds(0.3f);
        skill3Effect.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        skill3Effect.SetActive(false);
        isMove = false;
    }

    public void Skill4()
    {
        if (!isSkill4)
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
        yield return new WaitForSeconds(0.3f);
        skill4Effect.SetActive(true);
        GameObject effect = Instantiate(effect4, transform.position, transform.rotation);
        Rigidbody rigidEffect = effect.GetComponent<Rigidbody>();
        rigidEffect.velocity = transform.forward * 30;
        yield return new WaitForSeconds(0.5f);
        Destroy(effect);
        skill4Effect.SetActive(false);
        isMove = false;
    }

    public void Cooldown()
    {
        if (isSkill1)
        {
            skill1Cooldown += 1 * Time.deltaTime;
            if (skill1Cooldown > 5)
            {
                skill1Cooldown = 0;
                isSkill1 = false;
            }
        }
        if (isSkill2)
        {
            skill2Cooldown += 1 * Time.deltaTime;
            if (skill2Cooldown > 5)
            {
                skill2Cooldown = 0;
                isSkill2 = false;
            }
        }
        if (isSkill3)
        {
            skill3Cooldown += 1 * Time.deltaTime;
            if (skill3Cooldown > 5)
            {
                skill3Cooldown = 0;
                isSkill3 = false;
            }
        }
        if (isSkill4)
        {
            skill4Cooldown += 1 * Time.deltaTime;
            if (skill4Cooldown > 5)
            {
                skill4Cooldown = 0;
                isSkill4 = false;
            }
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
}
