using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenEffect : MonoBehaviour
{
    public static ScreenEffect instance;

    [SerializeField] Image _imgFade = null;

    bool _isFinished = true;

    [SerializeField] float _fadeSpeed = 1f;
    [SerializeField] float _splashSpeed = 1f;
    [SerializeField] float _splashDestAlpha = 0.15f;
    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
            ExecuteFadeOut();
        else if (Input.GetKeyDown(KeyCode.Alpha0))
            ExecuteFadeIn();
        else if (Input.GetKeyDown(KeyCode.Minus))
            ExecuteSplash(_splashDestAlpha);

    }

    // 페이드 아웃 (깜깜하게)
    public void ExecuteFadeOut()
    {
        StopAllCoroutines();
        _isFinished = false;
        StartCoroutine(FadeCoroutine(0f));
    }

    // 페이드 인 (원래대로)
    public void ExecuteFadeIn()
    {
        StopAllCoroutines();
        _isFinished = false;
        StartCoroutine(FadeCoroutine(1f));
    }

    // 스플래시 효과 (순간 번쩍임)
    public void ExecuteSplash(float destAlpha)
    {
        StopAllCoroutines();
        StartCoroutine(SplashEffect(destAlpha));
    }

    IEnumerator FadeCoroutine(float destAlpha)
    {
        _imgFade.gameObject.SetActive(true);

        Color color = Color.black;
        color.a = _imgFade.color.a;
        _imgFade.color = color;
        
        while (_imgFade.color.a != destAlpha)
        {
            color.a = Mathf.MoveTowards(_imgFade.color.a, destAlpha, _fadeSpeed * Time.deltaTime);
            _imgFade.color = color;
            yield return null;
        }

        color.a = destAlpha;
        _imgFade.color = color;
        _isFinished = true;

        if(destAlpha == 0)
            _imgFade.gameObject.SetActive(false);
    }


    IEnumerator SplashEffect(float destAlpha)
    {
        _imgFade.gameObject.SetActive(true);

        Color color = Color.white;
        color.a = 0;
        _imgFade.color = color;

        
        while(_imgFade.color.a != destAlpha)
        {
            color.a = Mathf.MoveTowards(_imgFade.color.a, destAlpha, _splashSpeed * Time.deltaTime);
            _imgFade.color = color;
            yield return null;
        }

        while (_imgFade.color.a != 0)
        {
            color.a = Mathf.MoveTowards(_imgFade.color.a, 0, _splashSpeed * Time.deltaTime);
            _imgFade.color = color;
            yield return null;
        }
        _imgFade.gameObject.SetActive(false);
    }

    public bool IsFinished() { return _isFinished; }
}
