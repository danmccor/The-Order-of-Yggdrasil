using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelControl : MonoBehaviour
{
    GameObject Canvas;
    public Image fade;
    bool fading = false;
    private Color Alpha1;
    private Color Alpha0;
    private float delay = 3.0f;
    bool loadingLevel = false;
    AsyncOperation asyncLoad;
    // Start is called before the first frame update
    void Start()
    {
        GameObject.DontDestroyOnLoad(this);
        Canvas = GameObject.Find("Canvas");
        fade = GameObject.Find("Fade").GetComponent<Image>();
        Alpha1 = new Color(fade.color.r, fade.color.g, fade.color.b, 1.0f);
        Alpha0 = new Color(fade.color.r, fade.color.g, fade.color.b, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        fade = GameObject.Find("Fade").GetComponent<Image>();
    }

    public void loadLevel(int level)
    {
        fading = true;
        StartCoroutine(FadeOut(level));
    }

    public void asyncLoadLevel(int level)
    {
        asyncLoad = SceneManager.LoadSceneAsync(level);
        asyncLoad.allowSceneActivation = false;
        StartCoroutine(LoadYourAsyncScene());
    }

    public void allowAysncLoad()
    {
        loadingLevel = true;
    }
    IEnumerator FadeOut(int level)
    {
        while ((fade.color.a < 0.99f) && (fading == true))
        {
            fade.color = Color.Lerp(fade.color, Alpha1, delay * Time.deltaTime);
            Debug.Log(fade.color.a);
            yield return null;
        }
        Debug.Log("Broken out of the loop");
        SceneManager.LoadScene(level);
        fading = false;
        StartCoroutine(FadeIn());
    }
    IEnumerator FadeOut()
    {
        while ((fade.color.a < 0.99f) && (fading == true))
        {
            fade.color = Color.Lerp(fade.color, Alpha1, delay * Time.deltaTime);
            Debug.Log(fade.color.a);
            yield return null;
        }
        asyncLoad.allowSceneActivation = true;
        Debug.Log("Broken out of the loop");
        fading = false;
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        fade = GameObject.Find("Fade").GetComponent<Image>();
        while (fade.color.a > 0.01f)
        {
            fade.color = Color.Lerp(fade.color, Alpha0, delay * Time.deltaTime);
            Debug.Log(fade.color.a);
            yield return null;
        }
        Debug.Log("Broken out of the loop");
    }

    IEnumerator LoadYourAsyncScene()
    {
        while (!asyncLoad.isDone && !loadingLevel)
        {
            yield return null;
        }
        fading = true;
        StartCoroutine(FadeOut());
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }



}

