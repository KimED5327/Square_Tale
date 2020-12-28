using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using LitJson;
using UnityEngine.Networking;

// Json Data 관련 함수 
public class JsonManager : MonoBehaviour
{
    public static JsonManager instance;

    private void Awake()
    {
        if (instance == null) instance = this; 
    }

    // path의 위치에 있는 파일을 제이슨 데이터 형태로 리턴  
    public JsonData GetJsonData(string path)
    {
        string jsonString = "";

        // 안드로이드 
        if(Application.platform == RuntimePlatform.Android)
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

    // str이 NULL 혹은 빈 문자열일 경우 true 리턴 
    public bool IsNullString(string str)
    {
        return string.IsNullOrEmpty(str);
    }
}
