using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TutorialType
{
    HUD,
    INVENTORY,
    SKILL,
    BLOCK,
    QUEST,
    ADVENTURE,
    SHOP,
    BASIC,
}

[System.Serializable]
public class TutorialInfo
{
    public string name;                        // 인스펙터용
    public Transform[] _tfArrowTarget;         // 타겟팅할 객체들
    public string[] _strTutorialQuestTitle;    // 튜토리얼 설명 타이틀
    [TextArea(1, 3)]
    public string[] _strTutorialQuest;         // 튜토리얼 설명
    public float[] _holderPosX;                // 튜토리얼 별 홀더 위치
}

public class Tutorial : MonoBehaviour
{
    const int HUD_START_INDEX = 0;
    const int TUTORIAL_QUEST_ID = 1;

    public static bool[] _isLookTutorial;        // 튜토리얼을 어디까지 봤는지 기록

    [Header("Tutorial UI")]
    [SerializeField] GameObject _goAllBlack = null;             // 백그라운드 캔버스
    [SerializeField] GameObject _tutorialUI = null;             // 캔버스
    [SerializeField] Transform _tfHolder = null;                // 홀더 위치 정보
    [SerializeField] GameObject _goExitBtn = null;              // 종료 버튼
    [SerializeField] Image _imgLeftBtn = null;                  // 왼쪽 화살표 버튼
    [SerializeField] Image _imgRightBtn = null;                 // 오른쪽 화살표 버튼
    [SerializeField] Image[] _imgDots = null;                   // 진행도
    [Space(20)]
    [SerializeField] Image _goTargetBackground = null;     // 타겟 강조 배경
    [SerializeField] Transform[] _tfArrowTarget = null;         // 타겟팅 객체 집합
    [SerializeField] GameObject _goArrow = null;                // 타겟팅 화살표
    [SerializeField] Transform _tfCenter = null;                // 화살표 방향처리용
    [SerializeField] GameObject _goScroll = null;               // 스크롤 이미지
    [SerializeField] Text _txtQuestTutorial = null;             // 텍스트
    [SerializeField] Text _txtQuestTutorialTitle = null;        // 타이틀 텍스트

    [Header("Hud Info")]
    [SerializeField] TutorialInfo[] _tutorialInfo = null;       // 튜토리얼 정보
    [SerializeField] float _arrowDistance = 100f;               // 튜토리얼 전용 화살표 거리 조정

    TutorialType _curTutorial;                                  // 진행중인 튜토리얼 타입
    int _curTutorialCount;                                      // 진행중인 튜토리얼 번호

    int _curTutorialLastIndex = 0;                              // 해당 튜토리얼의 마지막 인덱스
    int _curIndex;                                              // 해당 튜토리얼의 현재 인덱스

    bool _canNextTutorial = false;                              // 다음 튜토리얼로 넘어갈 수 있는지 체크
    bool _canExit = false;                                      // 튜토리얼 종료가 가능한지 체크
    bool _isForce = false;                                      // 튜토리얼을 도움말에서 강제로 띄운 건지 체크

    Vector3 _centerPos;                                         // 튜토리얼 캔버스의 중심 좌표

    Color _buttonOriginColor;                                      // 버튼의 본래 색깔

    // 필요한 컴포넌트
    QuestManager _quest;

    // 시작과 동시에 튜토리얼 체크
    void Start()
    {
        _isLookTutorial = new bool[_tutorialInfo.Length];
        _quest = QuestManager.instance;
        _centerPos = _tfCenter.localPosition;
        _buttonOriginColor = _imgLeftBtn.color;

        LoadTutorial();

        CallTutorial(TutorialType.BASIC);
    }

    /// <summary>
    /// 튜토리얼을 진행합니다. 
    /// 어떤 튜토리얼을 진행시킬지는 튜토리얼 타입을 넘겨주면 됩니다.
    /// </summary>
    /// <param name="tutorialStartIndex"></param>
    public void CallTutorial(TutorialType type, bool isForce = false)
    {
        // 캐싱
        _curTutorial = type;
        _curTutorialCount = (int)_curTutorial;
        _isForce = isForce;
        _canExit = false;

        // 이미 본 튜토리얼이면 실행 안 함 (강제로 실행시킬 경우는 제외 : ex 도움말)
        if (_isLookTutorial[_curTutorialCount] && !_isForce) return;

        // 튜토리얼 정보 세팅
        SettingTutorialInfo();

        // 튜토리얼 시작
        StartCoroutine(StartTutorial());
    }


    // 튜토리얼 진행 전 세팅
    void SettingTutorialInfo()
    {
        _imgRightBtn.color = _buttonOriginColor;
        _imgLeftBtn.color = Color.gray;

        _curTutorialLastIndex = _tutorialInfo[_curTutorialCount]._tfArrowTarget.Length - 1;
        _curIndex = 0;
    }


    // 튜토리얼 시작
    IEnumerator StartTutorial()
    {
        _isLookTutorial[_curTutorialCount] = true;

        QuestTutorialUI(true);
        while (!_canExit)
        {
            NextShowTutorial(_curIndex);
            yield return new WaitUntil(() => _canNextTutorial);
            _canNextTutorial = false;
        }
        QuestTutorialUI(false);
    }

    // UI 출력 여부
    void QuestTutorialUI(bool flag)
    {
        _tutorialUI.SetActive(flag);
        _goScroll.SetActive(flag);
        _goAllBlack.SetActive(flag);
        _goTargetBackground.gameObject.SetActive(flag);
        _goArrow.SetActive(flag);
        _goExitBtn.SetActive(!flag);
    }

    // 다음 튜토리얼 인덱스 진행
    void NextShowTutorial(int index, bool isTargetting = true)
    {
        // 관련 튜토리얼을 다 읽었으면 종료 버튼 출력
        if (_curIndex >= _curTutorialLastIndex)
            _goExitBtn.SetActive(true);

        TutorialInfo info = _tutorialInfo[_curTutorialCount];
        // 타겟팅 (강조) 이 필요한 튜토리얼이면
        if (isTargetting)
        {
            // 타겟팅
            Vector3 targetPos = info._tfArrowTarget[index].localPosition;
            _goTargetBackground.transform.localPosition = targetPos;
            _goArrow.transform.localPosition = targetPos;

            Vector3 dir = (targetPos - _centerPos).normalized;
            _goArrow.transform.right = -dir;
            _goArrow.transform.localPosition += _goArrow.transform.right * _arrowDistance;

            // 타겟팅 백그라운드 조절
            Color color = _goTargetBackground.color;
            color.a = (targetPos == _centerPos) ? 0.25f : 0.8f ;
            _goTargetBackground.color = color;

            // 화살표 표시
            if (targetPos == _centerPos)
                _goArrow.gameObject.SetActive(false);
            else
                _goArrow.gameObject.SetActive(true);
        }
        // 타겟팅이 필요없으면 중앙으로 원위치
        else
        {
            _goTargetBackground.transform.localPosition = _centerPos;
            _goArrow.transform.localPosition = _centerPos;
        }
        _tfHolder.localPosition = _centerPos + new Vector3(info._holderPosX[index], 0f, 0f);

        // 점 세팅
        DotSetting(_curTutorialLastIndex);
        _imgDots[index].color = Color.white;

        // 텍스트 세팅
        _txtQuestTutorial.text = info._strTutorialQuest[index];
        _txtQuestTutorialTitle.text = info._strTutorialQuestTitle[index];
    }


    // 점 세팅
    void DotSetting(int index)
    {
        for (int i = 0; i < _imgDots.Length; i++)
            _imgDots[i].gameObject.SetActive(false);


        for (int i = 0; i <= index; i++)
            _imgDots[i].gameObject.SetActive(true);

        for (int i = 0; i < _imgDots.Length; i++)
            _imgDots[i].color = Color.gray;
    }

    // 다음 버튼
    public void NextBtn()
    {
        bool isOverIndex = _curIndex >= _curTutorialLastIndex - 1;

        _curIndex = isOverIndex ? _curTutorialLastIndex
                                : _curIndex += 1;
        _imgRightBtn.color = isOverIndex ? Color.gray
                                         : _buttonOriginColor;
        _imgLeftBtn.color = _buttonOriginColor;
        _canNextTutorial = true;
    }

    // 이전 버튼
    public void PriorBtn()
    {
        bool isOverIndex = _curIndex <= 1;

        _curIndex = isOverIndex ? _curIndex = 0
                                : _curIndex -= 1;
        _imgLeftBtn.color = isOverIndex ? Color.gray
                                        : _buttonOriginColor;
        _imgRightBtn.color = _buttonOriginColor;
        _canNextTutorial = true;
    }

    /// <summary>
    /// 튜토리얼 종료 버튼
    /// </summary>
    public void BtnExit()
    {
        // HUD 튜토리얼일 경우
        if(_curTutorial == TutorialType.HUD && !_isForce)
            DialogueManager.instance.CompleteQuest();
        
        _canExit = true;
        SaveTutorial();
        QuestTutorialUI(false);
    }

    public static void SaveTutorial()
    {
        if (!PlayerPrefs.HasKey("Tutorial7"))
            PlayerPrefs.SetString("Tutorial7", false.ToString());

        for (int i = 0; i < _isLookTutorial.Length; i++)
        {
            PlayerPrefs.SetString("Tutorial" + i, _isLookTutorial[i].ToString());
        }
    }
    public static void LoadTutorial()
    {
        if (PlayerPrefs.HasKey("Tutorial0"))
        {

            if(!PlayerPrefs.HasKey("Tutorial7"))
                PlayerPrefs.SetString("Tutorial7", false.ToString());

            for (int i = 0; i < _isLookTutorial.Length; i++)
            {
                _isLookTutorial[i] = bool.Parse(PlayerPrefs.GetString("Tutorial" + i));
            }
        }
        else
        {
            SaveTutorial();
        }
    }
}
