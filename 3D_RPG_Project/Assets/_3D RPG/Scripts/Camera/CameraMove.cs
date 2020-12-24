using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;
    Vector3 offset;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - target.position;
        speed = 15.0f;

    }

    // Update is called once per frame
    void LateUpdate()
    {

        transform.position = target.position + offset;
        //RoatateCamera();
        
    }

    void RoatateCamera()
    {
        if (Input.GetKey("q"))
        {
             transform.RotateAround(target.transform.position, Vector3.up, speed * Time.deltaTime);
        }
        if (Input.GetKey("e"))
        {
             transform.RotateAround(target.transform.position, Vector3.up, speed);
        }

        transform.LookAt(target.transform);
    }
}
