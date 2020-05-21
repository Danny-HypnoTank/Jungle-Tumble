using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{

    #region AUDIO SETUP
    // Audio sources to be played when sub menus open or close
    [SerializeField]
    AudioSource MenuOpen;

    [SerializeField]
    AudioSource MenuClose;

    [SerializeField]
    AudioSource ButtonClick;

    [SerializeField]
    AudioSource MenuTheme;
    #endregion

    #region CANVAS SETUP
    [SerializeField]
    Canvas SubQuitCanvas;

    [SerializeField]
    Canvas SubResetCanvas;

    [SerializeField]
    Canvas MainmenuCanvas;

    [SerializeField]
    Canvas MainBackgroundCanvas;

    [SerializeField]
    Canvas OptionsMenuCanvas;

    [SerializeField]
    Canvas ArtefactMenuCanvas;

    [SerializeField]
    Canvas ArtefactConfirmCanvas;

    [SerializeField]
    Canvas LoadingScreenCanvas;

    [SerializeField]
    Canvas CustomMenuCanvas;

    [SerializeField]
    Canvas CustomConfirmCanvas;

    [SerializeField]
    Canvas NewProfileCanvas;

    [SerializeField]
    Canvas HighScoreCanvas;

    #endregion

    #region BUTTON PLAYER PREFS (?)

    [SerializeField]
    Button NEWConfirm;

    #endregion

    #region BUTTON SETUP ARTEFACT

    [SerializeField]
    Button ARTDoubleJump;

    [SerializeField]
    Button ARTSwing;

    [SerializeField]
    Button ARTAttack;

    [SerializeField]
    Button ARTObjectlower;

    [SerializeField]
    Button ARTHover;

    [SerializeField]
    Button ARTEagleEye;

    [SerializeField]
    Button ARTGhost;

    [SerializeField]
    Button ARTMoneyMagnet;

    #endregion

    #region BUTTON SETUP CUSTOM

    [SerializeField]
    Button CUS1;

    [SerializeField]
    Button CUS2;

    [SerializeField]
    Button CUS3;

    [SerializeField]
    Button CUS4;

    [SerializeField]
    Button CUS5;

    [SerializeField]
    Button CUS6;

    [SerializeField]
    Button CUS7;

    [SerializeField]
    Button CUS8;

    [SerializeField]
    Button CUS9;

    [SerializeField]
    Button CUS10;

    #endregion

    #region TOGGLE SETUP OPTIONS

    [SerializeField]
    Toggle MusicToggle;

    [SerializeField]
    Toggle SFXToggle;

    [SerializeField]
    Toggle VibrationToggle;

    bool MusicToggleVal;

    bool SFXToggleVal;

    bool VibrateToggleVal;

    #endregion

    #region MENU BUTTONS (OPT, ART, CUS)

    [SerializeField]
    Button PlayerReset;

    [SerializeField]
    Button CloseOptions;

    [SerializeField]
    Button CloseArtefact;

    [SerializeField]
    Button CloseCustom;

    [SerializeField]
    Button SaveSettingsOptions;

    [SerializeField]
    Button SaveSettingsArtefacts;

    [SerializeField]
    Button SaveSettingsCustom;
    #endregion

    #region PRIMITIVE SETUP

    private bool LoadingIsRunning = false;

    [SerializeField]
    Text HighscoreText;

    #endregion

    #region PRICE SETUP ARTEFACTS

    int MMDoubleJumpPrice;
    int MMSwingPrice;
    int MMAttackPrice;
    int MMObjectLowerPrice;
    int MMHoverPrice;
    int MMEagleEyePrice;
    int MMGhostPrice;
    int MMMoneyMagnetPrice;

    #endregion

    #region PRICE SETUP CUSTOM

    int MMCustom1Check;
    int MMCustom2Check;
    int MMCustom3Check;
    int MMCustom4Check;
    int MMCustom5Check;
    int MMCustom6Check;
    int MMCustom7Check;
    int MMCustom8Check;
    int MMCustom9Check;
    int MMCustom10Check;

    #endregion

    #region PLAYER CURENCY
    int MMPlayerCurrency;
    #endregion

    #region PLAYER PROFILE PRIMATIVES
    bool NewProfile;

    [SerializeField]
    InputField NewProfileTextField;
    #endregion

    #region LOADING SETUP

    AsyncOperation ALoad;
    #endregion

    #region SILDER SETUP

    [SerializeField]
    Slider LoadingProgressBar;
    #endregion

    #region TEXT SETUP

    [SerializeField]
    Text LoadingTxt;
    #endregion


    // ________________________________________

    #region AWAKE SECTION

    void Start()
    {

        Debug.Log("Activate MainBG, and Main menu");
        MainmenuCanvas.enabled = false; // Main menu OFF check || Don't ask me why, it doesn't work unless it turns off and on again               
        MainBackgroundCanvas.enabled = true; // Main menu background ON check 
        MainmenuCanvas.enabled = true; // Main menu ON check               
        // -----
        Debug.Log("Deactivate new profile menu");
        NewProfileCanvas.enabled = false;

        Debug.Log("Deactivate Option menu");
        OptionsMenuCanvas.enabled = false; // Option menu OFF check
        // -----
        Debug.Log("Deactivate artefact menu, artefact confirm menu");
        ArtefactMenuCanvas.enabled = false; //  Black market menu OFF check
        ArtefactConfirmCanvas.enabled = false; //  Artefact buy confirm menu OFF check
        // -----
        Debug.Log("Deactivate loading menu");
        LoadingScreenCanvas.enabled = false; // Loading Screen OFF check
        // -----
        Debug.Log("Deactivate sub quit and reset menu");
        SubQuitCanvas.enabled = false; //  Sub quit menu OFF check      
        SubResetCanvas.enabled = false; // Sub Reset menu OFF check  
        // -----
        Debug.Log("Deactivate custom menu, custom confirm");
        CustomMenuCanvas.enabled = false; //  Customise player menu OFF check
        CustomConfirmCanvas.enabled = false; //  Confirm custom buy menu OFF check
        // -----
        HighScoreCanvas.enabled = false;

        // Player pref checks

        /*
        PlayerPrefs.GetInt("MusicToggle"); // Check music settings
        PlayerPrefs.GetInt("SFXToggle"); //  Check sfx settings
        PlayerPrefs.GetInt("VibrationToggle"); //  Check Vibration settings
        // -----
        PlayerPrefs.GetInt("PlayerCurrency"); // Check player currency
        PlayerPrefs.GetInt("PlayerCustomise"); //  Check players customisation
        PlayerPrefs.GetInt("PlayerArtefacts"); // Check players artefacts
        // -----
        */

        // Setting save OFF check

        Debug.Log("Deactivate save options default");
        SaveSettingsOptions.interactable = false; // Check save settings options OFF check
        SaveSettingsArtefacts.interactable = false; // Check save settings artefacts OFF check
        SaveSettingsCustom.interactable = false; // Check save settings custom OFF check

        // -----

        //  Price check artefacts
        MMDoubleJumpPrice = MainPrefs.DoubleJumpPrice;
        MMSwingPrice = MainPrefs.SwingPrice;
        MMAttackPrice = MainPrefs.AttackPrice;
        MMObjectLowerPrice = MainPrefs.ObjectLowerPrice;
        MMHoverPrice = MainPrefs.HoverPrice;
        MMEagleEyePrice = MainPrefs.EagleEyePrice;
        MMGhostPrice = MainPrefs.GhostPrice;
        MMMoneyMagnetPrice = MainPrefs.MoneyMagenetPrice;

        // -----

        // Price check custom
        MMCustom1Check = MainPrefs.Custom1Check;
        MMCustom2Check = MainPrefs.Custom2Check;
        MMCustom3Check = MainPrefs.Custom3Check;
        MMCustom4Check = MainPrefs.Custom4Check;
        MMCustom5Check = MainPrefs.Custom5Check;
        MMCustom6Check = MainPrefs.Custom6Check;
        MMCustom7Check = MainPrefs.Custom7Check;
        MMCustom8Check = MainPrefs.Custom8Check;
        MMCustom9Check = MainPrefs.Custom9Check;
        MMCustom10Check = MainPrefs.Custom10Check;

        // -----

        MMPlayerCurrency = MainPrefs.PlayerCurrency;

        // -----

        // High Score Check

        // -----

        // 
        if (NewProfile == true)
        {
            NewProfileCanvas.enabled = true;
            NEWConfirm.interactable = false;
        }


    }
    #endregion

    // ________________________________________

    #region UPDATE SECTION

    void Update()
    {
        if (ArtefactMenuCanvas.enabled == true) // Check prices when menu open, options
        {

            if (MMDoubleJumpPrice > MMPlayerCurrency)
            {
                ARTDoubleJump.interactable = false;
            }

            if (MMSwingPrice > MMPlayerCurrency)
            {
                ARTSwing.interactable = false;
            }

            if (MMAttackPrice > MMPlayerCurrency)
            {
                ARTAttack.interactable = false;
            }

            if (MMObjectLowerPrice > MMPlayerCurrency)
            {
                ARTObjectlower.interactable = false;
            }

            if (MMHoverPrice > MMPlayerCurrency)
            {
                ARTHover.interactable = false;
            }

            if (MMEagleEyePrice > MMPlayerCurrency)
            {
                ARTEagleEye.interactable = false;
            }

            if (MMGhostPrice > MMPlayerCurrency)
            {
                ARTGhost.interactable = false;
            }

            if (MMMoneyMagnetPrice > MMPlayerCurrency)
            {
                ARTMoneyMagnet.interactable = false;
            }

        }

        if (CustomMenuCanvas.enabled == true) // Check prices when menu open, custom
        {
            // EnableCustomButtons();
        }

    }
    #endregion

    // ________________________________________

    #region NEW PROFILE FUNCTIONS
    public void BTNNewProfileInput()
    {
        // Takes player username and makes the confirm button avalible

        NEWConfirm.interactable = true;
    }

    public void BTNNewProfileConfirm()
    {
        // Saves player user name and deactivates new profile menu
        PlayerPrefs.Save();
        NewProfileCanvas.enabled = false;
    }

    #endregion

    // ________________________________________

    #region LEVEL START FUNCTIONS

    public void BTNBegin()
    {
        //  Press to start game / initalise the loading screen
    }
    #endregion

    // ________________________________________

    #region QUIT FUNCTIONS Finished

    public void BTNQuit()
    {
        // Press to open the quit sub menu

        Debug.Log("Opening sub quit menu and closing main menu canvas");
        SubQuitCanvas.enabled = true;
        MainmenuCanvas.enabled = false;
    }

    public void BTNYesQuit()
    {
        // Press to close application

        Debug.Log("Shutting down");
        Application.Quit();
    }

    public void BTNNoQuit()
    {
        // Press to close quit sub menu

        Debug.Log("Closing sub quit menu and opening main menu canvas");
        SubQuitCanvas.enabled = false;
        MainmenuCanvas.enabled = true;
    }

    #endregion

    // ________________________________________

    #region ARTEFACT MENU FUNCTIONS

    // JRH v0.4.4:
    int lastActiveArt, lastPassiveArt, activeArt, passiveArt;

    public void BTNOpenBlackMarket()
    {
        // Press to open black market menu

        lastActiveArt = PlayerPrefs.GetInt("ArtAct");
        lastPassiveArt = PlayerPrefs.GetInt("ArtPass");

        activeArt = lastActiveArt;
        passiveArt = lastPassiveArt;

        Debug.Log("Opening artefact menu, closing main menu");
        ArtefactMenuCanvas.enabled = true;
        MainmenuCanvas.enabled = false;
    }

    public void BTNCloseBlackMarket()
    {
        // Press to close black market menu

        Debug.Log("Closing artefact menu, opening main menu");
        ArtefactMenuCanvas.enabled = false;
        MainmenuCanvas.enabled = true;
        SaveSettingsArtefacts.interactable = false;
    }

    public void BTNSaveArtefacts()
    {
        // Press to save player selected artefacts

        Debug.Log("Saving player artefact settings");
        //PlayerPrefs.GetInt("");
        //PlayerPrefs.Save();

        PlayerPrefs.SetInt("ArtAct", activeArt);
        PlayerPrefs.SetInt("ArtPass", passiveArt);

        Debug.Log("disabling save button til a new selection is made");
        SaveSettingsArtefacts.interactable = false;
    }

    public void BTNYesArtefact()
    {
        // Press to confirm purchase

        Debug.Log("Enabling interactive artefact buttons, closing sub artefact menu, adding artefact to player prefs, taking currency away");
        ArtefactConfirmCanvas.enabled = false;
        EnableArtefacts();
    }

    public void BTNNoArtefact()
    {
        // Press to close artefact confirm menu

        Debug.Log("Enabling interactive artefact buttons, closing sub artefact menu");
        ArtefactConfirmCanvas.enabled = false;
        EnableArtefacts();

    }

    #endregion

    // ________________________________________

    #region CUSTOMISE FUNCTIONS

    public void BTNOpenCustom()
    {
        // Press to open player customise menu

        CustomMenuCanvas.enabled = true;
    }

    public void BTNCloseCustom()
    {
        // Press to close customise menu
        CustomMenuCanvas.enabled = false;
    }

    public void BTNSaveCustom()
    {
        // Press to save the player settings
        PlayerPrefs.Save();

        Debug.Log("disabling save button til a new selection is made");
        SaveSettingsCustom.interactable = false;
    }

    public void BTNYesCustom()
    {
        // Press to confirm buy custom
    }

    public void BTNNoCustom()
    {
        // Press to close confirm menu
    }

    #endregion

    // ________________________________________

    #region  OPTION FUNCTIONS Finished

    public void BTNOpenOptions()
    {
        // Press to open option menu

        Debug.Log("Opening options menu, close main menu");
        OptionsMenuCanvas.enabled = true;
        MainmenuCanvas.enabled = false;
    }

    public void BTNCloseOptions()
    {
        // Press to close option menu

        Debug.Log("Close option menu, open main menu");
        OptionsMenuCanvas.enabled = false;
        MainmenuCanvas.enabled = true;
    }

    public void BTNOPTReset()
    {
        // Press to open reset player prefs menu

        Debug.Log("Open sub reset menu, disable option menu buttons");
        DisableOptionButtons();
        SaveSettingsOptions.interactable = false;
        SubResetCanvas.enabled = true;

    }

    public void BTNYesReset()
    {
        // Press to reset player prefs and closes sub reset menu

        Debug.Log("Deleting Player artefact settings");
        PlayerPrefs.DeleteKey("PlayerArtefacts");

        Debug.Log("Deleting Player custom settings");
        PlayerPrefs.DeleteKey("PlayerCustomise");

        Debug.Log("Deleting Player currency settings");
        PlayerPrefs.DeleteKey("PlayerCurrency");

        Debug.Log("Closing sub reset canvas, changes");
        SubResetCanvas.enabled = false;

        Debug.Log("Enabling option buttons");
        EnableOptionButtons();
    }

    public void BTNNoReset()
    {
        // Press to close sub reset menu

        Debug.Log("Closing sub reset canvas, no changes");
        SubResetCanvas.enabled = false;

        Debug.Log("Enabling option buttons");
        EnableOptionButtons();
    }

    public void BTNOPTMusic()
    {
        // Press to toggle ON/OFF Music

        if (MusicToggle.isOn == true)
        {
            Debug.Log("Music on");
            PlayerPrefs.SetInt("Music Toggle", 1);
        }
        else
        {
            Debug.Log("Music off");
            PlayerPrefs.SetInt("Music Toggle", 1);
        }

        Debug.Log("Making save button avalible");
        SaveSettingsOptions.interactable = true;
    }

    public void BTNOPTSfx()
    {
        // Press to toggle ON/OFF SFX

        if (SFXToggle.isOn == true)
        {
            Debug.Log("SFX on");
            PlayerPrefs.SetInt("SFX Toggle", 1);
        }
        else
        {
            Debug.Log("SFX off");
            PlayerPrefs.SetInt("SFX Toggle", 0);
        }

        Debug.Log("Making save button avalible");
        SaveSettingsOptions.interactable = true;
    }

    public void BTNOPTVibration()
    {
        // Press to toggle ON/OFF Vibration
        if (VibrationToggle.isOn == true)
        {
            Debug.Log("Vibration on");
            PlayerPrefs.SetInt("Vibration Toggle", 1);
        }
        else
        {
            Debug.Log("Vibration off");
            PlayerPrefs.SetInt("Vibration Toggle", 0);
        }

        Debug.Log("Making save button avalible");
        SaveSettingsOptions.interactable = true;
    }

    public void BTNSaveOptions()
    {
        // Press to save options
        Debug.Log("Saving Player option settings");
        PlayerPrefs.Save();

        Debug.Log("Making save button unavalible");
        SaveSettingsOptions.interactable = false;
    }

    #endregion

    // ________________________________________

    #region ARTEFACT FUNCTIONS

    public void ARTEFACTDoubleJump()
    {
        Debug.Log("Double jump selected");
        activeArt = activeArt == 5 ? 0 : 5;
        SaveSettingsArtefacts.interactable = true;
    }

    public void ARTEFACTSwing()
    {
        Debug.Log("Swing selected");
        activeArt = activeArt == 2 ? 0 : 2;
        SaveSettingsArtefacts.interactable = true;

    }

    public void ARTEFACTAttack()
    {
        Debug.Log("Attack selected");
        activeArt = activeArt == 1 ? 0 : 1;
        SaveSettingsArtefacts.interactable = true;

    }

    public void ARTEFACTLowerObjects()
    {
        Debug.Log("Lower objects selected");
        activeArt = activeArt == 3 ? 0 : 3;
        SaveSettingsArtefacts.interactable = true;

    }

    public void ARTEFACTHover()
    {
        Debug.Log("Hover selected");
        activeArt = activeArt == 4 ? 0 : 4;
        SaveSettingsArtefacts.interactable = true;

    }

    public void ARTEFACTEagleEye()
    {
        Debug.Log("Eagle eye selected");
        passiveArt = passiveArt == 1 ? 0 : 1;
        SaveSettingsArtefacts.interactable = true;

    }

    public void ARTEFACTGhost()
    {
        Debug.Log("Ghost selected");
        passiveArt = passiveArt == 3 ? 0 : 3;
        SaveSettingsArtefacts.interactable = true;

    }

    public void ARTEFACTMoneyMagnet()
    {
        Debug.Log("Money magnet selected");
        passiveArt = passiveArt == 2 ? 0 : 2;
        SaveSettingsArtefacts.interactable = true;

    }

    #endregion

    // ________________________________________

    #region CUSTOM FUNCTIONS

    public void Cosmetic1()
    {
        Debug.Log("Cosmetic 1 selected");
    }

    public void Cosmetic2()
    {
        Debug.Log("Cosmetic 2 selected");
    }

    public void Cosmetic3()
    {
        Debug.Log("Cosmetic 3 selected");
    }

    public void Cosmetic4()
    {
        Debug.Log("Cosmetic 4 selected");
    }

    public void Cosmetic5()
    {
        Debug.Log("Cosmetic 5 selected");
    }

    public void Cosmetic6()
    {
        Debug.Log("Cosmetic 6 selected");
    }

    public void Cosmetic7()
    {
        Debug.Log("Cosmetic 7 selected");
    }

    public void Cosmetic8()
    {
        Debug.Log("Cosmetic 8 selected");
    }

    public void Cosmetic9()
    {
        Debug.Log("Cosmetic 9 selected");
    }

    public void Cosmetic10()
    {
        Debug.Log("Cosmetic 10 selected");
    }

    #endregion

    // ________________________________________

    #region ARTEFACT BUTTON ACTIVATION AND DEACTIVATION FUNCTIONS finished

    public void EnableArtefacts()
    {
        ARTDoubleJump.interactable = true;
        ARTSwing.interactable = true;
        ARTAttack.interactable = true;
        ARTObjectlower.interactable = true;
        ARTHover.interactable = true;
        ARTEagleEye.interactable = true;
        ARTGhost.interactable = true;
        ARTMoneyMagnet.interactable = true;
        // -----
        CloseArtefact.interactable = true;
    }

    public void DisableArtefacts()
    {
        ARTDoubleJump.interactable = false;
        ARTSwing.interactable = false;
        ARTAttack.interactable = false;
        ARTObjectlower.interactable = false;
        ARTHover.interactable = false;
        ARTEagleEye.interactable = false;
        ARTGhost.interactable = false;
        ARTMoneyMagnet.interactable = false;
        // -----
        CloseArtefact.interactable = false;
    }


    #endregion

    // ________________________________________

    #region CUSTOMISE BUTTON ACTIVATION AND DEACTIVATION FUNCTIONS

    public void EnableCustomButtons()
    {
        CUS1.interactable = true;
        CUS2.interactable = true;
        CUS3.interactable = true;
        CUS4.interactable = true;
        CUS5.interactable = true;
        CUS6.interactable = true;
        CUS7.interactable = true;
        CUS8.interactable = true;
        CUS9.interactable = true;
        CUS10.interactable = true;
    }

    public void DisableCustomButtons()
    {
        CUS1.interactable = false;
        CUS2.interactable = false;
        CUS3.interactable = false;
        CUS4.interactable = false;
        CUS5.interactable = false;
        CUS6.interactable = false;
        CUS7.interactable = false;
        CUS8.interactable = false;
        CUS9.interactable = false;
        CUS10.interactable = false;
    }
    #endregion

    // ________________________________________

    #region OPTIONS BUTTON ACTIVATION AND DEACTIVATION FUNCTIONS Finished

    public void EnableOptionButtons()
    {
        PlayerReset.interactable = true;
        CloseOptions.interactable = true;
        MusicToggle.interactable = true;
        SFXToggle.interactable = true;
        VibrationToggle.interactable = true;

        //SaveSettingsOptions.interactable = true;
    }

    public void DisableOptionButtons()
    {
        PlayerReset.interactable = false;
        CloseOptions.interactable = false;
        MusicToggle.interactable = false;
        SFXToggle.interactable = false;
        VibrationToggle.interactable = false;

        //SaveSettingsOptions.interactable = false;
    }

    #endregion

    // ________________________________________

    #region HIGHSCORE GENERATION

    public void BTNOpenHighScore()
    {
        HighScoreCanvas.enabled = true;

        if (PlayerPrefs.HasKey("HighScore"))
        {
            HighscoreText.text = PlayerPrefs.GetInt("HighScore").ToString();
        }
    } 

    public void BTNCloseHighScore()
    {
        HighScoreCanvas.enabled = false;
    }

    #endregion
}