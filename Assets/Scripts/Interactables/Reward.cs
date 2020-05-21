using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward : Interactable
{
    [SerializeField]
    protected RewardType type;

    Vector3 initRot;
    Transform mesh;

    public RewardType Type { get { return type; } }

    // JRH v0.1.9:
    /// <summary>
    /// Called by the player on collision, triggers the end state when the player fucks up and hits the reward
    /// </summary>
    public override void Interact()
    {
        string sfx = type == RewardType.GEM ? "BigRewardSFX" : "RewardSFX"; // JRH v0.2.3: Set the audio cue lookup as appropriate to the type of reward
        AudioManager.Instance.PlaySFX(sfx); // JRH V0.2.3: Play the audio 
        //GameObject.Find("ButtonClick").GetComponent<AudioSource>().Play();    // JRH v0.1.9: Plays audio feedback to indicate the player did a good! good job! congratulations!
        GameManager.Instance.ScoreManager.IncrementReward(type);    // JRH v0.1.9: Call the ScoreManager's IncrementReward method passing in the type of reward the player just got
        //gameObject.SetActive(false);    // JRH v0.1.9: Consume the object
        StopAllCoroutines();
        StartCoroutine(HoverOverPlayer());
    }

    // EH - Basic set up to attach onto reward prefabs / the prefab bubble if the money magnet artefact is active. The rewards then start to move towards player

    IEnumerator Attract(Transform target)
    {
        transform.SetParent(target);
        float t = 0;
        Vector3 initPos = transform.position;
        //GameManager.Instance.PlayerScript.StartCoroutine(GameManager.Instance.PlayerScript.FlashColor(GameManager.Instance.PassiveArtefact.ArtColor));

        while (transform.IsChildOf(gameObject.transform))
        {
            t += Time.deltaTime * GameManager.Instance.GameSpeed;
            transform.position = Vector3.Lerp(initPos, target.position, t);
            yield return null;
        }

        yield break;
    }

    private void MoneyMagnetCheck()
    {
        // If player pref for MM = true then activate bubble.

        // when bubble active, rewards within set distance will move towards player and tick score up properly 

        // potential code:

           // void RewardRadius (Vector3 center, float radius)
           //{
             //Collider[] HitCollider = Physics.OverlapShpere(center, radius);
             // int i = 0;
             // while (i < HitCollider.Length)
             //{
             // ProfitReward.transform.position = Vector3.Lerp(ProfitReward.transform.position,PlayerPull.position,Time.deltaTime * speed);
             //i++;
             //}
           //}

    }

    private void Awake()
    {
        mesh = transform.GetChild(0);
        initRot = mesh.transform.rotation.eulerAngles;
        StartCoroutine(Spin());
    }

    protected IEnumerator Spin()
    {
        while (true)
        {
            mesh.Rotate(0, -90 * Time.deltaTime, 0);
            yield return new WaitForEndOfFrame();
        }
    }

    protected IEnumerator HoverOverPlayer()
    {
        mesh.rotation = Quaternion.Euler(initRot);
        mesh.transform.SetParent(GameManager.Instance.PlayerScript.gameObject.transform);
        mesh.Translate(Vector3.up * 3);

        yield return new WaitForSeconds(2 / GameManager.Instance.GameSpeed);

        Destroy(mesh.gameObject);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Magnet" && (type == RewardType.COIN || type == RewardType.BAG)) StartCoroutine(Attract(other.transform));
    }
}


public enum RewardType { COIN, BAG, GEM, ART }