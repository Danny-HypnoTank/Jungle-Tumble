using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlatformChunk : MonoBehaviour
{
    #region VARIABLES
    protected List<RowData> rowCodes;    // JRH v0.1.10
    
    protected int length;
    [SerializeField]
    protected uint difficultyReq;
    #endregion

    #region PROPERTIES
    public int Length { get { return length; } }
    public uint Difficulty { get { return difficultyReq; } }
    #endregion

    #region METHODS
    // JRH v0.2.0:
    /// <summary>
    /// Allows an outside script or object to access the RowCodes produced by the chunk
    /// </summary>
    /// <returns></returns>
    public List<RowData> LoadRows()
    {
        rowCodes = GenerateRows();
        SetCritical();
        AddRewards();
        return rowCodes;
    }

    // JRH v0.2.0
    /// <summary>
    /// Returns a new list of RowData based on the overriden method in a child script and it's parameters
    /// </summary>
    /// <returns></returns>
    protected virtual List<RowData> GenerateRows()
    {
        rowCodes = new List<RowData>();
        return null;
    }

    protected virtual void SetCritical()
    {
        for (int i = 0; i < length; i++)
        {
            if (rowCodes[i].CritCheck(i == 0 ? null : (RowData?)rowCodes[i - 1]) != true) rowCodes[i] = new RowData('p', 'p', 'p');
        }
    }

    protected virtual void AddRewards()
    {

    }
    #endregion

    #region MONOBEHAVIOUR

    #endregion
}

public enum ChunkLength { Regular = 2, Short = 1, Long = 3 }