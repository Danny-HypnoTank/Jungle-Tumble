using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : MonoBehaviour
{
    float initialZ;
    Quaternion initialRot;

    bool rolling { get { return GameManager.Instance.Running; } }

    // JRH v0.1.2

    float time;
    float rotModifier { get { return GameManager.Instance.GameSpeed * Mathf.Rad2Deg / transform.localScale.z; } }

    // JRH v0.3.9
    int textureIndex;
    [SerializeField]
    List<Texture> textures;

    private void Start()
    {
        transform.position = (GetComponent<SphereCollider>().radius * Vector3.forward * 5) + (Vector3.forward) + (Vector3.up * GetComponent<SphereCollider>().radius);

        initialZ = transform.position.z;
        initialRot = transform.rotation;

        StartCoroutine(Roll());
    }

    IEnumerator Roll()
    {
        while (true)
        {
            transform.Rotate(Time.deltaTime * rotModifier, 0, 0);
            yield return null;
        }
    }

    public void RollForward()
    {
        textureIndex++;
        if (textureIndex < textures.Count) GetComponent<Renderer>().material.mainTexture = textures[textureIndex];
        StartCoroutine(RollForwardCoroutine());
    }

    public void RollForever()
    {
        StartCoroutine(RollForeverCoroutine());
    }

    IEnumerator RollForwardCoroutine()
    {
        float t = 0;
        float zI, zF;

        zI = transform.position.z;
        zF = transform.position.z - (GetComponent<SphereCollider>().radius * 2);

        while (t < GameManager.Instance.RecoveryTime)
        {
            t += Time.deltaTime;
            transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Lerp(zI, zF, t / GameManager.Instance.RecoveryTime));
            yield return null;
        }
    }

    IEnumerator RollForeverCoroutine()
    {
        while (true)
        {
            transform.Translate(0, 0, -(GameManager.Instance.GameSpeed) * Time.deltaTime, Space.World);
            yield return null;
        }
    }
}
