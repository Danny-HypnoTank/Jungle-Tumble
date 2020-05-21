using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtefactInitializer : MonoBehaviour
{
    [SerializeField]List<Artefact> passiveList, activeList;
    Artefact passive, active;

    void Start()
    {
        int passiveInt = PlayerPrefs.GetInt("ArtPass");
        int activeInt = PlayerPrefs.GetInt("ArtAct");

        passive = passiveList[passiveInt];
        active = activeList[activeInt];

        if (passive != null) GameManager.Instance.SetArtefacts(passive);
        if (active != null) GameManager.Instance.SetArtefacts(active);

        active.Reset();
        passive.Reset();

        GameManager.Instance.UIManager.UpdateArtefactUISprites();

        Destroy(gameObject);
    }
}
