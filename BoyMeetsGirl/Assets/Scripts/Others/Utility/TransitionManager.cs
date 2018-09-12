using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TransitionManager : SingletonMonoBehaviour<TransitionManager>
{
    [SerializeField] private GameObject m_fadePlane = null;
    public int stageNo { get; set; }
    public int score { get; set; }
    public bool isSalvated { get; private set; }
    public bool isTransited { get; private set; }
#if false
    //fadeTime : フェードアウトまでの時間
    //afterFadeTime:アルファ値が1になってからの時間
    public IEnumerator fadeOut(float fadeTime,float afterFadeTime)
    {
        for (float t = 0f; t < fadeTime; t += Time.deltaTime)
        {
            Color c = m_fadePlane.GetComponent<Image>().color;
            c.a = t / fadeTime;
            m_fadePlane.GetComponent<Image>().color = c;
            yield return null;
        }
        yield return new WaitForSeconds(afterFadeTime);
    }

    public IEnumerator fadeIn(float fadeTime,float afterFadeTime)
    {
        for(float t = fadeTime; t > 0f; t -= Time.deltaTime)
        {
            Color c = m_fadePlane.GetComponent<Image>().color;
            c.a = t / fadeTime;
            m_fadePlane.GetComponent<Image>().color = c;
            yield return null;
        }
        yield return new WaitForSeconds(afterFadeTime);
    }

#endif

    private IEnumerator salvation(float fadeOutTime, float fadeInTime)
    {
        float time = 0;
        isSalvated = true;


        while (true)
        {
            Color color = m_fadePlane.GetComponent<Image>().color;
            color.a = time / fadeOutTime;
            m_fadePlane.GetComponent<Image>().color = color;
            time += Time.deltaTime;

            if (color.a > 1f)
            {
                color.a = 1f;
                break;
            }
            yield return null;
        }




        //問答無用
        Fermion f = FindObjectOfType<Fermion>();
        Boson b = FindObjectOfType<Boson>();
        if (f != null)
        {
            f.transform.position = StageManager.Instance.startPos[stageNo];
            //  f.enabled = true;
        }
        if (b != null)
        {
            b.transform.position = StageManager.Instance.startPos[stageNo];
            // b.enabled = true;
        }

        Color c = m_fadePlane.GetComponent<Image>().color;
        c.a = 0f;
        m_fadePlane.GetComponent<Image>().color = c;
        //だんだん明るく

        time = 0;
        while (true)
        {
            Color color = m_fadePlane.GetComponent<Image>().color;
            color.a = 1f - time / fadeInTime;
            m_fadePlane.GetComponent<Image>().color = color;
            time += Time.deltaTime;

            if (color.a < 0f)
            {
                color.a = 0f;
                break;
            }
            yield return null;
        }


        isSalvated = false;
    }

    public void salvate(float fadeOutTime, float fadeInTime)
    {
        StartCoroutine(salvation(fadeOutTime, fadeInTime));
    }
    public void transite(string sceneName, float fadeOutTime, float fadeInTime)
    {
        StartCoroutine(transition(sceneName, fadeOutTime, fadeInTime));
    }
    private IEnumerator transition(string sceneName, float fadeOutTime, float fadeInTime = 0.1f)
    {
        float time = 0;
        isTransited = true;
        while (true)
        {
            Color color = m_fadePlane.GetComponent<Image>().color;
            color.a = time / fadeOutTime;
            m_fadePlane.GetComponent<Image>().color = color;
            time += Time.deltaTime;

            if (color.a > 1f)
            {
                break;
            }
            yield return null;
        }


        //シーン切替
        SceneManager.LoadScene(sceneName);
        //だんだん明るく
        time = 0;
        while (true)
        {
            Color color = m_fadePlane.GetComponent<Image>().color;
            color.a = 1f - time / fadeOutTime;
            m_fadePlane.GetComponent<Image>().color = color;
            time += Time.deltaTime;

            if (color.a < 0f)
            {
                break;
            }
            yield return null;
        }

        isTransited = false;
    }


}

