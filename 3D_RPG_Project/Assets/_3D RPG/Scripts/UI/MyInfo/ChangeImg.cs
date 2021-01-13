using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeImg : MonoBehaviour
{
    [SerializeField] Sprite[] _profile = null;

    private Image _curImg;

    PlayerMove _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayerMove>();
        _curImg = GetComponent<Image>();
    }

    private void Update()
    {
        if (_player.GetIsSword())
        {
            _curImg.sprite = _profile[0];
        }
        if (_player.GetIsMage())
        {
            _curImg.sprite = _profile[1];
        }
    }
}
