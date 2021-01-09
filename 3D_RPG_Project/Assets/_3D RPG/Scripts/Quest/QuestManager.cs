using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 진행, 완료된 퀘스트를 관리하고 퀘스트 타입별로 완료 조건 검사를 수행하는 매니져 
/// </summary>
public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public delegate void EventHandler();
    public static event EventHandler CheckAvailableQuest;

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

    /// <summary>
    /// 진행중인 퀘스트 리스트에 ongoingQuest를 원소로 추가하기 
    /// </summary>
    /// <param name="ongoingQuest"></param>
    public void AddOngoingQuest(Quest ongoingQuest)
    {
        _ongoingQuests.Add(ongoingQuest);

        // 퀘스트 타입이 'NPC와의 대화'일 경우 대화 상대 NPC의 상태값 세팅  
        if (ongoingQuest.GetQuestType() == QuestType.TYPE_TALKWITHNPC) SetPartnerNpcStatus(ongoingQuest);
    }

    /// <summary>
    /// 퀘스트가 완료되어 진행중인 퀘스트 리스트에서 삭제하기 
    /// </summary>
    public void DeleteOngoingQuest()
    {
        _ongoingQuests.RemoveAt(0);
    }

    /// <summary>
    /// 완료된 퀘스트 리스트에 finishedQuest를 원소로 추가하기 
    /// </summary>
    /// <param name="finishedQuest"></param>
    public void AddFinishedQuest(Quest finishedQuest)
    {
        Debug.Log(finishedQuest.GetQuestID() + "번 퀘스트 완료");
        _finishedQuests.Add(finishedQuest);

        // 퀘스트 완료로 해금된 퀘스트가 있다면 오픈 
        OpenQuest(finishedQuest.GetQuestID());
    }

    /// <summary>
    /// 선행 퀘스트가 완료된 퀘스트의 진행상태를 미해금 상태에서 진행가능 상태로 변경 
    /// </summary>
    public void OpenQuest(int questID)
    {
        // 완료된 퀘스트의 ID(questID)가 선행 퀘스트 ID와 일치하는 퀘스트를 진행가능 상태로 변경 
        for (int i = 1; i < QuestDB.instance.GetMaxCount() + 1; i++)
        {
            if (QuestDB.instance.GetQuest(i).GetPrecedentID() != questID) continue;

            Debug.Log(QuestDB.instance.GetQuest(i).GetQuestID() + "번 퀘스트 해금");
            QuestDB.instance.GetQuest(i).SetState(QuestState.QUEST_OPENED);
            CheckAvailableQuest();
        }
    }

    /// <summary>
    /// type7. 'NPC와의 대화' 퀘스트에서 대화 상대가 되는 NPC의 상태값 세팅 
    /// </summary>
    public void SetPartnerNpcStatus(Quest quest)
    {
        TalkWithNpc talkWithNpc = quest.GetQuestInfo()[questInfoKey] as TalkWithNpc;
        talkWithNpc.GetQuestFinisher().SetQuestState(QuestState.QUEST_COMPLETABLE);
        talkWithNpc.GetQuestFinisher().SetQuestMark();
        talkWithNpc.GetQuestFinisher().SetOngoingQuestID(quest.GetQuestID());
    }
}
