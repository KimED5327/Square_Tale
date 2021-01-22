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

    /// <summary>
    /// path의 경로에 있는 파일을 JsonData 타입으로 리턴 
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public JsonData GetJsonData(string path)
    {
        string jsonString = "";
        if (Application.platform == RuntimePlatform.Android)
        {
            WWW reader = new WWW(path);
            while (!reader.isDone)
            {

            }
            jsonString = reader.text;
        }
        else
        {
            jsonString = File.ReadAllText(path);
        }

        return JsonMapper.ToObject(jsonString);
    }

    /// <summary>
    /// 파라미터 str이 NULL 혹은 빈 문자열일 경우 true 리턴 
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public bool IsNullString(string str)
    {
        return string.IsNullOrEmpty(str);
    }
}
