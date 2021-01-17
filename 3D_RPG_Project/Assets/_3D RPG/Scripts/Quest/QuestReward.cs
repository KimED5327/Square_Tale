using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 퀘스트 메뉴 및 퀘스트 완료 UI 내 퀘스트 보상 클래스 
/// </summary>
public class QuestReward : MonoBehaviour
{
    [SerializeField] Image _imgItem = null;
    [SerializeField] Image _imgCount = null;
    [SerializeField] Text _txtCount = null;

    //getter
    public Image GetImg() { return _imgItem; }
    public Text GetCount() { return _txtCount; }

    //setter
    public void SetImg(Sprite sprite) { _imgItem.sprite = sprite; }
    public void SetCount(int count) { _txtCount.text = "" + count; }
    public void TurnOffCount() { _txtCount.gameObject.SetActive(false); }

}
