using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignPost : MonoBehaviour
{
    [SerializeField] int _id = 1;
    [SerializeField] Text _txtBalloon = null;
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(StringManager.playerTag))
        {
            _txtBalloon.text = "필요한 내용은 이곳에 넣어주면 된다.";
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
