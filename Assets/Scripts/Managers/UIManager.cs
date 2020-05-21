using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region CANVAS SET UP

    [SerializeField]
    Canvas MainBackgroundCanvas;

    [SerializeField]
    Canvas MainMenuCanvas;

    [SerializeField]
    Canvas SubQuitMenuCanvas;

    [SerializeField]
    Canvas SubQuitAppCanvas;

    [SerializeField]
    Canvas DeathStateCanvas;

    [SerializeField]
    Canvas GameSessionCanvas;

    public Canvas DeathCanvas { get { return DeathStateCanvas; } }

    #endregion

    #region AUDIO SET UP

    [SerializeField]
    AudioSource MenuOpen;

    [SerializeField]
    AudioSource MenuClose;

    [SerializeField]
    AudioSource ButtonClick;

    [SerializeField]
    AudioSource GameTheme;

    #endregion

    #region PRIMITIVE SET UP

    static bool isPaused;
    int ScoreManagerScore;
    int ScoreManagerDistance;

    [SerializeField]Image activeArtIcon, passiveArtIcon;

    public static bool IsPaused { get { return isPaused; } }

    #endregion

    #region AWAKE SECTION

    void Awake()
    {
        isPaused = false;

        GameSessionCanvas.enabled = true;

        MainBackgroundCanvas.enabled = false;
        MainMenuCanvas.enabled = false;

        SubQuitMenuCanvas.enabled = false;
        SubQuitAppCanvas.enabled = false;

        DeathStateCanvas.enabled = false;
    }
    #endregion

    #region UPDATE SECTION

    void Update()
    {
        ScoreManagerScore = GameManager.Instance.ScoreManager.Score;
    }

    public void TogglePause(bool? pause = null)
    {
        if (pause == null) isPaused = !IsPaused;
        else isPaused = (bool)pause;

        if (IsPaused)
        {
            Time.timeScale = 0;
            GameSessionCanvas.enabled = false;
            MainMenuCanvas.enabled = true;
            MainBackgroundCanvas.enabled = true;

            // This pauses what ever you specify
            // putaudioinhere.Pause();

            // JRH v0.2.3: Play the appropriate audio for unpausing the game
            AudioManager.Instance.ChangeBGMVol(0.1f, 0);
            AudioManager.Instance.PlaySFX("PauseSFX");
            // Ignore player input here?
        }
        else
        {
            Time.timeScale = 1;
            GameSessionCanvas.enabled = true;
            MainMenuCanvas.enabled = false;
            MainBackgroundCanvas.enabled = false;

            // This continues the music from where it last paused
            // putaudioinhere.UnPause();

            // JRH v0.2.3: Play the appropriate audio for unpausing the game
            AudioManager.Instance.ChangeBGMVol(1, 0);
            AudioManager.Instance.PlaySFX("UnpauseSFX");
            // Unignore player input here?
        }
    }

    #endregion

    #region CORE BUTTON FUNCTIONS

    public void ContinueBTN()
    {        
        TogglePause();
    }

    public void RestartBTN()
    {
        TogglePause(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitMenuBTN()
    {
        MainMenuCanvas.enabled = false;
        SubQuitMenuCanvas.enabled = true;
    }

    public void QuitAppBTN()
    {
        MainMenuCanvas.enabled = false;
        SubQuitAppCanvas.enabled = true;
    }

    #endregion

    #region SUB MENU FUNCTIONS

    public void QuitMenuYesBTN()
    {        
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitMenuNoBTN()
    {
        SubQuitMenuCanvas.enabled = false;
        MainMenuCanvas.enabled = true;        
    }

    public void QuitAppYesBTN()
    {
        Application.Quit();
    }

    public void QuitAppNoBTN()
    {
        MainMenuCanvas.enabled = true;
        SubQuitAppCanvas.enabled = false;
    }
    
    #endregion

    // JRH v0.2.12
    public void SetDeathStateCanvas()
    {
        GameSessionCanvas.enabled = false;
        DeathStateCanvas.enabled = true;
        DeathStateCanvas.gameObject.GetComponent<DeathStateUI>().RunDeathState();
    }

    public void UpdateArtefactUISprites()
    {
        activeArtIcon.sprite = GameManager.Instance.ActiveArtefact.IsInUse ? GameManager.Instance.ActiveArtefact.DisabledIcon : GameManager.Instance.ActiveArtefact.EnabledIcon;
        passiveArtIcon.sprite = GameManager.Instance.PassiveArtefact.DisabledIcon;
    }
}
