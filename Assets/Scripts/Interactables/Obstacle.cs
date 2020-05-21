using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : Interactable
{
    #region VARIABLES

    #region PRIVATE
    [Header("Obstacle Properties")]
    [SerializeField]
    [Range(1, 3)]
    int obsWidth;

    [SerializeField]
    bool isHighCollider;

    [SerializeField]
    char code;
    #endregion

    #region PUBLIC
    //public int Height { get { return obsHeight; } }
    public int Width { get { return obsWidth; } }
    public bool HighCollider { get { return isHighCollider; } }
    public char Code { get { return code; } }
    #endregion

    #endregion

    #region METHODS
    // JRH v0.1.9:
    /// <summary>
    /// Called by the player on collision, triggers the end state when the player fucks up and hits the obstacle
    /// </summary>
    public override void Interact()
    {
        //GameObject.Find("MenuClose").GetComponent<AudioSource>().Play();    // JRH v0.1.9: Plays audial feedback to show the player did a bad
        //GameManager.Instance.PlayerScript.gameObject.SetActive(false);    // JRH v0.1.9: Turns the player off
        //GameManager.Instance.EndGame();    // JRH v0.1.9: Ends the game session
        GameManager.Instance.PlayerScript.Damage();    // JRH v0.2.0: Cause the player to suffer some damage
    }

    #endregion
}
