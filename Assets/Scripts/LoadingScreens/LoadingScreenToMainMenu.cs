//DPH Ver0.2.8
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//DPH Ver0.2.8
public class LoadingScreenToMainMenu : MonoBehaviour
{
    //DPH Ver0.2.8
    AsyncOperation ao;
    public GameObject loadingScreenBG;
    public Slider progBar;
    public Text loadingText;

    public bool isFakeLoadingBar = false;
    public float fakeIncrement = 0f;
    public float fakeTiming = 0f;

    //DPH Ver0.2.8
    //Loading first level
    public void LoadLevel00()
    {
        loadingScreenBG.SetActive(true);
        progBar.gameObject.SetActive(true);
        loadingText.gameObject.SetActive(true);
        loadingText.text = "loading...";

        if (!isFakeLoadingBar)
        {
            StartCoroutine(LoadLevelWithRealProgress());
        }
        else
        {

        }
    }

    //DPH Ver0.2.8
    IEnumerator LoadLevelWithRealProgress()
    {
        yield return new WaitForSeconds(1);

        ao = SceneManager.LoadSceneAsync(0);

        ao.allowSceneActivation = false;

        while (!ao.isDone)
        {
            progBar.value = ao.progress;

            if (ao.progress == 0.9f)
            {
                loadingText.text = "Press F To Continue";
                if (Input.GetKeyDown(KeyCode.F))
                {
                    ao.allowSceneActivation = true;
                }
            }
            Debug.Log(ao.progress);
            yield return null;
        }
    }
}

