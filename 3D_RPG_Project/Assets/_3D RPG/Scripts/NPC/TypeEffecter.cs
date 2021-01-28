using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 기본/퀘스트 다이얼로그 구분용 타입 
public enum DialogueType
{
    DEFAULT,
    QUEST,
};

public class TypeEffecter : MonoBehaviour
{
    [SerializeField] GameObject endCursor;      // 
    [SerializeField] Text _txtQuestLine;        // 퀘스트 다이얼로그 패널 내 대사 텍스트 
    [SerializeField] Text _txtDefaultLine;      // 기본 다이얼로그 패널 내 대사 텍스트 
    [SerializeField] int _charPerSeconds;       // 1초에 출력되는 문자 개수 
    DialogueType _dialogueType;                 // 기본/퀘스트 다이얼로그 타입 
    string _targetMsg;                          // 타이핑 이펙트 타겟 메시지 
    string _msgText;                            // 타이핑 이펙트로 출력된 메시지 
    int _index;                                 // 타이핑 이펙트가 진행되고 있는 메시지의 인덱스 값 
    bool _isAnim = false;                       // 현재 타이핑 이펙트가 진행되고 있는지 여부 

    // 타이핑 애니메이션 값 세팅 및 시작 
    public void SetMsg(DialogueType type, string msg)
    {
        _msgText = "";
        _targetMsg = msg;
        _dialogueType = type;
        EffectStart();
    }

    // 타이핑 애니메이션 시작 함수 
    void EffectStart()
    {
        if (endCursor != null) endCursor.SetActive(false);
        _txtQuestLine.text = "";
        _txtDefaultLine.text = "";
        _index = 0;

        // 애니메이션 실행 
        Invoke("Effecting", 1f / _charPerSeconds);
        _isAnim = true;
    }

    // 타이핑 애니메이션 진행 함수 
    void Effecting()
    {
        // 모든 대사 출력 시 애니메이션 종료 
        if (_index == _targetMsg.Length)
        {
            EffectEnd();
            return;
        }

        // 슬래시 발견 시 개간 
        if (_targetMsg[_index] == '/')
        {
            _msgText += "\n";
        }
        else
        {
            _msgText += _targetMsg[_index];
        }

        if (_dialogueType == DialogueType.DEFAULT) _txtDefaultLine.text = _msgText;
        else _txtQuestLine.text = _msgText;

        // 타자 사운드 플레이 
        if (_targetMsg[_index] != ' ')
        {
            // 사운드 플레이 함수 호출 
        }

        _index++;

        // 재귀함수 
        Invoke("Effecting", 1f / _charPerSeconds);
    }

    // 타이핑 애니메이션 종료 함수 
    public void EffectEnd()
    {
        if (endCursor != null) endCursor.SetActive(true);

        _isAnim = false;
    }

    // 모든 대사 출력 
    public void ShowFullLine()
    {
        if (_isAnim)
        {
            _msgText = GetFullLine(_targetMsg);

            if (_dialogueType == DialogueType.DEFAULT) _txtDefaultLine.text = _msgText;
            else _txtQuestLine.text = _msgText;

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
            if (line[i] == '/')
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

    // 값 초기화 
    public void Initialize()
    {
        CancelInvoke();

        _index = 0;
        _isAnim = false;
        if (endCursor != null) endCursor.SetActive(false);
    }

    // getter
    public bool GetIsAnim() { return _isAnim; }
    public int GetIdx() { return _index; }

    // setter 
    public void SetIsAnim(bool value) { _isAnim = value; }
    public void SetIdx(int index) { _index = index; } 
}
