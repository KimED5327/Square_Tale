using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCTag : MonoBehaviour
{
    [SerializeField] TextMeshPro txtName;
    [SerializeField] string strName;

    // Start is called before the first frame update
    void Start()
    {
        SetNPCName(strName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetNPCName(string name)
    {
        txtName.SetText(name);
    }
}
