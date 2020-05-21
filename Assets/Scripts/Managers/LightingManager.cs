using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingManager : MonoBehaviour
{
    #region SET UP
        
    private Color EarlyDay;
        
    private Color MidDay;
    
    private Color LateDay;


    // Lerp time when switching
    [SerializeField]
    int LerpTime = 3;

    float WaitingTime;

    // THERE ARE FO- THREE LIGHTS
    [SerializeField]
    GameObject MainLight;

    
    // Possibly include the other lights?
    /*
    [SerializeField]
    GameObject SideLight;

    [SerializeField]
    GameObject BackLight;
    */
    #endregion


    #region AWAKE
    // ( Red, Green, Blue, Alpha )
    void Start()
    {
        // Sets the colour of the main light
        EarlyDay = new Color(1f,1f,1f,1f);

        MidDay = new Color(1f, 1f, 1f, 1f);

        LateDay = new Color(1f, 1f, 1f, 1f);

        // Possibly add intensity?

        // At the begining of the game it initiates the light cycle
        if (GameManager.Instance.Running)
        {
            StartCoroutine("LightCycle");
        }

    }
    #endregion

    #region EEEEEEENUM
    IEnumerator LightCycle()
    {

        // Repeats the loop
        while (true)
        {
            //WaitingTime = (((Mathf.RoundToInt(WaitingTime.time / LerpTime)) * LerpTime) - Time.time);

            //Debug.Log("Waiting");
            yield return new WaitForSeconds(WaitingTime);
            
            EarlyDayToMidDay();

            //Debug.Log("Waiting");
            yield return new WaitForSeconds(WaitingTime);

            MidDayToLateDay();

            //Debug.Log("Waiting");
            yield return new WaitForSeconds(WaitingTime);

            LateDayToEarlyDay();
        }

    }
    #endregion


    #region TIME CHANGE FUNCTIONS
    void EarlyDayToMidDay()
    {
        Color.Lerp(EarlyDay, MidDay, LerpTime);
        //Debug.Log("State change: Early to Mid");
    }
    
    void MidDayToLateDay()
    {
        Color.Lerp(MidDay, LateDay, LerpTime);
        //Debug.Log("State change: Mid to Late");
    }
    
    void LateDayToEarlyDay()
    {
        Color.Lerp(LateDay, EarlyDay, LerpTime);
        //Debug.Log("State change: Late to Early");
    }
    #endregion

}
