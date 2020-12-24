using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NPC의 네임태그가 회전시에도 카메라를 향할 수 있도록 하는 클래스 
public class Billboard : MonoBehaviour
{
    [SerializeField] Transform cam;
    
    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
