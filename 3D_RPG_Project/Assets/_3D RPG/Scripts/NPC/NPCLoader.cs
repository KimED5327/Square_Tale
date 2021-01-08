using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using UnityEngine.Networking;

public class NPCLoader : MonoBehaviour
{
    public static NPCLoader instance; 
    static string streamingAssetsPath = Application.streamingAssetsPath;
    bool _isParsingDone = false; 

    // NPC 정보 DB 경로 
    [SerializeField] string npcDBPath;

    private void Awake()
    {
        if (instance == null) instance = this; 
    }

    void Start()
    {
        ParsingNpcDB();
        //PrintNpcDB();

        _isParsingDone = true; 
    }

    private void ParsingNpcDB()
    {


        string path = streamingAssetsPath + npcDBPath;

        WWW androidPath = new WWW(path);

        JsonData jData = JsonMapper.ToObject(File.ReadAllText(androidPath.text));

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

            for (int j = 0; j < jData[i][3].Count; j++)
            {
                npc.AddLine(jData[i][3][j].ToString());
            }

            NpcDB.instance.AddNPC(npcID, npc);
        }
    }

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

    public bool ParsingCompleted() { return _isParsingDone; }

}
