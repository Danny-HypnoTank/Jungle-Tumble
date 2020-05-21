using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    #region VARIABLES
    Player p { get { return GameManager.Instance.PlayerScript; } }    // JRH v0.0.14

    BuildType inputDevice;
    #endregion

    #region PROPERTIES
    #endregion

    #region METHODS

    #endregion

    #region MONOBEHAVIOUR
    void Update()
    {
        // JRH v0.2.11
        if (GameManager.Instance.Running)
        {
            // JRH v0.1.2
            if (Input.GetKeyDown(KeyCode.Escape)) GameManager.Instance.UIManager.TogglePause();


            if (!UIManager.IsPaused)
            {
                // JRH: v0.0.2
                if (Input.GetKeyDown(KeyCode.Space)) GameManager.Instance.PlayerScript.Jump();

                // JRH v0.0.13
                if (Input.GetKeyDown(KeyCode.S)) p.Slide();

                // JRH v0.0.14
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)) p.Move();

                // JRH v0.3.11
                if (Input.GetKeyDown(KeyCode.W) && GameManager.Instance.ActiveArtefact != null && !GameManager.Instance.ActiveArtefact.IsInUse) GameManager.Instance.ActiveArtefact.UseArtefact();
            }
        }

        // JRH v0.1.2
        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(2);

    }
    #endregion
}

public enum BuildType
{
    WIN_STANDALONE, WIN_WEBPLAYER, MOBILE_ANDROID, MOBILE_IOS
}