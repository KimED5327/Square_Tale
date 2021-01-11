using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 퀘스트 HUD UI를 관리하는 클래스 
/// </summary>
public class QuestHUD : MonoBehaviour
{
    [Header("QuestHUD Panel")]
    [SerializeField] GameObject _questListPanel = null;
    [SerializeField] Image _imgUpdate = null;
    [SerializeField] Button _btnQuest = null;
    [SerializeField] Sprite _imgBtnDefault = null;
    [SerializeField] Sprite _imgBtnSelected = null;

    [Header("QuestList Panel")]
    [SerializeField] Text _questTitle = null;
    [SerializeField] Text _questGoal1 = null;
    [SerializeField] Text _questGoal2 = null;

    //getter 
    public GameObject GetQuestListPanel() { return _questListPanel; }
    public Text GetQuestTitle() { return _questTitle; }
    public Text GetQuestGoal1() { return _questGoal1; }
    public Text GetQuestGoal2() { return _questGoal2; }
    public Image GetUpdateImg() { return _imgUpdate; }
    public Button GetQuestBtn() { return _btnQuest; }

    //setter 
    public void SetQuestTitle(string text) { _questTitle.text = text; }
    public void SetQuestGoal1(string text) { _questGoal1.text = text; }
    public void SetQuestGoal2(string text) { _questGoal2.text = text; }

    /// <summary>
    /// 퀘스트 HUD 내 퀘스트 리스트 패널 열기 
    /// </summary>
    public void OpenQuestList()
    {
        _questListPanel.SetActive(true);
        _btnQuest.image.sprite = _imgBtnSelected;
    }

    /// <summary>
    /// 퀘스트 HUD 내 퀘스트 리스트 패널 닫기 
    /// </summary>
    public void CloseQuestList()
    {
        _questListPanel.SetActive(false);
        _btnQuest.image.sprite = _imgBtnDefault;
    }

    /// <summary>
    /// 퀘스트 HUD 내 퀘스트 리스트 패널 열고 닫기 
    /// </summary>
    public void ToggleQuestList()
    {
        if (_questListPanel.activeInHierarchy) CloseQuestList();
        else OpenQuestList();
    }
}
