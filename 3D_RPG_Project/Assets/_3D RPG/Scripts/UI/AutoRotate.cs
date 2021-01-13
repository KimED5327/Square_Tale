using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    [SerializeField] Vector3 _rotateOffset = Vector3.zero;
    [SerializeField] float _rotSpeed = 45f;


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(_rotateOffset * _rotSpeed * Time.deltaTime);
    }
}
