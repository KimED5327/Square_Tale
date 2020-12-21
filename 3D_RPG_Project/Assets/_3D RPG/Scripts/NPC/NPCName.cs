using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCName : MonoBehaviour
{
    [SerializeField] TextMeshPro txtName;
    [SerializeField] string strName;

    // Start is called before the first frame update
    void Start()
    {
        SetNPCName(strName);
    }

    void SetNPCName(string name)
    {
        txtName.SetText(name);
    }
}
