using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] GameObject _goDialoguePanel = null;
    [SerializeField] GameObject _goQuestPanel = null;
    [SerializeField] Text _txtNpcName = null;
    [SerializeField] Text _txtLines = null;

    public GameObject GetDialoguePanel() { return _goDialoguePanel; }
    public GameObject GetQuestPanel() { return _goQuestPanel; }
    public Text GetNpcName() { return _txtNpcName; }
    public Text GetLines() { return _txtLines; }

}
