using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public TouchBoard tb;
    float cameraAngleX;
    public float cameraSpeed = 0.2f;
    [SerializeField] Vector3 _offset = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        cameraAngleX += tb.touchDis.x * cameraSpeed;

        Camera.main.transform.position = transform.position + Quaternion.AngleAxis(cameraAngleX, Vector3.up) * _offset;

        Camera.main.transform.rotation = Quaternion.LookRotation(transform.position + Vector3.up * 1.2f - Camera.main.transform.position, Vector3.up);
    }
}
