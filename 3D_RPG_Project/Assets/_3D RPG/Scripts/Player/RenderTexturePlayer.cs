using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTexturePlayer : MonoBehaviour
{
    Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _anim.Play("IdleA");
    }
}
