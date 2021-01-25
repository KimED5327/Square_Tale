using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchBoard : MonoBehaviour
{
    public Vector2 touchDis; //거리
    public Vector2 pointerOld; //처음 터치한곳
    int _fingerID = -1;
    float _halfScreenWidth = Screen.width / 2;

    public static bool isPress; //눌렀냐?

    [SerializeField] LayerMask _layer;

    // Update is called once per frame
    void Update()
    {
        if (Application.platform != RuntimePlatform.Android)
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

        else
        {
            if (!GameHudMenu.s_isMenuOpen)
            {

                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch t = Input.GetTouch(i);

                    switch (t.phase)
                    {
                        case TouchPhase.Began:

                            if (EventSystem.current.IsPointerOverGameObject() == true) return;

                            if (t.position.x > this._halfScreenWidth && _fingerID == -1)
                            {
                                isPress = true;
                                _fingerID = t.fingerId;
                                pointerOld = Input.mousePosition;
                            }
                            break;

                        case TouchPhase.Moved:
                            if (!EventSystem.current.IsPointerOverGameObject(i) && isPress)
                            {
                                if (t.fingerId == _fingerID)
                                {
                                    touchDis = (Vector2)Input.mousePosition - pointerOld;
                                    pointerOld = Input.mousePosition;
                                }
                            }
                            break;

                        case TouchPhase.Ended:

                            if (t.fingerId == _fingerID)
                            {
                                _fingerID = -1;
                                isPress = false;
                                touchDis = Vector2.zero;
                            }
                            break;

                        case TouchPhase.Canceled:
                            if (t.fingerId == this._fingerID)
                            {
                                _fingerID = -1;
                                isPress = false;
                                touchDis = Vector2.zero;
                            }
                            break;
                    }
                }
            }
        }

      
    }
}
