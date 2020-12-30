using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;
using UnityEngine.Networking;

public class NPCLoader : MonoBehaviour
{
    static readonly string streamingAssetsPath = Application.streamingAssetsPath;

    // NPC 정보 DB 경로 
    [SerializeField] string npcDBPath;

    // Start is called before the first frame update
    void Start()
    {
        ParsingNpcDB();
        PrintNpcDB();
    }

    private void ParsingNpcDB()
    {
        string path = streamingAssetsPath + npcDBPath;
        JsonData jData = JsonManager.instance.GetJsonData(path);

        for (int i = 0; i < jData.Count; i++)
        {
            NPC npc = new NPC();

            int npcID = int.Parse(jData[i][0].ToString());
            npc.SetID(npcID);
            npc.SetName(jData[i][1].ToString());

            for (int j = 0; j < jData[i][2].Count; j++)
            {
                npc.AddLine(jData[i][2][j].ToString());
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

            for (int j = 0; j < NpcDB.instance.GetNPC(i + 1).GetLineList().Count; j++)
            {
                Debug.Log((j + 1) + "번째 대사 : " + NpcDB.instance.GetNPC(i + 1).GetLine(j));
            }
        }
    }
}
