using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//NPC 네임태그를 클릭하면 줌인
//X 아이콘을 클릭하면 줌아웃 
public class ZoomNPC : MonoBehaviour
{
    enum ZoomState
    {
        ZOOM_LEFT,
        ZOOM_RIGHT,
        ZOOM_CENTER
    };

    Camera _cam;
    GameObject _player;
    GameObject _hudCanvas;

    [SerializeField] ZoomState _zoomState;
    [SerializeField] Transform _target;
    [SerializeField] Vector3 _targetOffset;
    [SerializeField] float _minDistance;
    [SerializeField] float _smoothSpeed = 0.125f;

    public bool _zoomedInToggle = false;
    public bool _zoomingIn = false; 
    public bool _zoomingOut = false;
    private Vector3 _prePos;

    private void Awake()
    {
        _cam = Camera.main;
        _player = FindObjectOfType<PlayerMove>().gameObject;
        _hudCanvas = FindObjectOfType<GameHudMenu>().gameObject;
        _target = transform;
        _zoomedInToggle = false;
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

    //NPC 네임태그 클릭 시 NPC 줌인 
    public void ZoomInNPC()
    {
        Debug.Log("button working");

        //추후 플레이어 캐릭터 연동 시, 플레이어와 NPC와의 거리가
        //일정거리 이상 가까울 때만 버튼을 클릭할 수 있도록 할 것. 
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
            //_canvasUI.SetActive(false);

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
        //추후 플레이어 연동 시 플레이어 위치에 따라 
        //플레이어를 따라다니는 기존의 카메라 시점으로 변경할 것. 

        if (Vector3.Distance(_cam.transform.position, _prePos) <= 0.1f)
        {
            _zoomingOut = false;
            //_canvasUI.SetActive(false);

            //카메라 컨트롤러 켜기 
            //_player.GetComponent<CameraController>().enabled = true;
        }

        Vector3 smoothPos = Vector3.Lerp(_cam.transform.position, _prePos, _smoothSpeed);
        _cam.transform.position = smoothPos;
    }
}
