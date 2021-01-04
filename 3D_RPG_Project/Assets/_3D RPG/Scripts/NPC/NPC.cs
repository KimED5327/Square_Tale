using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC 
{
    int _id;             // NPC ID
    string _name;        // NPC 이름 

    //getter
    public int GetID() { return _id; }
    public string GetName() { return _name; }

    //setter
    public void SetID(int id) { _id = id; }
    public void SetName(string name) { _name = name; }
}
