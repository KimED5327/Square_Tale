using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 퀘스트 매니져 
/// </summary>
public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    /// <summary>
    /// 현재 진행중인 퀘스트 리스트 
    /// </summary>
    List<Quest> _ongoingQuests = new List<Quest>();

    /// <summary>
    /// 완료된 퀘스트 리스트 
    /// </summary>
    List<Quest> _finishedQuests = new List<Quest>();

    private void Awake()
    {
        if (instance == null) instance = this; 
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 진행중인 퀘스트 리스트에 ongoingQuest를 원소로 추가하기 
    /// </summary>
    /// <param name="ongoingQuest"></param>
    public void AddOngoingQuest(Quest ongoingQuest) { _ongoingQuests.Add(ongoingQuest); }

    /// <summary>
    /// 완료된 퀘스트 리스트에 finishedQuest를 원소로 추가하기 
    /// </summary>
    /// <param name="finishedQuest"></param>
    public void AddFinishedQuest(Quest finishedQuest) { _finishedQuests.Add(finishedQuest); }

}
