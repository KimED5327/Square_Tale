using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public TouchBoard tb;
    float cameraAngleX;
    public float cameraSpeed = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cameraAngleX += tb.touchDis.x * cameraSpeed;

        Camera.main.transform.position = transform.position + Quaternion.AngleAxis(cameraAngleX, Vector3.up) * new Vector3(0f, 3.5f, -5f);

        Camera.main.transform.rotation = Quaternion.LookRotation(transform.position + Vector3.up * 1.2f - Camera.main.transform.position, Vector3.up);
    }
}
