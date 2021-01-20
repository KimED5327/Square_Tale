using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureEffect : MonoBehaviour
{
    [SerializeField] Color _colorKeyword;
    [SerializeField] Color _colorGold;

    ParticleSystem _particle;


    // Start is called before the first frame update
    void Start()
    {
        _particle = GetComponent<ParticleSystem>();
    }


    public void SetColor(bool isKeyword)
    {

        if(_particle == null)
            _particle = GetComponent<ParticleSystem>();

        ParticleSystem.MainModule main = _particle.main;

        if (isKeyword)
            main.startColor = _colorKeyword;
        else
            main.startColor = _colorGold;
    }
}
