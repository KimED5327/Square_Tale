using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCooltime : MonoBehaviour
{

    [SerializeField] Image _imgSkill = null;
    [SerializeField] Image _imgCooltime = null;

    float _curTime = 0f;
    float _leftTime = 0f;

    bool _useSkill = false;

    public void SetSkillCooltime(float leftTime, bool useSkill)
    {
        _leftTime = leftTime;
        _useSkill = useSkill;
    }

    // Update is called once per frame
    void Update()
    {
        if (_useSkill)
        {
            _curTime += Time.deltaTime;
            _imgCooltime.fillAmount = _curTime / _leftTime;
            if (_curTime >= _leftTime)
            {
                _curTime = 0;
                _leftTime = 0;
                _useSkill = false;
                _imgCooltime.fillAmount = 0;
            }
        }
    }
}
