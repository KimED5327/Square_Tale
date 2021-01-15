using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestSlot : MonoBehaviour
{
    [SerializeField] Text _txtTitle;        // 슬롯란의 퀘스트 제목 
    [SerializeField] Image _imgQuestMark;   // 퀘스트 마크 
    [SerializeField] GameObject _imgGray;   // 버튼 선택용 마스크 이미지 
    QuestMenu _questMenu;                   // 퀘스트 메뉴 참조값 
    Quest _quest;                           // 퀘스트 참조값 

    private void Start()
    {
        _questMenu = FindObjectOfType<QuestMenu>();
    }

    public void SelectSlot()
    {
        // 나머지 버튼들은 어두운 색상으로 변경하고, 선택된 버튼만 원색상으로 설정 
        _questMenu.SetFinishedSlotsToGray();
        TurnOnColor();
        _questMenu.SetQuestInfo(_quest);
    }

    //getter 
    public Text GetTitle() { return _txtTitle; }
    public Quest GetQuest() { return _quest; }

    //setter 
    public void SetTitle(string title) { _txtTitle.text = title; }
    public void SetQuest(Quest quest) { _quest = quest; }
    public void TurnOffMark() { _imgQuestMark.enabled = false; }

    /// <summary>
    /// 버튼을 선택하지 않았을 경우 회색 마스크 이미지를 활성화하여 버튼을 어둡게 만들기 
    /// </summary>
    public void TurnOffColor() { _imgGray.SetActive(true); }

    /// <summary>
    /// 버튼을 선택했을 경우 회색 마스크 이미지를 비활성화하여 하이라이트 효과 만들기 
    /// </summary>
    public void TurnOnColor() { _imgGray.SetActive(false); }
}
