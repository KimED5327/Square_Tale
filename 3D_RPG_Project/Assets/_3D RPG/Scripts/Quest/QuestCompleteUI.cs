using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 퀘스트 완료 UI를 관리하는 클래스 
/// </summary>
public class QuestCompleteUI : MonoBehaviour
{
    [SerializeField] GameObject _questCompletedPanel = null;


    [SerializeField] Text _txtQuestTitle = null;

    [Header("Dialogue Panel UI")]
    [SerializeField] Text _txtNpcName = null;
    [SerializeField] Text _txtLines = null;

}
