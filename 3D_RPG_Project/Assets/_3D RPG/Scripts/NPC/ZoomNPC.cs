using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//NPC 네임태그를 클릭하면 줌인
//X 아이콘을 클릭하면 줌아웃 
public class ZoomNPC : MonoBehaviour
{
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
    Vector3 _preRot;                // 줌인 시 카메라 이동 전 각도값 

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

    //NPC 클릭 시 NPC 줌인 
    public void ZoomInNPC()
    {
        // 플레이어와 NPC의 거리가 일정 거리 이하일 때만 줌인 발동 
        if (Vector3.Distance(_player.transform.position, _target.position) > _minDistance) return; 

        if (_zoomedInToggle == false)
        {
            _zoomedInToggle = true;
            _zoomingIn = true;

            // 줌아웃 시 되돌아갈 이전 카메라 위치 
            _prePos = _cam.transform.position;
            _preRot = _cam.transform.rotation.eulerAngles;

            // 카메라 컨트롤러 끄기 
            _player.GetComponent<CameraController>().enabled = false;

            // 카메라 Light 키기 
            _cam.GetComponent<Light>().enabled = true;

            // 플레이어 비활성화 
            _player.gameObject.SetActive(false);

            // HUD 캔버스 비활성화 
            _hudCanvas.SetActive(false);
        }
    }

    //X 아이콘 클릭 시 NPC 줌아웃 
    public void ZoomOutNPC()
    {
        if (_zoomedInToggle == true)
        {
            _zoomedInToggle = false;
            _zoomingOut = true;

            // 카메라 Light 끄기 
            _cam.GetComponent<Light>().enabled = false;


            // HUD 캔버스 활성화 
            _hudCanvas.SetActive(true);
        }
    }

    public void ZoomedIn()
    {
        Vector3 desiredPos = _target.position + _target.forward;
        desiredPos = new Vector3(desiredPos.x, desiredPos.y + 0.5f, desiredPos.z);

        Vector3 desiredRot = Quaternion.LookRotation(_target.position - desiredPos, Vector3.up).eulerAngles;
        desiredRot = new Vector3(10f, desiredRot.y, 0f);

        // 목표지점까지 이동하였다면 줌인 중지 
        if (Vector3.Distance(_cam.transform.position, desiredPos) <= 0.1f)
        {
            _zoomingIn = false;
            transform.GetComponent<QuestNPC>().ClickNPC();
        }

        _cam.transform.position = Vector3.Lerp(_cam.transform.position, desiredPos, _smoothSpeed);
        _cam.transform.rotation = Quaternion.Slerp(_cam.transform.rotation, Quaternion.Euler(desiredRot), _smoothSpeed);
    }

    public void ZoomedOut()
    {
        // 목표지점까지 이동하였다면 줌아웃 중지 
        if (Vector3.Distance(_cam.transform.position, _prePos) <= 0.1f)
        {
            _zoomingOut = false;

            // 플레이어 활성화 
            _player.gameObject.SetActive(true);

            //카메라 컨트롤러 켜기 
            _player.GetComponent<CameraController>().enabled = true;
        }

        // 줌인 이전의 카메라 위치로 이동 
        _cam.transform.position = Vector3.Lerp(_cam.transform.position, _prePos, _smoothSpeed);
        _cam.transform.rotation = Quaternion.Slerp(_cam.transform.rotation, Quaternion.Euler(_preRot), _smoothSpeed);
    }
}
