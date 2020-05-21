using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    //Regions confused me with certain things
    #region VARIABLES

    #region PRIVATE
    // JRH v0.0.10: Variable to hold the total player score
    // EH v0.1.14 Public static to allow access for UI manager
    int score   // JRH v0.1.4: score turned to property rather than variable as calculation method remains constant
    {
        get
        {
            return (Mathf.RoundToInt(GameManager.Instance.Distance) * scoreModifier) + ((coins * coinValue) + (bags * bagValue) + (gems * gemValue) * scoreModifier); // JRH v0.1.8: now returns distance + collectables
        }
    } // JRH v0.0.10: playerScore does not need to be serialized

    // JRH v0.0.10 Variables to hold the individual score components
    // EH v0.1.14 Public static to allow access for UI manager
    //float distance;

    int coins, bags, gems;
    // JRH v0.1.6: artifacts and artifact score removed due to having inherent value outside of score and finite nature

    [SerializeField]
    int scoreModifier;

    [Header("Reward Values:")]
    [SerializeField]
    int coinValue;
    [SerializeField]
    int bagValue;
    [SerializeField]
    int gemValue;

    float lastDistance;

    [SerializeField]
    Text distanceText, profitText;
    #endregion

    #region PROPERTIES  
    // JRH v0.0.10
    public int Distance { get { return (Mathf.RoundToInt(GameManager.Instance.Distance) * scoreModifier); } }
    public int Profit { get { return ((coins * coinValue) + (bags * bagValue) + (gems * gemValue)) * scoreModifier; } }
    public int Score { get { return Distance + Profit; } }
    public int TopHighScore { get { return PlayerPrefs.GetInt("HighScore1"); } }
    public int Modifier { get { return scoreModifier; } }
    #endregion

    #endregion

    #region METHODS

    #region CONSTRUCTOR
    #endregion

    #region USER-DEFINED
    // JRH v0.1.6
    /// <summary>
    /// Updates the leaderboard held in PlayerPrefs
    /// </summary>
    public void UpdateLeaderboard()
    {
        bool updateRequired = false;
        List<int> leaderBoard = new List<int>();
        leaderBoard.AddRange(new int[] { PlayerPrefs.GetInt("HighScore1"), PlayerPrefs.GetInt("HighScore2"), PlayerPrefs.GetInt("HighScore3") });

        foreach (int i in leaderBoard) if (score > i)
            {
                updateRequired = true;
                break;
            }

        if (updateRequired)
        {

            int nextInt = score;
            for (int i = 0; i < leaderBoard.Count; i++)
            {
                /*int foo = leaderBoard[i];
                leaderBoard[i] = score;
                nextInt = foo;
                Debug.Log(foo + ", " + leaderBoard[i] + ", " + nextInt);*/

                if (nextInt > i)
                {
                    leaderBoard.Insert(i, nextInt);
                    nextInt = 0;
                }
            }

            PlayerPrefs.SetInt("HighScore1", leaderBoard[0]);
            PlayerPrefs.SetInt("HighScore2", leaderBoard[1]);
            PlayerPrefs.SetInt("HighScore3", leaderBoard[2]);
        }

 
    }

    public void UpdateLastDistance()
    {
        PlayerPrefs.SetFloat("LastDistance", GameManager.Instance.Distance);
    }

    public void SetNewHighScore(int newScore)
    {
        int oldHighScore = PlayerPrefs.HasKey("HighScore") ? PlayerPrefs.GetInt("HighScore") : 0;
        if (newScore > oldHighScore) PlayerPrefs.SetInt("HighScore", newScore);
        Debug.Log(newScore);
    }

    public void IncrementReward(RewardType type)
    {
        switch (type)
        {
            case RewardType.COIN: coins++; break;
            case RewardType.BAG: bags++; break;
            case RewardType.GEM: gems++; break;
            default: break;
        }
    }
    #endregion

    #region MONOBEHAVIOUR
    void Start()
    {
        if (PlayerPrefs.HasKey("LastDistance")) lastDistance = PlayerPrefs.GetFloat("LastDistance"); // Sets LastDistance to the playerpref set by the last playthrough
        else PlayerPrefs.SetFloat("LastDistance", 0); // If there is no LastDistance, creates it and sets it to zero
        //Debug.Log(PlayerPrefs.GetFloat("LastDistance"));
    }

    void Update()
    {
        distanceText.text = Distance.ToString() + "m";
        profitText.text = "$" + Profit.ToString();
    }
    #endregion

    #endregion
}