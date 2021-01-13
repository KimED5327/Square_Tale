using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillLevel : MonoBehaviour
{
    [SerializeField] GameObject[] _isSwordSkill = null;
    [SerializeField] Text[] _txtLevel = null;

    PlayerStatus _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayerStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.GetLevel() >= 2)
        {
            _isSwordSkill[0].SetActive(false);
            _txtLevel[0].color = Color.black;
        }
        if (_player.GetLevel() >= 3)
        {
            _isSwordSkill[1].SetActive(false);
            _txtLevel[1].color = Color.black;
        }
        if (_player.GetLevel() >= 4)
        {
            _isSwordSkill[2].SetActive(false);
            _txtLevel[2].color = Color.black;
        }
        if (_player.GetLevel() >= 5)
        {
            _isSwordSkill[3].SetActive(false);
            _txtLevel[3].color = Color.black;
        }
    }
}
