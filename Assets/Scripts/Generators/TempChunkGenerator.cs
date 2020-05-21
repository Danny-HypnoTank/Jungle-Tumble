using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempChunkGenerator : MonoBehaviour
{
    #region VARIABLES

    #region PRIVATE
    [SerializeField]
    List<PlatformChunk> chunkLib;

    [SerializeField]
    GameObject platformPrefab;

    Transform parentTransform { get { return GameObject.Find("Platforms").transform; } }

    PlatformRow lastRow;
    #endregion

    #region PUBLIC
    #endregion

    #endregion

    #region METHODS

    #region CONSTRUCTOR
    #endregion

    #region USER-DEFINED
    /*
    void GenerateRow(PlatformCode code)
    {
        GameObject rowObj = new GameObject("Platform Row #");   // JRH v0.1.2: Creates the initial object
        rowObj.transform.SetParent(parentTransform);   // JRH v0.1.2: Attaches the just created row object to the parent transform
        PlatformRow row = rowObj.AddComponent<PlatformRow>();   // JRH v0.1.2: Adds a fresh PlatformRow script to the row object
        Platform[] platforms = new Platform[3];   // JRH v0.1.2: Used to hold the platforms the following loop creates so they can be passed through Initialize

        for (int i = 0; i < 3; i++)
        {
            GameObject platformObj = Instantiate(platformPrefab);
            platformObj.transform.SetParent(rowObj.transform);
            platformObj.transform.Translate(new Vector3((i - 1) * 2.5f, 0f, 0f));
            platforms[i] = platformObj.AddComponent<Platform>();
        }

        row.Initialize(platforms[0], platforms[1], platforms[2]);

        if (lastRow != null)   // JRH v0.1.2: Move the platform row into the appropriate position if platforms already exist
        {
            row.transform.position = new Vector3(0f, 0f, lastRow.transform.position.z - 2.5f);
        }
        lastRow = row;

        row.gameObject.name += row.transform.GetSiblingIndex();

        if (code < 0 && lastRow != null) RollNewCode(ref code);    // JRH v0.1.2: Roll a random code that satisfies critical path criteria if one has not already been designated

        for (int i = 0; i < 3; i++) row.Platforms[i].SetFloor(DecipherCode(code)[i]);    // JRH v0.1.2: Set the platforms enabled status as determined by the code
    }

    public void RollNewCode(ref PlatformCode code)
    {
        do    // JRH v0.1.2: Run this loop while PlatformCode is set to the default value, -1
        {
            PlatformCode random = (PlatformCode)UnityEngine.Random.Range(0, Enum.GetNames(typeof(PlatformCode)).Length);    // JRH v0.1.2: Select a random potential platform type

            bool[] suggestion = DecipherCode(random);

            int critLaneCount = 0;

            try
            {
                for (int i = 0; i < 3; i++) if (suggestion[i] && lastRow.Platforms[i].IsCritical) critLaneCount++; // JRH v0.1.2:
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                Debug.Log(lastRow.gameObject.name);
            }

            if (critLaneCount > 0) code = random;    // JRH v0.1.2: If there is at least one viable critical path, select this platform
        }
        while (code < 0);
    }

    public void RollNewCode(ref PlatformCode code, List<PlatformCode> codeLib)
    {
        bool flag = false;
        do
        {
            int random = UnityEngine.Random.Range(0, codeLib.Count);

            bool[] suggestion = DecipherCode(codeLib[random]);

            int critLaneCount = 1;

            if (critLaneCount > 0)
            {
                flag = true;
                code = codeLib[random];
            }
        }
        while (flag != true);
    }

    public bool[] DecipherCode(PlatformCode code)
    {
        switch (code)
        {
            case PlatformCode.PPP: return new bool[3] { true, true, true };
            case PlatformCode.XPP: return new bool[3] { false, true, true };
            case PlatformCode.PXP: return new bool[3] { true, false, true };
            case PlatformCode.PPX: return new bool[3] { true, true, false };
            case PlatformCode.XXP: return new bool[3] { false, false, true };
            case PlatformCode.XPX: return new bool[3] { false, true, false };
            case PlatformCode.PXX: return new bool[3] { true, false, false };
            case PlatformCode.XXX: return new bool[3] { false, false, false };
            default: return null;
        }
    }

    IEnumerator WaitAndLoad()
    {
        Debug.Log("Waiting...");
        yield return new WaitForSeconds(1);
        foreach (PlatformChunk c in chunkLib) foreach (PlatformCode code in c.RowCodes) GenerateRow(code);
    }
    */
    #endregion

    #region MONOBEHAVIOUR
        /*
    void Start()
    {
        foreach (PlatformChunk c in chunkLib) foreach (PlatformCode code in c.RowCodes) GenerateRow(code);
    }*/
    #endregion

    #endregion
}
