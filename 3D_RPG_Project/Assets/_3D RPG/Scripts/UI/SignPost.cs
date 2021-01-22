using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignPost : MonoBehaviour
{
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(StringManager.playerTag))
        {
            anim.SetTrigger("Show");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(StringManager.playerTag))
        {
            anim.SetTrigger("Hide");
        }
    }



}
