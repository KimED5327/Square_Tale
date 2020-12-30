using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// NPC 데이터베이스 (int 타입 npcID를 key, NPC 클래스를 value로 Dictionary 컬렉션에서 데이터 관리)
/// </summary>
public class NpcDB : MonoBehaviour
{
    public static NpcDB instance;
    Dictionary<int, NPC> _npcDB = new Dictionary<int, NPC>();

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    /// <summary>
    /// npcID key, NPC value를 가진 데이터를 데이터베이스 Dictionary에 저장 
    /// </summary>
    /// <param name="npcID"></param>
    /// <param name="npc"></param>
    public void AddNPC(int npcID, NPC npc)
    {
        _npcDB.Add(npcID, npc);
    }

    /// <summary>
    /// npcID key값에 맞는 NPC 데이터를 반환 
    /// </summary>
    /// <param name="npcID"></param>
    /// <returns></returns>
    public NPC GetNPC(int npcID)
    {
        NPC npc = _npcDB[npcID];

        if (npc == null) Debug.Log("NPC ID " + npcID + "번은 등록되어있지 않습니다.");

        return npc;
    }

    /// <summary>
    /// NpcDB 내 전체 데이터의 개수를 반환 
    /// </summary>
    /// <returns></returns>
    public int GetMaxCount()
    {
        return _npcDB.Count;
    }
}
