using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffSlot : MonoBehaviour
{
    [SerializeField] Image _imgBuff = null;
    [SerializeField] Image _imgCooltime = null;
    [SerializeField] Image _background = null;

    int _buffID = 0;

    float _buffOrigintime = 0f;
    float _buffLeftTime = 0f;
    float _curTime = 0f;
    float _tickTime = 0f;
        
    bool _isApplying = false;
    bool _isTick = false;
    bool _isTickApply = false;

    public void PushBuffSlot(int id, float leftTime, Color color, bool isTick)
    {
        if (!_isApplying)
            _buffOrigintime = leftTime;

        _buffID = id;
        _buffLeftTime = leftTime;
        _curTime = 0f;

        _isTick = isTick;
        _isTickApply = false;
        _isApplying = true;

        _background.color = color;
        Sprite icon = SpriteManager.instance.GetBuffSprite(_buffID);
        _imgBuff.sprite = icon;

        gameObject.SetActive(true);
    }

    public void RemoveBuff()
    {
        _buffOrigintime = 0f;
        _tickTime = 0f;
        _isTickApply = false;
        _isApplying = false;
        _buffLeftTime = 0;
        _curTime = 0f;
    }



    // Update is called once per frame
    void Update()
    {
        if (_isApplying)
        {
            _curTime += Time.deltaTime;

            if (_isTick)
            {
                _tickTime += Time.deltaTime;
                if(_tickTime >= 1f)
                {
                    _isTickApply = true;
                }
            }

            if (_buffOrigintime <= _buffLeftTime)
                _imgCooltime.fillAmount = 1f;
            else
                _imgCooltime.fillAmount = _curTime / _buffLeftTime;

            if (_curTime >= _buffLeftTime)
            {
                RemoveBuff();
            }
        }
    }

    public int GetBuffID() { return _buffID; }
    public bool IsActive() { return _isApplying; }
    public void DeActive()
    {
        gameObject.SetActive(false);
    }

    public bool GetTickApply() { return _isTickApply; }
    public bool ResetTick() {
        _tickTime = 0f;
        return _isTickApply = false; 
    }
    
}
