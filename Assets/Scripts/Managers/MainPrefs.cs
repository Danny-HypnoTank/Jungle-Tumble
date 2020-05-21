using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPrefs : MonoBehaviour
{

    // Yes I know this set up isn't ideal, needs refinement

    public static int NewProfile; // used when no player profile is detected, i.e. new player or reset
    // -----
    public static int PlayerCurrency;  // Currency collected from game session, to be spent on artefact/custom items
    public static int PlayerCustomise; // Track custom options going into the next session
    public static int PlayerArtefacts; // Track artefact options going into the next session
    // -----
    public static int DoubleJumpCheck; // Check artefact(s)
    public static int SwingCheck;
    public static int AttackCheck;
    public static int ObjectLowerCheck;
    public static int HoverCheck;
    public static int EagleEyeCheck;
    public static int GhostCheck;
    public static int MoneyMagenetCheck;
    // -----
    public static int DoubleJumpPrice; // Check artefact(s) prices
    public static int SwingPrice;
    public static int AttackPrice;
    public static int ObjectLowerPrice;
    public static int HoverPrice;
    public static int EagleEyePrice;
    public static int GhostPrice;
    public static int MoneyMagenetPrice;
    // -----
    public static int MusicToggle; // Check option(s)
    public static int SFXToggle;
    public static int VibrationToggle;
    // -----
    public static int Custom1Check; // Check custom item(s)
    public static int Custom2Check;
    public static int Custom3Check;
    public static int Custom4Check;
    public static int Custom5Check;
    public static int Custom6Check;
    public static int Custom7Check;
    public static int Custom8Check;
    public static int Custom9Check;
    public static int Custom10Check;
    // -----


    void Awake()
    {
        // Retrieve player prefs, core player pres, artefacts, options, custom

        PlayerCurrency = PlayerPrefs.GetInt("Player Currency");
        PlayerCustomise = PlayerPrefs.GetInt("Player Customise");
        PlayerArtefacts = PlayerPrefs.GetInt("Player Artefacts");
        // -----
        DoubleJumpCheck = PlayerPrefs.GetInt("Double Jump");
        SwingCheck = PlayerPrefs.GetInt("Swing");
        AttackCheck = PlayerPrefs.GetInt("Attack");
        ObjectLowerCheck = PlayerPrefs.GetInt("Object Lower");
        HoverCheck = PlayerPrefs.GetInt("Hover");
        EagleEyeCheck = PlayerPrefs.GetInt("Eagle Eye");
        GhostCheck = PlayerPrefs.GetInt("Ghost");
        MoneyMagenetCheck = PlayerPrefs.GetInt("Money Magnet");
        // -----
        MusicToggle = PlayerPrefs.GetInt("Music Toggle");
        SFXToggle = PlayerPrefs.GetInt("SFX Toggle");
        VibrationToggle = PlayerPrefs.GetInt("SFX Toggle");
        // -----
        Custom1Check = PlayerPrefs.GetInt("CUS 1");
        Custom2Check = PlayerPrefs.GetInt("CUS 2");
        Custom3Check = PlayerPrefs.GetInt("CUS 3");
        Custom4Check = PlayerPrefs.GetInt("CUS 4");
        Custom5Check = PlayerPrefs.GetInt("CUS 5");
        Custom6Check = PlayerPrefs.GetInt("CUS 6");
        Custom7Check = PlayerPrefs.GetInt("CUS 7");
        Custom8Check = PlayerPrefs.GetInt("CUS 8");
        Custom9Check = PlayerPrefs.GetInt("CUS 9");
        Custom10Check = PlayerPrefs.GetInt("CUS 10");

    }

}
