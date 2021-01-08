using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private RectTransform joystickBack;
    [SerializeField] private RectTransform joystick;

    private float radius;
    public bool isTouch = false;

    public Vector3 moveVec;

    // Start is called before the first frame update
    void Start()
    {
        radius = joystickBack.rect.width * 0.5f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 value = eventData.position - (Vector2)joystickBack.position;

        value = Vector2.ClampMagnitude(value, radius);

        joystick.localPosition = value;

        value = value.normalized;

        moveVec = new Vector3(value.x, 0f, value.y);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isTouch = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isTouch = false;
        joystick.localPosition = Vector3.zero;
        moveVec = Vector3.zero;
    }
}
