using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSlide : MonoBehaviour
{

    const string SHOW = "Show";
    const string HIDE = "Hide";

    Animator myAnim;

    bool isShow = false;
    bool canTouch = true;

    void Awake() => myAnim = GetComponent<Animator>();

    public void OnTouchMenu()
    {
        if (canTouch)
        {
            SoundManager.instance.PlayEffectSound("Click");
            isShow = !isShow;
            StartCoroutine(SlideCo());
        }
    }


    IEnumerator SlideCo()
    {
        canTouch = false;

        if (isShow) myAnim.SetTrigger(SHOW);
        else myAnim.SetTrigger(HIDE);
        yield return new WaitForSeconds(0.25f);
        
        canTouch = true;
    }
}
