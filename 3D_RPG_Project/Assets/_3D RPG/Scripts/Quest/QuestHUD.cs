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
    [SerializeField] GameObject _completableIcon = null;
    [SerializeField] Button _btnQuest = null;
    [SerializeField] Sprite _imgBtnDefault = null;
    [SerializeField] Sprite _imgBtnSelected = null;

    [Header("QuestList Panel")]
    [SerializeField] Text _questTitle = null;
    [SerializeField] Text _questGoal1 = null;
    [SerializeField] Text _questGoal2 = null;


    private void Awake()
    {
        QuestManager.instance.InitializeLink();
    }

    private void Start()
    {
        QuestManager.instance.UpdateQuestHudOnStart();
    }

    //getter 
    public GameObject GetQuestListPanel() { return _questListPanel; }
    public GameObject GetUpdateImg() { return _completableIcon; }
    public Text GetQuestTitle() { return _questTitle; }
    public Text GetQuestGoal1() { return _questGoal1; }
    public Text GetQuestGoal2() { return _questGoal2; }
    public Button GetQuestBtn() { return _btnQuest; }

    //setter 
    public void SetQuestTitle(string text) { _questTitle.text = text; }
    public void SetQuestGoal1(string text) { _questGoal1.text = text; }
    public void SetQuestGoal2(string text) { _questGoal2.text = text; }
    public void TurnOnCompletableIcon() { _completableIcon.SetActive(true); }
    public void TurnOffCompletableIcon() { _completableIcon.SetActive(false); }

    /// <summary>
    /// 퀘스트 HUD 내 퀘스트 리스트 패널 열기 
    /// </summary>
    public void OpenQuestList()
    {
        if (_questListPanel == null) Debug.Log("패널 missing");

        _questListPanel.SetActive(true);
        _btnQuest.image.sprite = _imgBtnSelected;
        QuestManager.instance.SetIsHudOpen(true);
    }

    /// <summary>
    /// 퀘스트 HUD 내 퀘스트 리스트 패널 닫기 
    /// </summary>
    public void CloseQuestList()
    {
        _questListPanel.SetActive(false);
        _btnQuest.image.sprite = _imgBtnDefault;
        QuestManager.instance.SetIsHudOpen(false);
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
