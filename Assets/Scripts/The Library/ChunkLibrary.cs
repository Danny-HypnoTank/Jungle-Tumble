using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkLibrary : MonoBehaviour
{
    #region VARIABLES
    [SerializeField]
    PlatformChunk tutorial;

    [SerializeField]
    PlatformChunk genericChunk, genericShort; // JRH v0.1.9;

    [SerializeField]
    PlatformChunk potHoleMix, potHolePit, potHoleObs;    // JRH v0.1.10

    [SerializeField]
    PlatformChunk potHoleTall, potHoleHard;    // JRH v0.2.9

    [SerializeField]
    PlatformChunk zebra, zebraShort, zebraWide, zebraLong, zebraHard; // JRH v0.2.0

    [SerializeField]
    PlatformChunk mon, monEasy, monLong;

    Dictionary<string, PlatformChunk> chunkDictionary;    // JRH v0.1.10
    #endregion

    #region PROPERTIES
    public Dictionary<string, PlatformChunk> ChunkDictionary { get { return chunkDictionary; } }    // JRH v0.2.0
    #endregion

    #region METHODS
    // JRH v0.1.10:
    /// <summary>
    /// Called by PlatformManager, returns a Chunk of RowCodeEntry based on a string key (If no match is found, returns the Generic Chunk)
    /// </summary>
    /// <param name="lookup"></param>
    /// <returns></returns>
    public List<RowData> NextChunk(string lookup = "Random")
    {
        List<RowData> returnedChunk;

        // JRH v0.2.0: Set ReturnedChunk based on the parameter
        if (lookup == "Random") returnedChunk = RollNextChunk();
        // JRH v0.2.0: If no string was passed in, roll a random chunk
        else if (chunkDictionary.ContainsKey(lookup)) returnedChunk = chunkDictionary[lookup].LoadRows();
        // JRH v0.2.0: If a string was passed in and it matches a key, return the LoadRows method from that Chunk
        else returnedChunk = chunkDictionary["Generic"].LoadRows();
        // JRH v0.2.0: If a string was passed in and it doesn't match a key, load a generic chunk in as a substitute

        return returnedChunk;
    }

    //TODO: Complete RollNextChunk
    List<RowData> RollNextChunk()
    {
        List<string> dictionaryKeys = new List<string>();
        List<RowData> roll = null;

        foreach (string key in chunkDictionary.Keys) dictionaryKeys.Add(key);
        // JRH v0.2.0: Load up the DictionaryKeys list with all the keys included in the dictionary

        do
        {
            PlatformChunk ranChunk;
            string ranLookup;

            // JRH v0.3.1: Remove Undesirable Chunks from Pool;
            if (dictionaryKeys.Contains("Tutorial")) dictionaryKeys.Remove("Tutorial");
            if (dictionaryKeys.Contains("Generic")) dictionaryKeys.Remove("Generic");

            ranLookup = dictionaryKeys[Random.Range(0, dictionaryKeys.Count)];
            ranChunk = chunkDictionary[ranLookup];

            if (ranChunk.Difficulty == GameManager.Instance.Level)
                roll = ranChunk.LoadRows();
            else dictionaryKeys.Remove(ranLookup);

        } while (roll == null && dictionaryKeys.Count > 0);
        // JRH v0.2.0: Set the roll to the chunk returned by a random string remaining in DictionaryKeys

        return roll;
    }
    #endregion

    #region MONOBEHAVIOUR
    private void Awake()
    {
        chunkDictionary = new Dictionary<string, PlatformChunk>     // JRH v0.1.10: Loads the ChunkDictionary as soon as the game begins because I can't do it during declaration wtf
        {
            { "Tutorial", tutorial },
            //{ "Generic", genericChunk },
            { "Generic Short", genericShort },
            { "Pothole Mixed", potHoleMix },
            { "Pothole Pits", potHolePit },
            { "Pothole Obstacles", potHoleObs },
            { "Pothole Tall Obstacles", potHoleTall },
            { "Pothole Hard", potHoleHard },
            { "Zebra Standard", zebra },
            { "Zebra Short" , zebraShort },
            //{ "Zebra Wide", zebraWide },
            { "Zebra Long", zebraLong },
            { "Zebra Hard", zebraHard },
            {"Monument", mon },
            {"Monument Long", monLong },
            {"Monument Easy", monEasy }, 
        };
    }
    #endregion
}