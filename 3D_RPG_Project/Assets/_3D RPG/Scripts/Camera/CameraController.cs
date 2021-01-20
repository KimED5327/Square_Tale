using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    TouchBoard tb;
    static float cameraAngleX;
    public float cameraSpeed = 0.2f;
    [SerializeField] Vector3 _offset = Vector3.zero;
    [SerializeField] float _height = 0f;

    Transform _tfCam;

    bool _canMove = true;

    void Awake()
    {
        cameraAngleX = 90;
        tb = FindObjectOfType<TouchBoard>();
        _tfCam = Camera.main.transform;
    }

    public void SetCamRot(float angleY)
    {
        cameraAngleX = angleY;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_canMove) return;
        
        cameraAngleX += tb.touchDis.x * cameraSpeed;

        _tfCam.position = transform.position + Quaternion.AngleAxis(cameraAngleX, Vector3.up) * _offset;
        _tfCam.rotation = Quaternion.LookRotation(transform.position + Vector3.up * _height - _tfCam.position, Vector3.up);
    }

    public void SetCanMove(bool flag) { _canMove = flag; }
}
