using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObstacle : Obstacle
{
    [SerializeField]
    DestructableType type;

    Renderer render { get { return transform.GetChild(0).GetComponent<Renderer>(); } }

    public DestructableType DestrType { get { return type; } }

    public override void Interact()
    {
        // todo: Deal with interact also triggering on contact with the attack artefact
        if (CheckCondition()) Destroy(); else base.Interact();
    }

    public bool CheckCondition()
    {
        switch (type)
        {
            case DestructableType.Vase:
                return GameManager.Instance.PlayerScript.State == PlayerState.Sliding ? true : false;
            case DestructableType.Smashy:
                if (GameManager.Instance.ActiveArtefact != null) return GameManager.Instance.ActiveArtefact.ArtType == ArtefactType.Attack ? true : false; else return false;
            case DestructableType.Twisty:
                if (GameManager.Instance.ActiveArtefact != null) return GameManager.Instance.ActiveArtefact.ArtType == ArtefactType.Twist ? true : false; else return false;
            default: return false;
        }
    }

    void Destroy()
    {
        switch (type)
        {
            case DestructableType.Vase:
            case DestructableType.Smashy:
                AudioManager.Instance.PlaySFX(type == DestructableType.Vase ? "CrackSFX" : "SmashSFX");

                Destroy(transform.GetChild(0).gameObject);

                transform.GetChild(1).gameObject.SetActive(true);

                foreach (Transform shard in transform.GetChild(1))
                {
                    shard.SetParent(transform.parent);
                    shard.gameObject.AddComponent<Rigidbody>();
                    shard.gameObject.AddComponent<MeshCollider>();
                    shard.gameObject.GetComponent<MeshCollider>().convex = true;
                    Physics.IgnoreCollision(shard.GetComponent<Collider>(), GameManager.Instance.PlayerScript.GetComponent<Collider>());
                    shard.GetComponent<Rigidbody>().AddForce(Vector3.Normalize(shard.transform.position - transform.position) * 10f, ForceMode.VelocityChange);
                }

                float prize = Random.value;
                if (prize > 0.4)
                {
                    GameObject newRew = GameManager.Instance.PlatformGenerator.GenerateReward(transform, prize > 0.9f ? 3 : 1).gameObject;
                }

                Destroy(gameObject);

                break;
        }
    }

    public void DestroyByArtefact()
    {
        switch (type)
        {
            case DestructableType.Vase:
            case DestructableType.Smashy: if (GameManager.Instance.ActiveArtefact.ArtType == ArtefactType.Attack) StartCoroutine(Smash()); break;
            case DestructableType.Twisty: if (CheckCondition()) StartCoroutine(Twist()); break;
        }
    }

    IEnumerator Smash()
    {
        Vector3 delta = transform.position - GameManager.Instance.PlayerScript.transform.position;
        if (delta.magnitude < 5f) Destroy();
        else Debug.Log(delta.magnitude);
        yield return null;
    }

    IEnumerator Twist()
    {
        float t = 0;
        while (t < 1)
        {
            transform.Rotate(0f, Time.deltaTime * 360 * 3, 0f);
            transform.Translate(0f, Time.deltaTime * -4.33f * 3, 0f);
            yield return null;
            t += Time.deltaTime * 3;
        }
    }

    void Awake()
    {
        if (GameManager.Instance.ActiveArtefact != null && CheckCondition() && type != DestructableType.Vase)
        {
            render.material.SetColor("_OutlineColor", GameManager.Instance.ActiveArtefact.ArtColor);
            render.material.SetFloat("_Outline", render.material.GetFloat("_Outline") * 5);
        }
    }
}

public enum DestructableType
{
    Vase, Twisty, Smashy
}
