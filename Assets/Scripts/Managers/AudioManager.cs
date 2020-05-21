using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    #region VARIABLES
    static AudioManager instance;

    [SerializeField]
    AudioSource bgmSource;

    [SerializeField]
    AudioSource sfxSource;

    AudioLibrary library { get { return GetComponent<AudioLibrary>(); } }

    float bgmVolMax, sfxVolMax;

    int currentSceneIndex;
    #endregion

    #region PROPERTIES
    public static AudioManager Instance { get { return instance; } }
    public AudioLibrary Library { get { return library; } }
    #endregion

    #region METHODS

    // JRH v0.2.3
    /// <summary>
    /// Assigns the instance variable based on the Singleton pattern
    /// </summary>
    void Singleton()
    {
        if (instance == null) instance = this; else Destroy(gameObject);
    }

    // JRH v0.2.3:
    /// <summary>
    /// Plays the game's background music
    /// </summary>
    void PlayBGM(string lookup = "GameBGM")
    {
        bgmSource.clip = library.LookupAudioClip(lookup);
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    // JRH v0.2.3:
    /// <summary>
    /// Publicly accessible, plays a specified audio cue through the SFX Source via the Audio Dictionary
    /// </summary>
    /// <param name="lookup"></param>
    public void PlaySFX(string lookup = "GenericSFX")
    {
        AudioClip clip = library.LookupAudioClip(lookup);

        sfxSource.PlayOneShot(clip);
    }

    // JRH v0.2.3:
    /// <summary>
    /// Publicly accessible, increases or decreases the BGM volume to a specified level over a specified amount of time
    /// </summary>
    /// <param name="vol"></param>
    /// <param name="timeMax"></param>
    public void ChangeBGMVol(float vol = 0, float timeMax = 1)
    {
        // JRH v0.2.3: Before running the Coroutine, manipulate the parameters so they are suitable for passing in
        vol = Mathf.Clamp(vol, 0, 1) * bgmVolMax;
        // JRH v0.2.3: Sets the volume to between 0 and 1, them multiplies it by the maximum volume to ensure it stays proportional
        timeMax = Mathf.Clamp(timeMax, 0, 5);
        // JRH v0.2.3: Sets the amount of time to manipulate the volume over between 0 and 5 seconds, to not have extensively long or negative fades

        // JRH v0.2.3: Now that the parameters have been modified, run the Coroutine
        StartCoroutine(ChangeBGMVolCoroutine(vol, timeMax));
    }

    // JRH v0.2.3:
    IEnumerator ChangeBGMVolCoroutine (float volumeTerminal, float timeMax)
    {
        float timeInitial = Time.time;
        timeMax += timeInitial;
        float volumeInitial = bgmSource.volume;

        // JRH v0.2.3: Reduce the volume linearly over the time passed in
        while (Time.time < timeMax)
        {
            bgmSource.volume = Mathf.Lerp(volumeInitial, volumeTerminal, Time.time - timeInitial);
            yield return null;
        }

        // JRH v0.2.3: Before ending the Coroutine, ensure that the volume is set to it's new level to not allow variation with time.deltatime
        bgmSource.volume = volumeTerminal;
    }

    public void Mute(bool source)
    {
        if (source) bgmSource.mute = !bgmSource.mute;
        else sfxSource.mute = !sfxSource.mute;
    }
    #endregion

    #region MONOBEHAVIOUR
    private void Awake()
    {
        Singleton();    // JRH v0.2.3: Run the Singleton pattern
        DontDestroyOnLoad(instance);

        // JRH v0.2.3: Assign the maximum allowed values for BGMsource and SFXsource based on their current inspector values
        bgmVolMax = bgmSource.volume; 
        sfxVolMax = sfxSource.volume;


        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // JRH v0.2.11:
    /// <summary>
    /// Assigns the OnLevelLoaded method to the sceneLoaded event
    /// </summary>
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    // JRH v0.2.11: 
    /// <summary>
    /// This should never be called, but removes OnLevelLoaded from sceneLoaded just in case
    /// </summary>
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
    }

    // JRH v0.2.11:
    /// <summary>
    /// Called when a new level is loaded, checks to see if a new scene has loaded or reset, and plays audio appropriately
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        // JRH v0.2.11: If the scene has changed, switch the audio track to a new one, else keep it playing
        if (scene.buildIndex != currentSceneIndex)
        {
            bgmSource.Stop();
            // JRH v0.2.11: Stop the audio that's playing atm
            switch (scene.buildIndex)
            {
                case 1: PlayBGM("MenuBGM"); break;
                case 2: PlayBGM("GameBGM"); break;
            }
            // JRH v0.2.11: Play the appropriate new BGM track/s
            currentSceneIndex = scene.buildIndex;

            // JRH v0.2.11: Set the currentSceneIndex, used for checking this very method, to the new current scene
            bgmSource.volume = bgmVolMax;
        }
    }
    #endregion
}
