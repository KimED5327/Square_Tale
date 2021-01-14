using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraEventMoving
{
    public Transform tfMovePoint;
    public float waitTime;
}


public class Trigger : MonoBehaviour
{
    protected const int WAIT = 0, PLAY = 1;
    protected const string ANIM_PLAY = "Play", ANIM_FORCE_FINISH = "Finish";

    [SerializeField] protected int _eventID = 0;
    [SerializeField] protected CameraEventMoving[] _eventMove;
    [SerializeField] protected float _speed = 0.035f;

    Transform _nextLoc;
    Vector3 _camOriginPos;
    Quaternion _camOriginRot;

    protected bool _isActivated = false;
    bool _isEvent = false;
    bool _isBack = false;

    Animator _anim;
    Camera _cam;
    CameraController theCameraController;
    //PlayerMove thePlayerMove;


    private void Start()
    {
        theCameraController = FindObjectOfType<CameraController>();
        //thePlayerMove = theCameraController.GetComponent<PlayerMove>();
        _anim = GetComponent<Animator>();
        _cam = Camera.main;

        // 최초 실행시 키 생성
        if (!PlayerPrefs.HasKey(StringManager.GetEventKey(_eventID)))
            PlayerPrefs.SetInt(StringManager.GetEventKey(_eventID), WAIT);
        
        // 이후부턴 키값 비교
        else
        {
            int status = PlayerPrefs.GetInt(StringManager.GetEventKey(_eventID));

            // 이미 활성화된 상황이라면 강제로 트리거 활성화
            if (status == PLAY)
            {
                _isActivated = true;
                _anim.SetTrigger(ANIM_FORCE_FINISH);
            }
        }
    }

    private void FixedUpdate()
    {
        // 이벤트 중이면 카메라 무빙 시작
        if (_isEvent)
        {

            // 트랜스폼 목표를 따라 카메라 이동
            if (!_isBack)
            {
                _cam.transform.position = Vector3.Lerp(_cam.transform.position, _nextLoc.position, _speed);
                _cam.transform.rotation = Quaternion.Lerp(_cam.transform.rotation, _nextLoc.rotation, _speed);
            }

            // 카메라 원점 복귀
            else
            {
                _cam.transform.position = Vector3.Lerp(_cam.transform.position, _camOriginPos, _speed + _speed * 0.5f);
                _cam.transform.rotation = Quaternion.Lerp(_cam.transform.rotation, _camOriginRot, _speed + _speed * 0.5f);

                // 복귀 완료
                if (Vector3.SqrMagnitude(_cam.transform.position - _camOriginPos) <= 0.03f)
                    if (Quaternion.Angle(_cam.transform.rotation, _camOriginRot) <= 0.1f)
                    {
                        _isEvent = false;
                        _isBack = false;
                        PlayerMove.s_canMove = true;
                        theCameraController.SetCanMove(true);
                        GameHudMenu.instance.ShowMenu();
                        InteractionManager._isOpen = false;
                    }
            }
        }
        
    }

    // 트리거 작동 시작
    public virtual void ActiveTrigger()
    {
        StartCoroutine(ActiveCoroutine());
    }


    // 트리거 내용 실행
    protected IEnumerator ActiveCoroutine()
    {
        PlayerMove.s_canMove = false;
        InteractionManager._isOpen = true;
        _isActivated = true;
        PlayerPrefs.SetInt(StringManager.GetEventKey(_eventID), PLAY);
        GameHudMenu.instance.HideMenu();
        
        theCameraController.SetCanMove(false);

        _camOriginPos = _cam.transform.position;
        _camOriginRot = _cam.transform.rotation;


        yield return new WaitForSeconds(0.75f);

        _anim.SetTrigger(ANIM_PLAY);
        StartCoroutine(CamMoving());
    }


    // 캠 무빙
    IEnumerator CamMoving()
    {
        for (int i = 0; i < _eventMove.Length; i++)
        {
            _nextLoc = _eventMove[i].tfMovePoint;
            _isEvent = true;
            while (true)
            {
                if (Vector3.SqrMagnitude(_cam.transform.position - _nextLoc.position) <= 0.5f)
                    if (Quaternion.Angle(_cam.transform.rotation, _nextLoc.rotation) <= 0.75f)
                        break;
                yield return null;
            }

            yield return new WaitForSeconds(_eventMove[i].waitTime);
        }
        _isBack = true; 
    }
}