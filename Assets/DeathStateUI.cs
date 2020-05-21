using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathStateUI : MonoBehaviour
{
    [SerializeField]
    Text distanceText, profitText, scoreText, highScoreText;

    [SerializeField]
    Button restartButton, quitToMenuButton;

    public void RunDeathState()
    {
        StartCoroutine(DeathStateCoroutine());
    }

    IEnumerator DeathStateCoroutine()
    {
        int distanceRoll = 0, profitRoll = 0, scoreRoll, highScoreRoll;
        int modifier = GameManager.Instance.ScoreManager.Modifier;

        //distanceText.gameObject.SetActive(false);
        //profitText.gameObject.SetActive(false);
        //scoreText.gameObject.SetActive(false);
        //highScoreText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        quitToMenuButton.gameObject.SetActive(false);

        //yield return new WaitForSeconds(1);

        // JRH v0.2.10: Roll up the distance
        //distanceText.gameObject.SetActive(true);
        distanceRoll = modifier;
        do
        {
            int m = (int)Mathf.Pow(10, 1 + distanceRoll.ToString().Length - modifier.ToString().Length) / 10;
            distanceRoll += modifier * m;
            distanceRoll = Mathf.Min(distanceRoll, GameManager.Instance.ScoreManager.Distance);
            distanceText.text = distanceRoll.ToString();
            AudioManager.Instance.PlaySFX("RewardSFX");
            yield return new WaitForSeconds(1f/30);
        } while (distanceRoll < GameManager.Instance.ScoreManager.Distance);

        yield return new WaitForSeconds(1f/15);

        // JRH v0.2.10: Roll up the Profit
        //profitText.gameObject.SetActive(true);
        profitRoll = modifier;
        do
        {
            int m = (int)Mathf.Pow(10, 1 + profitRoll.ToString().Length - modifier.ToString().Length) / 10;
            profitRoll += modifier * m;
            profitRoll = Mathf.Min(profitRoll, GameManager.Instance.ScoreManager.Profit);
            profitText.text = profitRoll.ToString();
            AudioManager.Instance.PlaySFX("RewardSFX");
            yield return new WaitForSeconds(1f/30);
        } while (profitRoll < GameManager.Instance.ScoreManager.Profit);

        yield return new WaitForSeconds(1f/15);

        scoreRoll = distanceRoll;
        do
        {
            int m = (int)Mathf.Pow(10, 1 + scoreRoll.ToString().Length - modifier.ToString().Length) / 10;
            scoreRoll += modifier * m;
            scoreRoll = Mathf.Min(scoreRoll, GameManager.Instance.ScoreManager.Score);
            scoreText.text = scoreRoll.ToString();
            AudioManager.Instance.PlaySFX("RewardSFX");
            yield return new WaitForSeconds(1f/30);
        } while (scoreRoll < GameManager.Instance.ScoreManager.Score);

        GameManager.Instance.ScoreManager.SetNewHighScore(scoreRoll);

        // JRH v0.2.10: With all that out of the way, enable the buttons again
        restartButton.gameObject.SetActive(true);
        quitToMenuButton.gameObject.SetActive(true);

        yield return null;
    }
}
