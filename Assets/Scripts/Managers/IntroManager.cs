using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    [SerializeField]
    GameObject sidCanvas, bludwarfCanvas;
    [SerializeField]
    LoadSceneManager load;

    void Awake()
    {
        PlayerPrefs.SetInt("ArtPass", 0);
        PlayerPrefs.SetInt("ArtAct", 0);

        StartCoroutine(IntroRoutine());
    }

    IEnumerator IntroRoutine()
    {
        sidCanvas.SetActive(true);
        bludwarfCanvas.SetActive(false);
        load.gameObject.SetActive(false);

        yield return new WaitForSeconds(2);

        sidCanvas.SetActive(false);
        bludwarfCanvas.SetActive(true);

        yield return new WaitForSeconds(2);

        load.gameObject.SetActive(true);
        load.LoadScene(1);
        gameObject.SetActive(false);
    }
}
