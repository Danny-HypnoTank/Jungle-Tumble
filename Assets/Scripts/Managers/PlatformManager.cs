using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    #region VARIABLES
    [SerializeField]
    List<PlatformChunk> chunkLib;

    [SerializeField]
    List<GameObject> Terrain;

    List<PlatformRow> platformRows = new List<PlatformRow>();
    List<RowData> rowDataQueue = new List<RowData>();
    #endregion

    #region PROPERTIES
    public List<RowData> RowDataQueue { get { return rowDataQueue; } }

    public PlatformRow LastRow { get { if (platformRows.Count > 0) return platformRows[platformRows.Count - 1]; else return null; } }
    public RowData NextRowData { get { if (rowDataQueue.Count > 0) return rowDataQueue[0]; else return new RowData('p', 'p', 'p'); } }
    #endregion

    #region METHODS
    // JRH v0.1.1
    /// <summary>
    /// Moves all of the platforms in the scene backwards by the amount based on GameSpeed
    /// </summary>
    void Treadmill()
    {
        foreach (PlatformRow p in platformRows)
        {
            p.transform.position += Vector3.forward * Time.deltaTime * GameManager.Instance.GameSpeed;
        }

        foreach (GameObject t in Terrain)
        {
            t.transform.position += Vector3.forward * Time.deltaTime * GameManager.Instance.GameSpeed;
            if (t.transform.position.z > 120f) t.transform.Translate(0, 0, -160);
        }
    }

    // JRH v0.1.10 - Updated to function with rowCodeQueue
    /// <summary>
    /// Adds new platforms totalling up to MaxPlatformRows if there are any less than that amount in the scene
    /// </summary>
    void AddNewPlatforms()
    {
        if (platformRows.Count < GameManager.Instance.MaxPlatformRows)
        {
            while (platformRows.Count < GameManager.Instance.MaxPlatformRows)
            {
                // JRH v0.1.10: If the code queue has run dry, roll a new set of codes from the Chunk Library
                if (rowDataQueue.Count <= 0)
                {
                    foreach (RowData data in GameManager.Instance.ChunkLibrary.NextChunk()) rowDataQueue.Add(data);
                }

                platformRows.Add(GameManager.Instance.PlatformGenerator.GenerateRow(NextRowData));
                // JRH v0.1.10: Instantiate a new row based on the code inside the next RowData
                rowDataQueue.Remove(NextRowData);
                // JRH v0.2.0: Remove the used RowData from the queue
            }
        }
    }

    // JRH v0.1.2
    /// <summary>
    /// Deletes platform rows in the scene if there are any less than 
    /// </summary>
    void DeleteOldPlatforms()
    {
        bool updated = false;
        List<PlatformRow> toDelete = new List<PlatformRow>();

        foreach (PlatformRow row in platformRows)
            if (row.transform.position.z > GameManager.Instance.MaxPlatformDistance)
            {
                toDelete.Add(row);
                if (updated != true) updated = true;
            }

        foreach (PlatformRow row in toDelete)
        {
            platformRows.Remove(row);
            Destroy(row.gameObject);
        }

        if (updated)
            foreach (PlatformRow p in platformRows)
                p.name = "Platform Row #" + platformRows.IndexOf(p);
    }
    #endregion

    #region MONOBEHAVIOUR
    void Start()    // JRH v0.1.2: DO NOT SWITCH TO AWAKE - WILL CAUSE A NULL REFERENCE EXCEPTION AND DISABLING OF THE SCRIPT
    {
        for (int i = 0; i < GameManager.Instance.MaxPlatformRows; i++) rowDataQueue.Add(new RowData('p', 'p', 'p'));
        rowDataQueue.AddRange(GameManager.Instance.ChunkLibrary.NextChunk("Tutorial"));
        // JRH v0.1.10: Sets the initial platforms using RowCodeEntry
        AddNewPlatforms();
        // JRH v0.1.10: Add the first lot of RowData into the Queue
        foreach (PlatformRow r in platformRows) r.transform.Translate(new Vector3(0f, 0f, GameManager.Instance.MaxPlatformDistance));
        // JRH v0.1.0: Move the initial rows back to a suitable position where the player and boulder both have some grounding
    }

    void Update()
    {
        AddNewPlatforms();    // JRH v0.1.2
        if (GameManager.Instance.Running) Treadmill();    // JRH v0.1.2
        DeleteOldPlatforms();    // JRH v0.1.2
    }
    #endregion
}