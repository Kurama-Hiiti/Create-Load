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
        StartCoroutine("RetryNextWait");
        //SceneManager.LoadScene(bulidIndex);
    }


    //ネクストステージ
    public void NextStage()
    {
        bulidIndex = bulidIndex + 1;
        SoundManager.instance.PlaySE(0);
        StartCoroutine("RetryNextWait");
        //SceneManager.LoadScene(bulidIndex);
    }

    //ステージセレクト
    public void StageSelect()
    {
        PlayerPrefs.SetInt("Check", 0);
        SoundManager.instance.PlaySE(0);
        Time.timeScale = 1.0f;
        StartCoroutine("SelectWait");
        //SceneManager.LoadScene("StageSelect");
    }

    //タイトル
    public void Title()
    {
        PlayerPrefs.SetInt("Check", 0);
        SoundManager.instance.PlaySE(0);
        Time.timeScale = 1.0f;
        StartCoroutine("TitleWait");
        //SceneManager.LoadScene("Title");
    }

    public void Stage1()
    {
        SoundManager.instance.PlaySE(0);
        StartCoroutine("Stage1Wait");
        //SceneManager.LoadScene("GameMain1");
    }
    public void Stage2()
    {
        SoundManager.instance.PlaySE(0);
        StartCoroutine("Stage2Wait");
        //SceneManager.LoadScene("GameMain2");
    }
    public void Stage3()
    {
        SoundManager.instance.PlaySE(0);
        StartCoroutine("Stage3Wait");
        //SceneManager.LoadScene("GameMain3");
    }
    public void Stage4()
    {
        SoundManager.instance.PlaySE(0);
        StartCoroutine("Stage4Wait");
        //SceneManager.LoadScene("GameMain4");
    }
    public void Stage5()
    {
        SoundManager.instance.PlaySE(0);
        StartCoroutine("Stage5Wait");
        //SceneManager.LoadScene("GameMain5");
    }
    public void Stage6()
    {
        SoundManager.instance.PlaySE(0);
        StartCoroutine("Stage6Wait");
        //SceneManager.LoadScene("GameMain6");
    }
    public void Stage7()
    {
        SoundManager.instance.PlaySE(0);
        StartCoroutine("Stage7Wait");
        //SceneManager.LoadScene("GameMain7");
    }
    public void Stage8()
    {
        SoundManager.instance.PlaySE(0);
        StartCoroutine("Stage8Wait");
        //SceneManager.LoadScene("GameMain8");
    }
    public void Stage9()
    {
        SoundManager.instance.PlaySE(0);
        StartCoroutine("Stage9Wait");
        //SceneManager.LoadScene("GameMain9");
    }


    //コルーチン

    private IEnumerator RetryNextWait()
    {
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(bulidIndex);
    }


    private IEnumerator SelectWait()
    {
        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("StageSelect");
    }


    private IEnumerator TitleWait()
    {
        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("Title");
    }


    private IEnumerator Stage1Wait()
    {
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene("GameMain1");
    }


    private IEnumerator Stage2Wait()
    {
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene("GameMain2");
    }

    private IEnumerator Stage3Wait()
    {
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene("GameMain3");
    }

    private IEnumerator Stage4Wait()
    {
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene("GameMain4");
    }

    private IEnumerator Stage5Wait()
    {
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene("GameMain5");
    }

    private IEnumerator Stage6Wait()
    {
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene("GameMain6");
    }

    private IEnumerator Stage7Wait()
    {
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene("GameMain7");
    }

    private IEnumerator Stage8Wait()
    {
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene("GameMain8");
    }

    private IEnumerator Stage9Wait()
    {
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene("GameMain9");
    }

}
