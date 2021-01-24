using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeEffecter : MonoBehaviour
{
    [SerializeField] GameObject endCursor;
    [SerializeField] Text txtQuestLine;
    [SerializeField] Text txtDefaultLine;
    [SerializeField] int charPerSeconds;
    string targetMsg;
    string msgText;
    int _index;
    bool _isAnim = false;

    public void SetMsg(string msg)
    {
        msgText = "";
        targetMsg = msg;
        EffectStart();
    }

    // 타이핑 애니메이션 시작 함수 
    void EffectStart()
    {
        if(endCursor != null) endCursor.SetActive(false);
        txtQuestLine.text = "";
        _index = 0;

        // 애니메이션 실행 
        Invoke("Effecting", 1f / charPerSeconds);
        _isAnim = true; 
    }

    // 타이핑 애니메이션 진행 함수 
    void Effecting()
    {
        // 모든 대사 출력 시 애니메이션 종료 
        if (_index == targetMsg.Length)
        {
            EffectEnd();
            return;
        }

        // 슬래시 발견 시 개간 
        if(targetMsg[_index] == '/')
        {
            msgText += "\n";
        }
        else
        {
            msgText += targetMsg[_index];
        }

        txtQuestLine.text = msgText;

        // 타자 사운드 플레이 
        if(targetMsg[_index] != ' ')
        {
            // 사운드 플레이 함수 호출 
        }

        _index++;

        // 재귀함수 
        Invoke("Effecting", 1f / charPerSeconds);
    }

    // 타이핑 애니메이션 종료 함수 
    void EffectEnd()
    {
        if (endCursor != null) endCursor.SetActive(true);
        _isAnim = false;
    }

    public void ShowFullLine()
    {
        if (_isAnim)
        {
            msgText = GetFullLine(targetMsg);
            txtQuestLine.text = msgText;
            CancelInvoke();
            EffectEnd();
        }
    }

    // 슬래시를 개간으로 전환한 전체 대사 리턴 
    string GetFullLine(string line)
    {
        string fullLine = "";

        for (int i = 0; i < line.Length; i++)
        {
            if(line[i] == '/')
            {
                fullLine += "\n";
            }
            else
            {
                fullLine += line[i];
            }
        }

        return fullLine;
    }

    // getter
    public bool GetIsAnim() { return _isAnim; }

    // setter 
    public void SetIsAnim(bool value) { _isAnim = value; }
}
