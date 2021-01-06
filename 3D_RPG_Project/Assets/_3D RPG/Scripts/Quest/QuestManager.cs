using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 진행, 완료된 퀘스트를 관리하고 퀘스트 타입별로 완료 조건 검사를 수행하는 매니져 
/// </summary>
public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    string questInfoKey = "info";

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

    /// <summary>
    /// 진행중인 퀘스트 리스트에 ongoingQuest를 원소로 추가하기 
    /// </summary>
    /// <param name="ongoingQuest"></param>
    public void AddOngoingQuest(Quest ongoingQuest)
    {
        _ongoingQuests.Add(ongoingQuest);

        // 퀘스트 타입이 'NPC와의 대화'일 경우 대화 상대 NPC의 상태값 세팅  
        if (ongoingQuest.GetQuestType() == QuestType.TYPE_TALKWITHNPC) SetPartnerNpcStatus();
    }

    /// <summary>
    /// 완료된 퀘스트 리스트에 finishedQuest를 원소로 추가하기 
    /// </summary>
    /// <param name="finishedQuest"></param>
    public void AddFinishedQuest(Quest finishedQuest) { _finishedQuests.Add(finishedQuest); }

    /// <summary>
    /// type7. 'NPC와의 대화' 퀘스트에서 대화 상대가 되는 NPC의 상태값 세팅 
    /// </summary>
    public void SetPartnerNpcStatus()
    {

    }

    /// <summary>
    /// type7. 특정 NPC와 대화하여 완수하는 타입의 퀘스트 완료 조건 검사 
    /// </summary>
    public void CheckTalkWithNpc(int npcID)
    {
        // 대화 상대의 tag가 QuestNPC일 때만 검사 

        // 현재 진행중인 퀘스트 리스트 카운트만큼 검사 
        for (int i = 0; i < _ongoingQuests.Count; i++)
        {
            // 해당 퀘스트 타입이 아닐 경우 건너뛰기 
            if (_ongoingQuests[i].GetQuestType() != QuestType.TYPE_TALKWITHNPC) continue;

            TalkWithNpc questInfo = _ongoingQuests[1].GetQuestInfo()[questInfoKey] as TalkWithNpc;

            // 대화상대 NPC ID와 일치하지 않는 경우 건너뛰기  
            if (npcID != questInfo.GetNpcID()) continue;

            // 대화상대 NPC ID와 일치하는 경우 

        }
    }

}
