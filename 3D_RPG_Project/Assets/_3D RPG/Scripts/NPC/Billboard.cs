using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NPC의 네임태그가 회전시에도 카메라를 향할 수 있도록 하는 클래스 
public class Billboard : MonoBehaviour
{
    Transform cam;

    void OnEnable()
    {
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(cam != null)
            transform.LookAt(transform.position + cam.forward);
    }
}
