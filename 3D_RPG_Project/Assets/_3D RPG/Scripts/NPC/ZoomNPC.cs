using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] ZoomState zoomState;
    [SerializeField] Camera cam;
    [SerializeField] GameObject canvasUI;
    [SerializeField] Transform target;
    [SerializeField] Vector3 targetOffset;
    [SerializeField] float smoothSpeed = 0.125f;
    public bool zoomedInToggle = false;
    public bool zoomingIn = false; 
    public bool zoomingOut = false;
    private Vector3 prePos;

    private void Awake()
    {
        cam = Camera.main;
        target = transform.parent.transform;
        zoomedInToggle = false;
    }

    //private void FixedUpdate()
    //{
    //    if (zoomingIn)
    //    {
    //        ZoomedIn();
    //    }
        
    //    if(zoomingOut)
    //    {
    //        ZoomedOut();
    //    }
    //}

    //NPC 네임태그 클릭 시 NPC 줌인 
    public void ZoomInNPC()
    {

        //추후 플레이어 캐릭터 연동 시, 플레이어와 NPC와의 거리가
        //일정거리 이상 가까울 때만 버튼을 클릭할 수 있도록 할 것. 

        if (zoomedInToggle == false)
        {
            zoomedInToggle = true;
            zoomingIn = true;
            prePos = cam.transform.position;
        }
    }

    //X 아이콘 클릭 시 NPC 줌아웃 
    public void ZoomOutNPC()
    {
        if (zoomedInToggle == true)
        {
            zoomedInToggle = false;
            zoomingOut = true;
            //canvasUI.SetActive(false);
        }
    }

    public void ZoomedIn()
    {
        switch (zoomState)
        {
            case ZoomState.ZOOM_LEFT:
                break;

            case ZoomState.ZOOM_RIGHT:
                break;

            case ZoomState.ZOOM_CENTER:
                Vector3 desiredPos = target.position + targetOffset;

                if (Vector3.Distance(cam.transform.position, desiredPos) <= 0.1f)
                {
                    zoomingIn = false;
                    canvasUI.SetActive(true);
                }

                Vector3 smoothPos = Vector3.Lerp(cam.transform.position, desiredPos, smoothSpeed);
                cam.transform.position = smoothPos;

                cam.transform.LookAt(target);
                break;
        }
    }

    public void ZoomedOut()
    {
        //추후 플레이어 연동 시 플레이어 위치에 따라 
        //플레이어를 따라다니는 기존의 카메라 시점으로 변경할 것. 

        if (Vector3.Distance(cam.transform.position, prePos) <= 0.1f)
        {
            zoomingOut = false;
            canvasUI.SetActive(false);
        }

        Vector3 smoothPos = Vector3.Lerp(cam.transform.position, prePos, smoothSpeed);
        cam.transform.position = smoothPos;
    }
}
