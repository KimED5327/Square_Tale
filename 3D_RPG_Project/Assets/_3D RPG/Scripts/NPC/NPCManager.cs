using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCManager : MonoBehaviour
{
    public TalkTest _talkTest;
    public GameObject _dialoguePanel;
    public Text _line;
    public GameObject _scanObj;
    public bool _isTalking;
    int lineIdx = 0;
    NpcWithLines npc;

    public void Test()
    {
    }

    public void Dialogue(NPC npc)
    {
        Talk(npc.GetID());
        _dialoguePanel.SetActive(_isTalking);
    }
  
    void Talk(int npcId)
    {
        string line = _talkTest.GetTalk(npcId, lineIdx);

        if (line == null)
        {
            _isTalking = false;
            lineIdx = 0;
            return; 
        }

        _line.text = line;
        _isTalking = true;
        lineIdx++;
    }
}
