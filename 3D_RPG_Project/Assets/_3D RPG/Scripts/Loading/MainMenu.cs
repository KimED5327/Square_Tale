using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickMainGame()
    {
        LoadingScene.LoadScene("Scene2");
    }

    public void OnClickLoad()
    {
        LoadingScene.LoadScene("Scene2");
    }

    public void OnClickOption()
    {
        LoadingScene.LoadScene("Scene2");
    }

    public void OnClickQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}