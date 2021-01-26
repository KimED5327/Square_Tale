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
    [SerializeField] Image _imgName = null;
    [SerializeField] Text _txtCount = null;
    [SerializeField] Text _txtName = null;

    public void TurnOnNameTag()
    {
        // 현재 네임태그가 비활성화 상태라면 활성화 시키기 
        if(!_imgName.gameObject.activeInHierarchy) _imgName.gameObject.SetActive(true);

        SoundManager.instance.PlayEffectSound("Coin");
    }

    //getter
    public Image GetImg() { return _imgItem; }
    public Text GetCount() { return _txtCount; }
    public Text GetName() { return _txtName; }

    //setter
    public void SetImg(Sprite sprite) { _imgItem.sprite = sprite; }
    public void SetCount(int count) { _txtCount.text = "" + count; }
    public void SetName(string name) { _txtName.text = name; }
    public void TurnOffCount() { _txtCount.gameObject.SetActive(false); }

}
