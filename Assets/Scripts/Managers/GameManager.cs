using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region VARIABLES

    #region PRIVATE
    // JRH v0.0.6: Manager Variables
    static GameManager instance;

    // JRH v0.0.10: GameState variables
    bool gameRunning;

    [Header("Physics Variables:")]
    [SerializeField]
    float gameSpeed;    // JRH v0.0.3

    [SerializeField]
    float maxJumpHeight, maxJumpDistance;    // JRH v0.0.3

    [SerializeField]
    float laneDistance;    // JRH v0.0.4

    float playerDistance;   // JRH v0.3.0


    [Space][Header("Artefact Variables")][SerializeField]
    Artefact activeArtefact;
    [SerializeField]Artefact passiveArtefact;
    bool ghostMode;

    [Space]
    [Header("Level Variables:")]
    [SerializeField]
    int playerLevel = 0;    // JRH v0.3.0
    [SerializeField]
    int maxLevel; // JRH v0.3.0
    [SerializeField]
    float levelBaseMultiplier, levelModifier;

    [Space]
    [Header("Row Variables:")]

    [SerializeField]
    int maxRows;    // JRH v0.1.2

    [SerializeField]
    float maxZDist;    // JRH v0.1.2

    [SerializeField]
    float recoveryTime;    // JRH v0.2.1

    // JRH.v0.0.6: Object Variables
    Player playerScript;
    Boulder boulder;
    #endregion

    #region PUBLIC
    public static GameManager Instance;    // JRH v0.0.6

    // JRH v0.2.0
    public UIManager UIManager { get { return GetComponent<UIManager>(); } }
    public GameMenuManager GameMenuManager { get { return GetComponent<GameMenuManager>(); } }
    public InputManager InputManager { get { return GetComponent<InputManager>(); } }
    public PlatformManager PlatformManager { get { return GetComponent<PlatformManager>(); } }
    public ScoreManager ScoreManager { get { return GetComponent<ScoreManager>(); } }

    // JRH v0.2.0
    public PlatformGenerator PlatformGenerator { get { return GetComponent<PlatformGenerator>(); } }  // JRH v0.1.2
    public ChunkLibrary ChunkLibrary { get { return GetComponent<ChunkLibrary>(); } }    // JRH v0.1.10

    public bool Running { get { return gameRunning; } }    // JRH v0.0.13
    public float GameSpeed { get { return gameSpeed; } }    // JRH v0.0.4

    public float JumpHeight { get { return maxJumpHeight; } }     // JRH v0.0.11
    public float JumpDist { get { return maxJumpDistance; } }    // JRH v0.0.11
    public float JumpTime { get { return maxJumpDistance / gameSpeed; } }    // JRH v0.0.11

    public float LaneDistance { get { return laneDistance; } }    // JRH v0.0.4

    public int MaxPlatformRows { get { return maxRows; } }
    public float MaxPlatformDistance { get { return maxZDist; } }

    public float Distance { get { return playerDistance; } }
    public int Level { get { return playerLevel; } }

    public Player PlayerScript { get { return playerScript; } }    // JRH v0.0.2
    public Artefact ActiveArtefact { get { return activeArtefact; } }
    public Artefact PassiveArtefact { get { return passiveArtefact; } }
    public bool GhostMode { get { return ghostMode; } }
     
    public float RecoveryTime { get { return recoveryTime; } }    // JRH v0.2.1
    #endregion

    #endregion

    #region METHODS

    #region CONSTRUCTOR
    #endregion

    #region USER-DEFINED
    // JRH v0.0.6 
    /// <summary>
    /// Sets the static variable instance 
    /// </summary>
    void SetSingleton()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    // JRH v0.0.11
    /// <summary>
    /// Returns a new gravicational constant as a Vector3, given the serialized values of MaxJumpHeight, MaxJumpDistance and GameSpeed
    /// </summary>
    Vector3 SetGrav()
    {
        float gravConst = (2 * maxJumpHeight) / Mathf.Pow((maxJumpDistance / gameSpeed), 2); // JRH v0.1.2 - Changed from Mathf.Pow((maxJumpDistance / gameSpeed / 2)
        return new Vector3(0f, -gravConst, 0f);
    }

    // JRH v0.0.13
    /// <summary>
    /// Allows outside scripts to end the game
    /// </summary>
    public void EndGame()
    {
        gameRunning = false;
        playerScript.gameObject.SetActive(false);
        GameObject.Find("Boulder").GetComponent<Boulder>().RollForever();

        //Debug.LogWarning("Final Score: " + ScoreManager.Score);
        ScoreManager.UpdateLeaderboard();
        ScoreManager.UpdateLastDistance();

        GameObject.Find("Boulder").GetComponent<Boulder>().RollForever();

        UIManager.SetDeathStateCanvas();

        AudioManager.Instance.ChangeBGMVol(0.1f, 1);
        AudioManager.Instance.PlaySFX("GameOverSFX");
        // JRH v0.2.3: Fade out the BGM
    }

    public void StaggerGamespeed()
    {
        StartCoroutine(StaggerGamespeedCoroutine());
    }

    IEnumerator StaggerGamespeedCoroutine()
    {
        float initSpeed = gameSpeed;
        float t = 0;

        gameSpeed = 2.5f;

        
        do
        {
            t += Time.deltaTime;
            gameSpeed = Mathf.Lerp(2.5f, 7.5f, t);
            yield return null;
        } while (t < recoveryTime);
        

        //yield return new WaitForSeconds(recoveryTime);

        gameSpeed = initSpeed;
    }

    public void SetArtefacts(Artefact artefact)
    {
        if (artefact.IsActivePower) activeArtefact = artefact;
        else
        {
            passiveArtefact = artefact;
            passiveArtefact.UseArtefact();
        }
    }

    public void ActivateGhost()
    {
        ghostMode = true;
    }
    #endregion

    #region MONOBEHAVIOUR
    private void Awake()
    {
        Application.targetFrameRate = 30;   // JRH v0.1.3

        SetSingleton(); // JRH v0.0.6 
        //SetManagers(); // JRH v0.0.6  // JRH v0.2.0: Legacy Code

        Physics.gravity = SetGrav(); // JRH v0.0.11

        playerScript = GameObject.Find("PlayerContainer").GetComponent<Player>(); // JRH v0.0.2

        gameRunning = true; // JRH v0.1.2

        if (passiveArtefact != null) passiveArtefact.UseArtefact();
    }

    private void Update()
    {
        //if (Physics.gravity != SetGrav()) Physics.gravity = SetGrav(); // JRH v0.0.11

        if (gameRunning)
        {
            playerDistance += Time.deltaTime * gameSpeed / laneDistance;
            // Debug.Log("Score is " + playerScore);           
            if (playerScript.transform.position.y < -1)
            {
                playerScript.GetComponent<Collider>().enabled = false;
                EndGame();
            }
        }

        // JRH v0.3.0: Level Manager
        if (playerLevel + ((playerLevel - 1) * levelModifier) < (playerDistance / levelBaseMultiplier))
        {
            playerLevel = Mathf.Min(playerLevel + 1, maxLevel);
            // JRH v0.3.0: Increment the level only when distance exceeds level * base multiplier plus the modifier
        }

        if (playerScript.transform.position.y < -35) playerScript.gameObject.SetActive(false);
    }
    #endregion

    #endregion
}
