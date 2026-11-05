using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    //シーン
    private Scene scene;

    //シーン番号
    private int bulidIndex;

    //シーンの名前
    private string sceneName;

    // インスタンスをキャッシュ
    private readonly WaitForSeconds waitSeconds = new(1.0f);

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        scene = SceneManager.GetActiveScene();

        bulidIndex = scene.buildIndex;

        sceneName = scene.name;
    }

    //リトライ
    public void Retry()
    {
        SoundManager.instance.PlaySE(0);
        StartCoroutine(SceneTransition(bulidIndex));

    }


    //ネクストステージ
    public void NextStage()
    {
        bulidIndex++;
        SoundManager.instance.PlaySE(0);
        StartCoroutine(SceneTransition(bulidIndex));

    }

    //ステージセレクト
    public void StageSelect()
    {
        PlayerPrefs.SetInt("Check", 0);
        SoundManager.instance.PlaySE(0);
        Time.timeScale = 1.0f;
        StartCoroutine(SceneTransition("StageSelect"));

    }

    //タイトル
    public void Title()
    {
        PlayerPrefs.SetInt("Check", 0);
        SoundManager.instance.PlaySE(0);
        Time.timeScale = 1.0f;
        StartCoroutine(SceneTransition("Title"));

    }

    //各ステージへの遷移処理
    public void StageSceneTransition(int sceneIndex)
    {
        SoundManager.instance.PlaySE(0);
        StartCoroutine(SceneTransition(sceneIndex));
    }


    //コルーチン
    private IEnumerator SceneTransition(string sceneName)
    {
        yield return waitSeconds;

        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator SceneTransition(int sceneIndex)
    {
        yield return waitSeconds;

        SceneManager.LoadScene(sceneIndex);
    }


}
