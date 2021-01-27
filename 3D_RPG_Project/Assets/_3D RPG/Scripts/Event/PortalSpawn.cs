using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalSpawn : MonoBehaviour
{
    [SerializeField] GameObject _goBlock = null;
    [SerializeField] GameObject _goPortal = null;

    // Update is called once per frame
    void Update()
    {
        _goPortal.SetActive(!_goBlock.activeSelf);
    }
}
