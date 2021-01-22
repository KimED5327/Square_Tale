using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using LitJson;
using UnityEngine.Networking;

/// <summary>
/// 퀘스트 다이얼로그 제이슨 데이터를 파싱하는 클래스 
/// </summary>
public class QuestDialogueLoader : MonoBehaviour
{
    static string streamingAssetsPath = Application.streamingAssetsPath;
    [SerializeField] string questDialogueDBPath;

    // Start is called before the first frame update
    void Start()
    {
        // 이미 데이터 파싱이 이루어졌다면 리턴 
        if (QuestDialogueDB.instance.GetMaxCount() > 0) return;

        // 퀘스트 다이얼로그 데이터 파싱 
        ParsingQuestDialogueDB();

        // 테스트용 출력 
        //PrintDialogue();
    }

    // 퀘스트 다이얼로그 데이터 파싱 함수 
    private void ParsingQuestDialogueDB()
    {
        string path = streamingAssetsPath + questDialogueDBPath;
        JsonData jData = JsonManager.instance.GetJsonData(path);

        for (int i = 0; i < jData.Count; i++)
        {
            int questID = int.Parse(jData[i][0].ToString());
            QuestState state = (QuestState)int.Parse(jData[i][1].ToString());

            LineUnit line = new LineUnit();
            line.SetLineID(int.Parse(jData[i][2].ToString()));
            line.SetNpcID(int.Parse(jData[i][3].ToString()));
            line.SetLine(jData[i][4].ToString());

            // 퀘스트 ID의 questDialogue 데이터가 있는지 확인하고 없을 경우 생성하고 for문 건너뛰기 
            if(!QuestDialogueDB.instance.CheckKey(questID))
            {
                QuestDialogue questDialogue = new QuestDialogue();
                DialogueUnit dialoguePerState = new DialogueUnit();
                List<LineUnit> lineList = new List<LineUnit>();

                questDialogue.SetQuestID(questID);

                dialoguePerState.SetQuestState(state);
                lineList.Add(line);
                dialoguePerState.SetLineList(lineList);
                questDialogue.SetDialoguePerState(state, dialoguePerState);

                QuestDialogueDB.instance.AddDialogue(questID, questDialogue);
                //Debug.Log(questID + "번 퀘스트 다이얼로그 클래스 생성");
                //Debug.Log(questID + "번 퀘스트 " + state + " 다이얼로그 추가");
                continue; 
            }

            // 해당 퀘스트 ID의 questDialogue 데이터가 있는 경우 
            // 해당 state의 DialogueUnit 데이터가 있는 경우 line만 추가 
            if(QuestDialogueDB.instance.GetDialogue(questID).CheckDialogue(state))
            {
                QuestDialogueDB.instance.GetDialogue(questID).AddLine(state, line);

                //Debug.Log(questID + "번 퀘스트 " + state + "대사 추가");
            }
            else
            {
                // 해당 state의 DialogueUnit 데이터가 없는 경우 lineList와 함께 생성 
                DialogueUnit dialoguePerState = new DialogueUnit();
                List<LineUnit> lineList = new List<LineUnit>();

                dialoguePerState.SetQuestState(state);
                lineList.Add(line);
                dialoguePerState.SetLineList(lineList);
                QuestDialogueDB.instance.GetDialogue(questID).SetDialoguePerState(state, dialoguePerState);
            }
        }
    }

    // 데이터 확인용 출력 함수 
    private void PrintDialogue()
    {
        for (int i = 0; i < QuestDialogueDB.instance.GetDialogue(1).
            GetLinesCount(QuestState.QUEST_OPENED); i++)
        {
            if(QuestDialogueDB.instance.GetDialogue(1).
                GetLineUnit(QuestState.QUEST_OPENED, i).GetNpcID() == 0)
            {
                Debug.Log("유저 : " + QuestDialogueDB.instance.GetDialogue(1).GetLine(QuestState.QUEST_OPENED, i));
            }
            else
            {
                Debug.Log("NPC : " + QuestDialogueDB.instance.GetDialogue(1).GetLine(QuestState.QUEST_OPENED, i));
            }
        }

        //for (int i = 0; i < QuestDialogueDB.instance.GetDialogue(1).
        //     GetLinesCount(QuestState.QUEST_ONGOING); i++)
        //{

        //}

        //for (int i = 0; i < QuestDialogueDB.instance.GetDialogue(1).
        //    GetLinesCount(QuestState.QUEST_COMPLETABLE); i++)
        //{

        //}
    }
}
