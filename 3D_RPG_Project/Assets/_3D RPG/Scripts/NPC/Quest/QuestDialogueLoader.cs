using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using LitJson;
using UnityEngine.Networking;

public class QuestDialogueLoader : MonoBehaviour
{
    static readonly string streamingAssetsPath = Application.streamingAssetsPath;

    [SerializeField] string questDialogueDBPath;

    // Start is called before the first frame update
    void Start()
    {
        // 퀘스트 다이얼로그 데이터 파싱 
        //ParsingQuestDialogueDB();

        //// 테스트용 
        //printDialogue();
    }

    // NPC 독백 버전으로 대사 덩어리로 파싱 
    //private void ParsingQuestDialogueDB()
    //{
    //    string path = streamingAssetsPath + questDialogueDBPath;
    //    JsonData jData = GetJsonData(path);

    //    for (int i = 0; i < jData.Count; i++)
    //    {
    //        int questId = int.Parse(jData[i][0].ToString());
    //        int npcId = int.Parse(jData[i][1].ToString());
    //        int lineId = int.Parse(jData[i][2].ToString());
    //        QuestState state = (QuestState)int.Parse(jData[i][3].ToString());

    //        DialogueUnit dialoguePerState = new DialogueUnit();
    //        List<string> lineLists = new List<string>();
    //        for (int j = 0; j < jData[i][4].Count; j++)
    //        {
    //            if (IsNullString(jData[i][4][j].ToString())) break;

    //            string line = jData[i][4][j].ToString();
    //            lineLists.Add(line);
    //        }

    //        dialoguePerState.SetLineId(lineId);
    //        dialoguePerState.SetQuestState(state);
    //        dialoguePerState.SetLineLists(lineLists);

    //        // 이미 해당 키값을 가진 퀘스트 다이얼로그 클래스가 있는 경우 
    //        // 퀘스트 진행상태에 맞는 인덱스에 세팅 
    //        if (QuestDialogueDB.instance.CheckKey(questId))
    //        {
    //            QuestDialogueDB.instance.GetDialogue(questId).
    //                SetDialoguePerState(dialoguePerState, state);

    //            Debug.Log(questId + "번 퀘스트 " + state + " 다이얼로그 추가");
    //        }
    //        //해당 키값을 가진 퀘스트 다이얼로그 클래스가 없는 경우 생성 
    //        else
    //        {
    //            QuestDialogue questDialogue = new QuestDialogue();

    //            questDialogue.SetQuestId(questId);
    //            questDialogue.SetNPCId(npcId);
    //            questDialogue.SetDialoguePerState(dialoguePerState, state);

    //            QuestDialogueDB.instance.AddDialogue(questId, questDialogue);
    //            Debug.Log(questId + "번 퀘스트 다이얼로그 클래스 생성");
    //            Debug.Log(questId + "번 퀘스트 " + state + " 다이얼로그 추가");
    //        }
    //    }
    //}

    // NPC 독백 버전으로 대사 1줄씩 파싱 
    //private void ParsingQuestDialogueDB()
    //{
    //    string path = streamingAssetsPath + questDialogueDBPath;
    //    JsonData jData = GetJsonData(path);

    //    for (int i = 0; i < jData.Count; i++)
    //    {
    //        int questId = int.Parse(jData[i][0].ToString());
    //        int npcId = int.Parse(jData[i][1].ToString());
    //        int lineId = int.Parse(jData[i][2].ToString());
    //        QuestState state = (QuestState)int.Parse(jData[i][3].ToString());
    //        string line = jData[i][4].ToString();

    //        // 퀘스트ID의 questDialogue 데이터가 있는지 확인하고 없으면 전체 생성 
    //        if (!QuestDialogueDB.instance.CheckKey(questId))
    //        {
    //            QuestDialogue questDialogue = new QuestDialogue();
    //            DialogueUnit dialoguePerState = new DialogueUnit();
    //            List<string> lineLists = new List<string>();

    //            questDialogue.SetQuestId(questId);
    //            questDialogue.SetNPCId(npcId);

    //            lineLists.Add(line);
    //            dialoguePerState.SetQuestState(state);
    //            dialoguePerState.SetLineLists(lineLists);
    //            questDialogue.SetDialoguePerState(dialoguePerState, state);

    //            QuestDialogueDB.instance.AddDialogue(questId, questDialogue);
    //            Debug.Log(questId + "번 퀘스트 다이얼로그 클래스 생성");
    //            Debug.Log(questId + "번 퀘스트 " + state + " 다이얼로그 추가");
    //            continue;
    //        }

    //        // 해당 state의 DialogueUnit이 만들어져있는지 확인 
    //        if (QuestDialogueDB.instance.GetDialogue(questId).GetDialoguePerState(state) != null)
    //        {
    //            QuestDialogueDB.instance.GetDialogue(questId).GetDialoguePerState(state).
    //                GetLineLists().Add(line);

    //            Debug.Log(questId + "번 퀘스트 대사 추가");
    //            Debug.Log(QuestDialogueDB.instance.GetDialogue(questId).GetLine(state, i));
    //        }
    //        else
    //        {
    //            DialogueUnit dialoguePerState = new DialogueUnit();
    //            List<string> lineLists = new List<string>();

    //            lineLists.Add(line);
    //            dialoguePerState.SetLineId(lineId);
    //            dialoguePerState.SetQuestState(state);
    //            dialoguePerState.SetLineLists(lineLists);

    //            QuestDialogueDB.instance.GetDialogue(questId).
    //                SetDialoguePerState(dialoguePerState, state);

    //            Debug.Log(questId + "번 퀘스트 " + state + " 다이얼로그 추가");
    //        }
    //    }
    //}

    // 

    private void ParsingQuestDialogueDB()
    {
        //string path = streamingAssetsPath + questDialogueDBPath;
        //JsonData jData = GetJsonData(path);

        //for (int i = 0; i < jData.Count; i++)
        //{
        //    int questId = int.Parse(jData[i][0].ToString());
        //    int lineId = int.Parse(jData[i][2].ToString());
        //    int npcId = int.Parse(jData[i][3].ToString());
        //    QuestState state = (QuestState)int.Parse(jData[i][1].ToString());

        //    LineUnit line = new LineUnit();
        //    line.SetNPCId(npcId);
        //    line.SetLineId(lineId);
        //    line.SetLine(jData[i][4].ToString());

        //    // 퀘스트ID의 questDialogue 데이터가 있는지 확인하고 없으면 전체 생성 
        //    if (!QuestDialogueDB.instance.CheckKey(questId))
        //    {
        //        QuestDialogue questDialogue = new QuestDialogue();
        //        DialogueUnit dialoguePerState = new DialogueUnit();
        //        List<LineUnit> lineLists = new List<LineUnit>();

        //        questDialogue.SetQuestId(questId);

        //        lineLists.Add(line);
        //        dialoguePerState.SetQuestState(state);
        //        dialoguePerState.SetLineLists(lineLists);
        //        questDialogue.SetDialoguePerState(dialoguePerState, state);

        //        QuestDialogueDB.instance.AddDialogue(questId, questDialogue);
        //        Debug.Log(questId + "번 퀘스트 다이얼로그 클래스 생성");
        //        Debug.Log(questId + "번 퀘스트 " + state + " 다이얼로그 추가");
        //        continue;
        //    }

        //    // 해당 state의 DialogueUnit이 만들어져있는지 확인 
        //    if (QuestDialogueDB.instance.GetDialogue(questId).GetDialoguePerState(state) != null)
        //    {
        //        QuestDialogueDB.instance.GetDialogue(questId).GetDialoguePerState(state).
        //            GetLineLists().Add(line);

        //        Debug.Log(questId + "번 퀘스트 대사 추가");
        //        //Debug.Log(QuestDialogueDB.instance.GetDialogue(questId).GetLine(state, i));
        //    }
        //    else
        //    {
        //        DialogueUnit dialoguePerState = new DialogueUnit();
        //        List<LineUnit> lineLists = new List<LineUnit>();

        //        lineLists.Add(line);
        //        dialoguePerState.SetQuestState(state);
        //        dialoguePerState.SetLineLists(lineLists);

        //        QuestDialogueDB.instance.GetDialogue(questId).
        //            SetDialoguePerState(dialoguePerState, state);

        //        Debug.Log(questId + "번 퀘스트 " + state + " 다이얼로그 추가");
        //    }
        //}
    }

    private void printDialogue()
    {
        for (int i = 0; i < QuestDialogueDB.instance.GetDialogue(1).
            GetLinesCount(QuestState.QUEST_OPENED); i++)
        {
            if(QuestDialogueDB.instance.GetDialogue(1).
                GetLineUnit(QuestState.QUEST_OPENED, i).GetNPCId() == 0)
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

    private JsonData GetJsonData(string path)
    {
        string jsonString = "";

        // 안드로이드 
        if (Application.platform == RuntimePlatform.Android)
        {
            UnityWebRequest reader = new UnityWebRequest(path);
            while (!reader.isDone)
                jsonString = reader.downloadHandler.text;
        }
        else // PC
        {
            jsonString = File.ReadAllText(path);
        }

        return JsonMapper.ToObject(jsonString);
    }

    // 넘겨받은 문자열이 NULL이나 빈 문자열인지 판별 
    private bool IsNullString(string str)
    {
        return string.IsNullOrEmpty(str);
    }
}
