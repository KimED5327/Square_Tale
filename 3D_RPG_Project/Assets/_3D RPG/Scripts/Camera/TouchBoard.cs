using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchBoard : MonoBehaviour
{
    public Vector2 touchDis; //거리
    public Vector2 pointerOld; //처음 터치한곳

    public bool isPress; //눌렀냐?

    // Update is called once per frame
    void Update()
    {
        if (!GameHudMenu.s_isMenuOpen)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // UI에 부딪친 경우 취소
                if (EventSystem.current.IsPointerOverGameObject() == true) return;

                isPress = true;
                pointerOld = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(0))
            {
                isPress = false;
            }
        }

        if (isPress)
        {
            touchDis = (Vector2)Input.mousePosition - pointerOld;
            pointerOld = Input.mousePosition;
        }
        else
        {
            touchDis = Vector2.zero;
        }
    }
}
