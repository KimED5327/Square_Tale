using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCam : MonoBehaviour
{
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();

    }

    public void DeActive()
    {
        gameObject.SetActive(false);
    }

    public void PlayAnimator(string code)
    {
        anim.SetTrigger(code);
    }
}
