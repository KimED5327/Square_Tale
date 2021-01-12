using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    [SerializeField] Text _txtDamage = null;

    public void SetText(int num, bool isPlayerHurt)
    {
        _txtDamage.text = num.ToString();
        _txtDamage.color = (isPlayerHurt) ? Color.red : Color.white;
    }

}
