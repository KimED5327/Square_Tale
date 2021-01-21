using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using UnityEngine.Networking;

/// <summary>
/// NPC 관련 제이슨 데이터를 파싱하는 클래스 
/// </summary>
public class NPCLoader : MonoBehaviour
{
    public static NPCLoader instance; 
    static string streamingAssetsPath = Application.streamingAssetsPath;

    // NPC 정보 DB 경로 
    [SerializeField] string npcDBPath;

    private void Awake()
    {
        if (instance == null) instance = this; 
    }

    private void OnEnable()
    {
        // 이미 데이터 파싱이 이루어졌다면 리턴 
        if (NpcDB.instance.GetMaxCount() > 0) return; 

        ParsingNpcDB();
    }

    // NPC 데이터 파싱 함수 
    private void ParsingNpcDB()
    {
        string path = streamingAssetsPath + npcDBPath;
        JsonData jData = JsonManager.instance.GetJsonData(path);

        for (int i = 0; i < jData.Count; i++)
        {
            NpcWithLines npc = new NpcWithLines();

            int npcID = int.Parse(jData[i][0].ToString());
            npc.SetID(npcID);
            npc.SetName(jData[i][1].ToString());

            if (!JsonManager.instance.IsNullString(jData[i][2].ToString()))
            {
                // 단일 변수일 경우 
                if (!jData[i][2].IsArray)
                {
                    npc.AddQuestID(int.Parse(jData[i][2].ToString()));
                }
                else
                {
                    for (int j = 0; j < jData[i][2].Count; j++)
                    {
                        npc.AddQuestID(int.Parse(jData[i][2][j].ToString()));
                    }
                }
            }

            if (!jData[i][3].IsArray)
            {
                npc.AddLine(jData[i][3].ToString());
            }
            else
            {
                for (int j = 0; j < jData[i][3].Count; j++)
                {
                    npc.AddLine(jData[i][3][j].ToString());
                }
            }

            NpcDB.instance.AddNPC(npcID, npc);
        }
    }

    // 데이터 확인용 출력 함수 
    private void PrintNpcDB()
    {
        for (int i = 0; i < NpcDB.instance.GetMaxCount(); i++)
        {
            Debug.Log(NpcDB.instance.GetNPC(i + 1).GetID() + "번째 NPC( " +
                NpcDB.instance.GetNPC(i + 1).GetName() + " )");

            for (int j = 0; j < NpcDB.instance.GetNPC(i + 1).GetQuestList().Count; j++)
            {
                Debug.Log((j + 1) + "번째 퀘스트ID : " + NpcDB.instance.GetNPC(i + 1).GetQuestID(j));
            }

            for (int j = 0; j < NpcDB.instance.GetNPC(i + 1).GetLineList().Count; j++)
            {
                Debug.Log((j + 1) + "번째 대사 : " + NpcDB.instance.GetNPC(i + 1).GetLine(j));
            }
        }
    }
}
