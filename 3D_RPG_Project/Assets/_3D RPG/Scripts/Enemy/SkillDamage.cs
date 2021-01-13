using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDamage : MonoBehaviour
{
    private void OnCollisionStay(Collision collision)
    {
        if(collision.transform.CompareTag("Face"))
        {



            Destroy(this);
        }
    }
}
