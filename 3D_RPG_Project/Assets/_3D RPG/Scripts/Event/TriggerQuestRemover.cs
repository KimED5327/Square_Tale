using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerQuestRemover : MonoBehaviour
{
    [SerializeField] int _questId = 3;

    private void Start()
    {
        StartCoroutine(RemoveCheck());
    }

    IEnumerator RemoveCheck()
    {
        yield return new WaitUntil(()=>QuestManager.instance.SearchCompleteQuestID(_questId));

        gameObject.SetActive(false);
    }
}
