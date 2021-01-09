using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//NPC 네임태그를 클릭하면 줌인
//X 아이콘을 클릭하면 줌아웃 
public class ZoomNPC : MonoBehaviour
{
    /// <summary>
    /// 줌인 시 대상의 위치(좌측, 정면, 우측) 설정 스테이트 
    /// </summary>
    enum ZoomState  
    {
        ZOOM_LEFT,
        ZOOM_RIGHT,
        ZOOM_CENTER
    };

    [SerializeField] ZoomState _zoomState = ZoomState.ZOOM_CENTER;    // 줌인 상태(좌측, 정면, 우측)
    [SerializeField] Vector3 _targetOffset;                           // 줌인 시 오프셋 값  
    [SerializeField] float _smoothSpeed = 0.125f;                     // 줌인 시 속도 
    [SerializeField] bool _zoomedInToggle = false;                    // 줌인 토글(on/off 설정)
    [SerializeField] bool _zoomingIn = false;                         // 줌인 실행중인지 확인하는 bool값 
    [SerializeField] bool _zoomingOut = false;                        // 줌아웃 실행중인지 확인하는 bool값 

    Camera _cam;                    // 카메라  
    GameObject _player;             // 플레이어 
    GameObject _hudCanvas;          // HUD 캔버스 
    Transform _target;              // 타겟 NPC
    float _minDistance = 2.0f;      // 줌인 발동 최소거리 
    Vector3 _prePos;                // 줌인 시 카메라 이동 전 위치값 

    private void Awake()
    {
        _cam = Camera.main;
        _player = FindObjectOfType<PlayerMove>().gameObject;
        _hudCanvas = FindObjectOfType<GameHudMenu>().gameObject;
        _target = transform;
        _zoomedInToggle = false;

        // 카메라 오프셋값 설정 
        _targetOffset = new Vector3(-1.1f, -0.1f, 0.01f);
    }

    private void FixedUpdate()
    {
        if (_zoomingIn)
        {
            ZoomedIn();
        }

        if (_zoomingOut)
        {
            ZoomedOut();
        }
    }

    //NPC 클릭 시 NPC 줌인 
    public void ZoomInNPC()
    {
        // 플레이어와 NPC의 거리가 일정 거리 이하일 때만 줌인 발동 
        if (Vector3.Distance(_player.transform.position, _target.position) > _minDistance) return; 

        if (_zoomedInToggle == false)
        {
            _zoomedInToggle = true;
            _zoomingIn = true;
            _prePos = _cam.transform.position;

            //카메라 컨트롤러 끄기 
            _player.GetComponent<CameraController>().enabled = false;

            // HUD 캔버스 비활성화 
            _hudCanvas.SetActive(false);

            Debug.Log("zoomin working");
        }
    }

    //X 아이콘 클릭 시 NPC 줌아웃 
    public void ZoomOutNPC()
    {
        if (_zoomedInToggle == true)
        {
            _zoomedInToggle = false;
            _zoomingOut = true;

            //카메라 컨트롤러 켜기 
            _player.GetComponent<CameraController>().enabled = true;
        }
    }

    public void ZoomedIn()
    {
        switch (_zoomState)
        {
            case ZoomState.ZOOM_LEFT:
                break;

            case ZoomState.ZOOM_RIGHT:
                break;

            case ZoomState.ZOOM_CENTER:
                Vector3 desiredPos = _target.position + _targetOffset;

                if (Vector3.Distance(_cam.transform.position, desiredPos) <= 0.1f)
                {
                    _zoomingIn = false;
                    _zoomedInToggle = false;
                    transform.GetComponent<QuestNPC>().ClickNPC();
                }

                Vector3 smoothPos = Vector3.Lerp(_cam.transform.position, desiredPos, _smoothSpeed);
                _cam.transform.position = smoothPos;
                _cam.transform.LookAt(_target);
                break;
        }
    }

    public void ZoomedOut()
    {
        if (Vector3.Distance(_cam.transform.position, _prePos) <= 0.1f)
        {
            _zoomingOut = false;

            //카메라 컨트롤러 켜기 
            //_player.GetComponent<CameraController>().enabled = true;
        }

        Vector3 smoothPos = Vector3.Lerp(_cam.transform.position, _prePos, _smoothSpeed);
        _cam.transform.position = smoothPos;
    }
}
