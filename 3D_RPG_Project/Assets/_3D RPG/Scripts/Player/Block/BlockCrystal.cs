using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCrystal : Block
{
    const int NORMAL = 0, DESTROY = 1, ACQUIRE_CRYSTAL = 2;

    [SerializeField] int _crystalID = 0;
    [SerializeField] CrystalCheck _crystal = null;

    protected override void BlockEffect()
    {
        PlayerPrefs.SetInt(StringManager.GetCrystalKey(_crystalID), DESTROY);
        CreateCrystalEffect();
    }


    public void SetAcquireCrystal()
    {
        PlayerPrefs.SetInt(StringManager.GetCrystalKey(_crystalID), ACQUIRE_CRYSTAL);
    }

    public override void Initialized()
    {
        _anim = GetComponent<Animator>();

        if (!PlayerPrefs.HasKey(StringManager.GetCrystalKey(_crystalID)))
        {
            PlayerPrefs.SetInt(StringManager.GetCrystalKey(_crystalID), NORMAL);
            _curHp = _maxHp;
        }
        else
        {
            int status = PlayerPrefs.GetInt(StringManager.GetCrystalKey(_crystalID));

            if (status == NORMAL)
                _curHp = _maxHp;

            else if(status == DESTROY)
            {
                CreateCrystalEffect();
            }

            else if(status == ACQUIRE_CRYSTAL)
                gameObject.SetActive(false);
            
        }
    }

    void CreateCrystalEffect()
    {


        Renderer[] rend = GetComponentsInChildren<Renderer>();
        foreach(Renderer r in rend)
        {
            r.gameObject.SetActive(false);
        }

        _crystal.gameObject.SetActive(true);

        GetComponent<BoxCollider>().enabled = false;
    }
}
