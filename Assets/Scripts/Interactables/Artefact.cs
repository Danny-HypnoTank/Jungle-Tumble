using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Artefact : Reward
{
    #region VARIABLES
    [SerializeField]
    Renderer render;
    [SerializeField]
    ArtefactType artType;
    [SerializeField]
    Color artColor;
    [SerializeField]
    bool isActivePower;
    [SerializeField]
    float cooldown;
    [SerializeField]
    Sprite enabledIcon, disabledIcon;

    bool isInUse = false;
    #endregion

    #region PROPERTIES
    public ArtefactType ArtType { get { return artType; } }
    public bool IsActivePower { get { return isActivePower; } }
    public Color ArtColor { get { return artColor; } }
    public bool IsInUse { get { return isInUse; } }

    public Sprite EnabledIcon { get { return enabledIcon; } }
    public Sprite DisabledIcon { get { return disabledIcon; } }
    #endregion

    #region METHODS
    public void Reset()
    {
        isInUse = false;
    }

    public override void Interact()
    {
        AudioManager.Instance.PlaySFX("RewardSFX");
        AudioManager.Instance.PlaySFX("BigRewardSFX");



        StopAllCoroutines();
        StartCoroutine(HoverOverPlayer());
    }

    public void UseArtefact()
    {
        if (artType == ArtefactType.Null) return;

        GameManager.Instance.PlayerScript.StartCoroutine(GameManager.Instance.PlayerScript.FlashColor(artColor));
        AudioManager.Instance.PlaySFX("ArtefactSFX");
        if (IsActivePower) GameManager.Instance.StartCoroutine(RunCooldown());

        //todo: this shit
        switch (artType)
        {
            case ArtefactType.Attack:
                Debug.LogWarning("ATTIC USED");
                foreach (DestructableObstacle obs in FindObjectsOfType<DestructableObstacle>()) obs.DestroyByArtefact();
                break;
            case ArtefactType.Eye:
                Debug.Log("AYE USED");
                GameObject.Find("Main Camera").transform.position = new Vector3(0, 15, -15);
                GameObject.Find("Main Camera").transform.rotation = Quaternion.Euler(45, 0, 0);
                break;
            case ArtefactType.Ghost:
                Debug.LogWarning("GOATS USED");
                GameManager.Instance.ActivateGhost();
                break;
            case ArtefactType.Hover: Debug.LogWarning("ARTICHOKE USED"); break;
            case ArtefactType.Jump:
                Debug.LogWarning("JAMS USED");
                if (GameManager.Instance.PlayerScript.State == PlayerState.Falling)
                {
                    GameManager.Instance.PlayerScript.RB.velocity = Vector3.zero;
                    GameManager.Instance.PlayerScript.RB.AddForce(GameManager.Instance.PlayerScript.jumpVelocity / 1.25f, ForceMode.VelocityChange);
                }
                break;
            case ArtefactType.Magnet:
                Debug.LogWarning("MUGNET USED");
                GameManager.Instance.PlayerScript.transform.GetChild(1).gameObject.SetActive(true);
                break;
            case ArtefactType.Swing: Debug.LogWarning("ARTICHOKE USED"); break;
            case ArtefactType.Twist:
                Debug.LogWarning("TWINKS USED");
                foreach (DestructableObstacle obs in FindObjectsOfType<DestructableObstacle>()) obs.DestroyByArtefact();
                break;
        }
    }


    public IEnumerator RunCooldown()
    {
        float t = 0;

        isInUse = true;
        GameManager.Instance.UIManager.UpdateArtefactUISprites();

        while (t < cooldown)
        {
            t += Time.deltaTime;
            yield return null;
        }

        isInUse = false;
        GameManager.Instance.UIManager.UpdateArtefactUISprites();
        yield return null;
    }
    #endregion

    #region MONOBEHAVIOUR
    private void Awake()
    {
        type = RewardType.ART;
        artColor = render.material.GetColor("_OutlineColor");
    }
    #endregion
}

public enum ArtefactType
{
    Null = 0, Attack, Swing, Twist, Hover, Eye, Magnet, Ghost, Jump
}