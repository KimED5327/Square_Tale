using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPreview : MonoBehaviour
{
    List<GameObject> _enterObjectList = new List<GameObject>();

    [SerializeField] Material _matRed = null;
    Material _matOrigin;
    Renderer _renderer;


    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _matOrigin = _renderer.material;
    }

    private void OnDisable()
    {
        _enterObjectList.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag(StringManager.TriggerTag))
            _enterObjectList.Add(other.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(StringManager.TriggerTag))
            _enterObjectList.Remove(other.gameObject);
    }

    private void Update()
    {
        _renderer.material = IsExistObject() ? _matRed : _matOrigin;
    }

    public bool IsExistObject() {

        return _enterObjectList.Count > 0;
    }
}
