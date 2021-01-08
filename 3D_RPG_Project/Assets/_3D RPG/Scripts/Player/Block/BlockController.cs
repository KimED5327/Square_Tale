using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    [SerializeField] GameObject _goBlockPreview = null;

    [SerializeField] float _coolTime = 3f; float _curTime = 0f;

    [SerializeField] float _previewEffectHeight = -0.25f;
    [SerializeField] float _summonHeight = 0.5f;
    [SerializeField] float _swipeDistance = 5f;

    string _blockName;

    bool _canTouch = true;
    bool _isTouching = false;

    Vector2 _priorMousePos;
    Vector2 _lastMousePos;


    static readonly string _strPreviewEffectName = "블록 프리뷰 이펙트";
    const int LEFT = 0, UP = 1;

    BlockManager _blockManager;
    BlockPreview _preview;

    private void Awake()
    {
        _blockManager = FindObjectOfType<BlockManager>();
        _preview = _goBlockPreview.GetComponent<BlockPreview>();
    }

    public void BtnTouchDown()
    {
        if (_canTouch)
        {
            _isTouching = true;
            _priorMousePos = Input.mousePosition;
            ObjectPooling.instance.GetObjectFromPool(_strPreviewEffectName, _goBlockPreview.transform.position + Vector3.down * 0.25f); ;
            _goBlockPreview.SetActive(true);
        }
    }
    public void BtnTouchUp()
    {
        if (_canTouch && _isTouching)
        {
            _lastMousePos = Input.mousePosition;

            bool canSummonBlock = false;

            // 일정 거리 이상 스와이프를 한 경우
            if (Vector2.SqrMagnitude(_priorMousePos - _lastMousePos) > _swipeDistance)
            {
                Vector2 dir = (_priorMousePos - _lastMousePos).normalized;

                // 방향이 가로로 더 치우쳐진 경우
                if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
                {
                    // 왼쪽으로 스와이프 한 경우
                    if (_priorMousePos.x > _lastMousePos.x)
                        canSummonBlock = CanBlockSummon(LEFT);
                    // 오른쪽 스와이프
                    else
                        Notification.instance.ShowFloatingMessage(StringManager.msgWrongSwipeBlock);
                }
                // 방향이 세로로 더 치우쳐진 경우
                else
                {
                    // 위쪽으로 스와이프 한 경우
                    if (_priorMousePos.y < _lastMousePos.y)
                        canSummonBlock = CanBlockSummon(UP);
                    // 아래 방향 스와이프
                    else
                        Notification.instance.ShowFloatingMessage(StringManager.msgWrongSwipeBlock);
                }
            }
            else
                Debug.Log("너무 짧게 스와이프");
            

            if (canSummonBlock)
            {
                Vector3 pos = _goBlockPreview.transform.position + Vector3.up * _summonHeight;
                ObjectPooling.instance.GetObjectFromPool(_blockName, pos);
                ObjectPooling.instance.GetObjectFromPool(_strPreviewEffectName, _goBlockPreview.transform.position + Vector3.up * _previewEffectHeight); ;
            }

            Initialized();
        }
    }


    bool CanBlockSummon(int index)
    {
        if (_preview.IsExistObject())
        {
            Notification.instance.ShowFloatingMessage(StringManager.msgBlockExistObject);
            return false;
        }
        _blockName = _blockManager.GetBlockName(index);
        return true;
    }

    void Initialized()
    {
        _isTouching = false;
        _canTouch = false;
        _goBlockPreview.SetActive(false);
        _priorMousePos = Vector2.zero;
        _lastMousePos = Vector2.zero;
    }


    void Update()
    {
        if (!_canTouch)
        {
            _curTime += Time.deltaTime;
            if (_curTime >= _coolTime)
            {
                _curTime = 0f;
                _canTouch = true;
            }
        }
    }

}
