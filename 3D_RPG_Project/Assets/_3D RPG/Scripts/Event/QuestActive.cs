using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestActive : MonoBehaviour
{
    [SerializeField] int _questCompleteID = 0;
    [SerializeField] int _deActiveQuestID = 0;

    [SerializeField] GameObject _goActive = null;


    // Start is called before the first frame update
    void Start()
    {
        if (_deActiveQuestID != 0 && QuestManager.instance.CheckIfQuestIsCompleted(_deActiveQuestID)) return;

        if (QuestManager.instance.CheckIfQuestIsCompleted(_questCompleteID))
            _goActive.SetActive(true);
    }
}
