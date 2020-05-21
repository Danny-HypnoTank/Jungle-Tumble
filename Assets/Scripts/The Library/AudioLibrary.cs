using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioLibrary : MonoBehaviour
{
    #region VARIABLES
    Dictionary<string, AudioClip> audioDictionary;

    [Header("BGM Library")]
    [SerializeField]
    AudioClip menuBGM;
    [SerializeField]
    AudioClip gameBGM, loadBGM;

    [Header("SFX Library")]
    [SerializeField]
    AudioClip genericSFX;
    [SerializeField]
    AudioClip buttonYesSFX, buttonNoSFX, pauseSFX, unpauseSFX, jumpSFX, landSFX, moveSFX, slideSFX, artefactSFX, rewardSFX, bigRewardSFX, damageSFX, crackSFX, smashSFX, twistSFX, gameOverSFX;
    #endregion

    #region METHODS
    public AudioClip LookupAudioClip(string index)
    {
        AudioClip clip = null;

        if (audioDictionary.ContainsKey(index)) clip = audioDictionary[index];

        return clip;
    }
    #endregion

    #region MONOBEHAVIOUR
    private void Awake()
    {
        audioDictionary = new Dictionary<string, AudioClip>
        {
            { "GameBGM", gameBGM },
            { "MenuBGM", menuBGM },
            { "ButtonYesSFX", buttonYesSFX },
            { "ButtonNoSFX", buttonNoSFX },
            { "PauseSFX", pauseSFX },
            { "UnpauseSFX", unpauseSFX },
            { "JumpSFX", jumpSFX },
            { "LandSFX", landSFX },
            { "MoveSFX", moveSFX},
            { "SlideSFX", slideSFX},
            {"ArtefactSFX", artefactSFX },
            { "RewardSFX", rewardSFX },
            { "BigRewardSFX", bigRewardSFX },
            { "DamageSFX", damageSFX },
            { "CrackSFX", crackSFX },
            { "SmashSFX", smashSFX },
            { "TwistSFX", twistSFX },
            { "GameOverSFX", gameOverSFX },
        };
    }
    #endregion
}
