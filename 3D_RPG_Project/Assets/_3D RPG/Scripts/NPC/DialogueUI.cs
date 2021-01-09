using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 다이얼로그 UI 데이터를 관리하는 클래스  
/// </summary>
public class DialogueUI : MonoBehaviour
{
    [SerializeField] GameObject _dialoguePanel = null;
    [SerializeField] GameObject _questDialoguePanel = null;

    [Header("Dialogue Panel UI")]
    [SerializeField] Text _txtNpcName = null;
    [SerializeField] Text _txtLines = null;

    //[Header("Quest Dialogue Panel UI")]

    public GameObject GetDialoguePanel() { return _dialoguePanel; }
    public GameObject GetQuestPanel() { return _questDialoguePanel; }

    /// <summary>
    /// 기본 다이얼로그 패널 내 txtNpcName 리턴 
    /// </summary>
    /// <returns></returns>
    public Text GetNpcName() { return _txtNpcName; }

    /// <summary>
    /// 기본 다이얼로그 패널 내 txtLines 리턴 
    /// </summary>
    /// <returns></returns>
    public Text GetLines() { return _txtLines; }

}
