using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutEventFunction : MonoBehaviour
{
    [SerializeField] GameObject _objectToFadeOut;

    /// <summary>
    /// 퀘스트 수락 팝업메뉴 비활성화 하기 
    /// </summary>
    public void TurnOffPanel()
    {
        _objectToFadeOut.SetActive(false);
    }
}
