using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropBlockManager : MonoBehaviour
{
    public static DropBlockManager instance;

    public List<GameObject> _dropTeam;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
