using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestActive : MonoBehaviour
{
    [SerializeField] int _questCompleteID = 0;
    [SerializeField] GameObject _goActive = null;


    // Start is called before the first frame update
    void Start()
    {
        if (QuestManager.instance.CheckIfQuestIsCompleted(_questCompleteID))
            _goActive.SetActive(true);
    }
}
