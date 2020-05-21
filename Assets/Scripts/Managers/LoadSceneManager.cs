using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    [SerializeField]
    Text loadProgressText;

    // JRH v0.2.12: To be called by buttons etc., sets the parameters for readying the async loading of a new scene while the LoadScene canvas plays out
    public void LoadScene(int index)
    {
        index = index > 0 && index < SceneManager.sceneCountInBuildSettings ? index : 0;
        // JRH v0.2.12: Ensure the scene being loaded is between 0 and the highest ranked scene, if not substituting in a return to the main menu
        GetComponent<Canvas>().enabled = true;
        // JRH v0.2.12: Turn on the canvas this script is attached to
        StartCoroutine(LoadSceneViaAsync(index));
        // JRH v0.2.12: Run the coroutine with the modified index so that the proper scene begins loading
    }

    IEnumerator LoadSceneViaAsync(int index)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(index);

        AudioManager.Instance.StopBGM();
        AudioManager.Instance.PlaySFX("ButtonYesSFX");

        async.allowSceneActivation = false;

        // JRH v0.2.12: Run only for as long as the asyc is done
        while (!async.isDone)
        {
            loadProgressText.text = Mathf.RoundToInt(async.progress * 100) + "%";
            // JRH v0.2.12: Round the progress to an int and multiply by 100 to get an accurate int percentage
            if (async.progress == 0.9f)
            {
                // JRH v0.3.11: Ensure that having paused the game does not carry over to the next scene
                Time.timeScale = 1;
                async.allowSceneActivation = true;
            }
            // JRH v0.2.12: If the level has all but loaded in, allow it to be loaded again
            yield return null;
        }
    }
}
