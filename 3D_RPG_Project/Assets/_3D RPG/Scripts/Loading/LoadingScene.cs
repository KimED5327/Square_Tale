using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    [SerializeField]
    Image progressBar = null;
    [SerializeField] Image _imgScreen = null;
    [SerializeField] float _fadeSpeed = 1f;


    static string nextScene;

    bool _isFinished = true;

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    void Start()
    {
        _isFinished = false;
        StartCoroutine(FadeInCo());

        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator FadeInCo()
    {
        Color color = _imgScreen.color;
        while (_imgScreen.color.a > 0)
        {
            color.a -= Time.deltaTime * _fadeSpeed;
            _imgScreen.color = color;
            yield return null;
        }
        _isFinished = true;
    }

    IEnumerator LoadSceneProcess()
    {
        yield return new WaitUntil(() => _isFinished);

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0f;
        while(!op.isDone)
        {
            yield return null;

            if(op.progress < 0.9f)
            {
                progressBar.fillAmount = op.progress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
                if(progressBar.fillAmount>=1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}
