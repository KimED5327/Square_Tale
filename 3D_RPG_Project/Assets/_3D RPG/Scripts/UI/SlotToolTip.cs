using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotToolTip : MonoBehaviour
{
    public static SlotToolTip instance;

    [Header("UI")]
    [SerializeField] GameObject goToolTip = null;
    [SerializeField] Text txtName = null;
    [SerializeField] Text txtType = null;
    [SerializeField] Text txtLimitLevel = null;
    [SerializeField] Text txtOption = null;
    [SerializeField] Text txtDesc = null;

    // 터치 반응 시간
    [Header("TouchReact")]
    [SerializeField] float touchReactTime = 1.0f;
    float curReactTime = 0f;
    bool isTouchSlot = false;
    int slotIndex;

    [Header("Offset")]
    [SerializeField] float offsetRightX = 210f;
    [SerializeField] float offsetLeftX = 455f;

    [SerializeField] float offsetY = -150f;
    [SerializeField] float mirrorLinePosX = 0f;

    Inventory theInven;
    void Awake()
    {
        theInven = GetComponent<Inventory>();
        instance = this;
    }
    // 터치 시작
    public void OnTouchDown(int index)
    {
        curReactTime = 0f;
        slotIndex = index;
        isTouchSlot = true;
        StartCoroutine(Touching());
    }

    // 터치 끝
    public void OnTouchUp()
    {
        isTouchSlot = false;
        StopAllCoroutines();
        HideToolTip();
    }

    // 터치중
    IEnumerator Touching()
    {
        while (isTouchSlot)
        {
            curReactTime += Time.deltaTime;
            if (curReactTime >= touchReactTime)
            {
                ShowToolTip();
                isTouchSlot = false;
            }

            yield return null;
        }

    }

    // 툴팁 출력
    void ShowToolTip()
    {
        
        Item item = theInven.GetSlotItem(slotIndex); // 터치한 슬롯 아이템 정보 얻어오기
        Vector3 pos = theInven.GetSlotLocalPos(slotIndex); // 터치한 슬롯 위치 정보 얻어오기

        // 툴팁 내용 세팅
        txtName.text = item.name;
        txtDesc.text = item.desc;
        txtType.text = "장비류 / 검 류"; // 임시 테스트용
        txtLimitLevel.text = item.levelLimit + "Lv";
        txtOption.text = "";
        if (item.options.Count > 0)
        {
            for(int i = 0; i < item.options.Count; i++)
                txtOption.text += item.options[i].name + " " + item.options[i].num + " ";
        }

        // 툴팁 위치 세팅
        pos.x += (pos.x >= mirrorLinePosX) ? offsetLeftX : offsetRightX;
        pos.y += offsetY;
        goToolTip.transform.localPosition = pos;


        goToolTip.SetActive(true);
    }

    void HideToolTip()
    {
        goToolTip.SetActive(false);
    }
}
