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
    [SerializeField] GameObject _questListPanel = null;     // 퀘스트 정보를 출력하는 패널 
    [SerializeField] GameObject _iconCompletable = null;    // 퀘스트 완료가능 아이콘(빨간색 원) 
    [SerializeField] Button _btnQuest = null;               // 퀘스트 버튼 아이콘  
    [SerializeField] Sprite _imgBtnDefault = null;          // 퀘스트 버튼 기본 이미지 
    [SerializeField] Sprite _imgBtnSelected = null;         // 퀘스트 버튼 눌렀을 때 이미지 

    [Header("QuestList Panel")]
    [SerializeField] Text _questTitle = null;               // 퀘스트 제목 
    [SerializeField] Text _questGoal1 = null;               // 퀘스트 목표1
    [SerializeField] Text _questGoal2 = null;               // 퀘스트 목표2 

    bool _isHudOpen = false;
    bool _isCompletableIconOn = false; 


    private void Start()
    {        
        // 객체가 활성화될 때마다 퀘스트 매니져와 링크 
        QuestManager.instance.InitializeLink();

        // 객체가 활성화될 때마다 HUD 데이터값 업데이트 (씬이 변경되기 전의 데이터와 싱크 맞추기)
        UpdateHudOnStart();
    }

    /// <summary>
    /// 퀘스트 HUD 내 퀘스트 리스트 패널 열기 
    /// </summary>
    public void OpenQuestList()
    {
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

    /// <summary>
    /// 퀘스트 HUD 비활성화 
    /// </summary>
    public void DisalbeHUD()
    {
        CloseQuestList();
        GetQuestBtn().enabled = false;
        TurnOffCompletableIcon();
        QuestManager.instance.SetIsCompletableIconOn(false);
        _isCompletableIconOn = false;
    }

    /// <summary>
    /// 씬이 초기화될 때 퀘스트 HUD 데이터 업데이트
    /// </summary>
    public void UpdateHudOnStart()
    {
        // 현재 진행중인 퀘스트가 없을 경우 HUD를 비활성화시키고 리턴 
        if(QuestManager.instance.GetOngoingQuestList().Count <= 0)
        {
            DisalbeHUD();
            return; 
        }

        // 퀘스트 매니져에 퀘스트 HUD 메뉴, 완료가능 아이콘 on/off 여부 저장해놓고 UI에 적용
        if (_isHudOpen) OpenQuestList();
        if (_isCompletableIconOn) TurnOnCompletableIcon();

        // 진행중인 퀘스트 리스트의 0번 index 값을 가져와서 데이터 값 세팅  
        Quest quest = QuestManager.instance.GetOngoingQuestByIdx(0);

        SetQuestTitle(quest.GetTitle());
        SetQuestGoal2("");

        // 진행중인 퀘스트의 타입에 맞게 HUD 내 퀘스트 목표값 설정 
        SetQuestGoal1(QuestManager.instance.GetQuestGoal(quest));
    }

    /// <summary>
    /// 퀘스트를 수락하거나, 퀘스트 목표 데이터가 변경될 때 HUD 업데이트 
    /// </summary>
    /// <param name="quest"></param>
    public void UpdateHUD(Quest quest)
    {
        SetQuestTitle(quest.GetTitle());
        SetQuestGoal2("");

        SetQuestGoal1(QuestManager.instance.GetQuestGoal(quest));
    }

    /// <summary>
    /// 퀘스트 HUD에 진행중인 퀘스트의 데이터 값 세팅하기 
    /// </summary>
    /// <param name="quest"></param>
    public void SetOngoingQuestHUD(Quest quest)
    {
        OpenQuestList();
        GetQuestBtn().enabled = true;
        UpdateHUD(quest);
    }

    //getter 
    public GameObject GetQuestListPanel() { return _questListPanel; }
    public GameObject GetUpdateImg() { return _iconCompletable; }
    public Text GetQuestTitle() { return _questTitle; }
    public Text GetQuestGoal1() { return _questGoal1; }
    public Text GetQuestGoal2() { return _questGoal2; }
    public Button GetQuestBtn() { return _btnQuest; }
    public bool GetIsHudOpen() { return _isHudOpen; }
    public bool GetIsCompletableIconOn() { return _isCompletableIconOn; }

    //setter 
    public void SetQuestTitle(string text) { _questTitle.text = text; }
    public void SetQuestGoal1(string text) { _questGoal1.text = text; }
    public void SetQuestGoal2(string text) { _questGoal2.text = text; }
    public void TurnOnCompletableIcon() { _iconCompletable.SetActive(true); }
    public void TurnOffCompletableIcon() { _iconCompletable.SetActive(false); }
    public void SetIsHudOpen(bool value) { _isHudOpen = value; }
    public void SetIsCompletableIconOn(bool value) { _isCompletableIconOn = value; }
}
