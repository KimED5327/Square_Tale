using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 퀘스트 메뉴의 퀘스트 슬롯 
/// </summary>
public class QuestSlot : MonoBehaviour
{
    [SerializeField] Text _txtTitle;                // 슬롯란의 퀘스트 제목 
    [SerializeField] Image _imgQuestMark;           // 퀘스트 마크 
    [SerializeField] GameObject _iconCompletable;   // 완료 가능 아이콘(빨간색 원 이미지)
    [SerializeField] GameObject _imgGray;           // 버튼 선택용 마스크 이미지 
    QuestMenu _questMenu;                           // 퀘스트 메뉴 참조값 
    Quest _quest;                                   // 퀘스트 참조값 

    private void Start()
    {
        _questMenu = FindObjectOfType<QuestMenu>();
    }

    /// <summary>
    /// 퀘스트 슬롯을 선택했을 때, 버튼 색상 변경 및 해당 퀘스트에 대응하는 정보 출력 
    /// </summary>
    public void SelectSlot()
    {
        _questMenu.SetQuestInfo(_quest);
        _questMenu.ShowQuestRewards(_quest);

        // 나머지 버튼들은 원색상으로 변경하고, 선택된 버튼만 회색 마스크 이미지를 활성화  
        _questMenu.SetFinishedSlotsDefault();

        // 버튼의 개수가 2개 이상일 때만 눌렀을 때 색상 변경 
        if (_questMenu.CheckSlotsCount() > 1) TurnOffColor();

        SoundManager.instance.PlayEffectSound("Menu_Click", 0.5f);
    }

    //getter 
    public Text GetTitle() { return _txtTitle; }
    public Quest GetQuest() { return _quest; }

    //setter 
    public void SetTitle(string title) { _txtTitle.text = title; }
    public void SetQuest(Quest quest) { _quest = quest; }
    public void TurnOnCompletableIcon() { _iconCompletable.SetActive(true); }
    public void TurnOffCompletableIcon() { _iconCompletable.SetActive(false); }

    /// <summary>
    /// 버튼을 선택했을 경우 회색 마스크 이미지를 활성화하여 버튼을 어둡게 만들기 
    /// </summary>
    public void TurnOffColor() { _imgGray.SetActive(true); }

    /// <summary>
    /// 버튼을 선택하지 않았을 경우 회색 마스크 이미지를 비활성화하여 하이라이트 효과 만들기 
    /// </summary>
    public void TurnOnColor() { _imgGray.SetActive(false); }
}
