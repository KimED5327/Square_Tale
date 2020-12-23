using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchBoard : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Vector2 touchDis; //거리
    public Vector2 pointerOld; //처음 터치한곳
    public int pointerId; //중복터치 방지
    public bool isPress; //눌렀냐?

    public void OnPointerDown(PointerEventData eventData)
    {
        isPress = true;
        pointerOld = eventData.position;
        pointerId = eventData.pointerId;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPress = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isPress)
        {
            if (pointerId >= 0 && pointerId < Input.touches.Length)
            {
                touchDis = Input.touches[pointerId].position - pointerOld;
                pointerOld = Input.touches[pointerId].position;
            }
            else
            {
                touchDis = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - pointerOld;
                pointerOld = Input.mousePosition;
            }
        }
        else
        {
            touchDis = new Vector2();
        }
    }
}
