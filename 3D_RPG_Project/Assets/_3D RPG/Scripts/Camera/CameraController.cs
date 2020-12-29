using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public TouchBoard tb;
    float cameraAngleX;
    public float cameraSpeed = 0.2f;
    [SerializeField] Vector3 _offset = Vector3.zero;
    [SerializeField] float _height = 0f;

    Transform _tfCam;

    void Start()
    {
        _tfCam = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        cameraAngleX += tb.touchDis.x * cameraSpeed;

        _tfCam.position = transform.position + Quaternion.AngleAxis(cameraAngleX, Vector3.up) * _offset;
        _tfCam.rotation = Quaternion.LookRotation(transform.position + Vector3.up * _height - _tfCam.position, Vector3.up);
    }
}
