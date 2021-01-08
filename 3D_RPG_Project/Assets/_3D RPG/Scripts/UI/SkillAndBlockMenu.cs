﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillAndBlockMenu : MonoBehaviour
{
    [SerializeField] GameObject _goUI = null;
    [SerializeField] GameObject _goSkillUI = null;
    [SerializeField] GameObject _goSkillUI2 = null;
    [SerializeField] GameObject _goBlockUI = null;
    [SerializeField] GameObject _goBlockUI2 = null;

    [SerializeField] Image _imgSkillBtn = null;
    [SerializeField] Image _imgBlockBtn = null;

    [SerializeField] Text _txtTitle = null;

    string _strSkill = "스킬";
    string _strBlock = "블록";

    BlockManager _blockManager;

    private void Awake()
    {
        _blockManager = FindObjectOfType<BlockManager>();
    }

    public void BtnMenuOpen()
    {
        _goUI.SetActive(true);
        BtnTab(0);
    }

    public void BtnMenuClose()
    {
        _goUI.SetActive(false);
    }

    public void BtnTab(int index)
    {
        if(index == 0)
        {
            _goSkillUI.SetActive(true);
            _goBlockUI.SetActive(false);
            _goSkillUI2.SetActive(true);
            _goBlockUI2.SetActive(false);
            _txtTitle.text = _strSkill;
            _imgSkillBtn.color = Color.white;
            _imgBlockBtn.color = Color.gray;

        }
        else
        {
            _goSkillUI.SetActive(false);
            _goBlockUI.SetActive(true);
            _goSkillUI2.SetActive(false);
            _goBlockUI2.SetActive(true);
            _txtTitle.text = _strBlock;

            _imgBlockBtn.color = Color.white;
            _imgSkillBtn.color = Color.gray;

            _blockManager.Setting();
        }
    }
}