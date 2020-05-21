using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotHoleChunk : PlatformChunk
{
    #region VARIABLES
    int obstacleCount;    // JRH v0.2.0: 

    [SerializeField]
    PotHoleOptions potholeOption;    // JRH v0.2.0: 

    [SerializeField]
    ChunkLength lengthOption;    // JRH v0.2.0: 

    [SerializeField]
    bool hardMode;    // JRH v0.2.5

    int lastLane = -1;

    char obsCode
    {
        get
        {
            switch (potholeOption)
            {
                case PotHoleOptions.Obstacle: return 'o';
                case PotHoleOptions.Pit: return 'x';
                case PotHoleOptions.TallObstacle: return 't';
                default: return ObstacleLibrary.RandomCode();
            }
        }
    }
    #endregion

    #region PROPERTIES
    #endregion

    #region METHODS

    protected override List<RowData> GenerateRows()
    {
        base.GenerateRows();    // JRH v0.2.0: Call to reset the list of rows

        List<int> obstacleRows = new List<int>();

        switch (lengthOption) { case ChunkLength.Long: length = 11; break; default: length = 7; break; }
        // JRH v0.2.0: Set the length to 7, unless the chunk length option specifies Long

        obstacleCount = Random.Range(length / 3, length + 1);
        // JRH v0.2.0: Roll a random number of total obstacles to be generated over the rows

        while (obstacleRows.Count < obstacleCount)
        {
            int index = Random.Range(0, length);
            if (!obstacleRows.Contains(index)) obstacleRows.Add(index);
            // JRH v0.2.0: Load up ObstacleRows with a random row to put an obstacle in for every value inside obstacleCount
        }

        for (int i = 0; i < length; i++)
        {
            // JRH v0.2.0: For each row the chunk is meant to generate, initialise and manipulate the code to have three platforms / two platforms and a danger

            char[] codes = new char[] { 'p', 'p', 'p' };
            // JRH v0.2.0: Initially sets code to return a default row, all platforms

            if ((i + 2) % 2 == 1)
            {
                int maxObsCount = hardMode ? 3 : 2, minObsCount = hardMode ? 2 : 1;
                // JRH v0.2.9: Set the minimum and maximum number of obstacles on this row based on the difficulty
                int obsCount = Random.Range(minObsCount, maxObsCount);
                // JRH v0.2.9: Determine how many obstacles are going into this row
                List<int> lanes = new List<int>();
                while (lanes.Count < obsCount)
                {
                    int ran = Random.Range(0, 3);
                    if (!lanes.Contains(ran) && ran != lastLane)
                    {
                        lanes.Add(ran);
                        lastLane = ran;
                    }
                }
                foreach (int lane in lanes) codes[lane] = obsCode;
            }

            rowCodes.Add(new RowData(codes[0], codes[1], codes[2]));
            // JRH v0.2.0: Finally, add a new RowData to RowCodes using the now manipulated code
        }

        return rowCodes;
    }

    protected override void AddRewards()
    {
        int rewardCount = lengthOption == ChunkLength.Long ? 3 : 2;

        // JRH v0.2.5: Determine which rows the rewards get put on
        List<int> rows = new List<int>();
        while (rows.Count < rewardCount)
        {
            int ran = Random.Range(0, rowCodes.Count);
            if (!rows.Contains(ran)) rows.Add(ran);
        }

        // JRH v0.2.5: For each row determined before, add a reward
        foreach (int row in rows)
        {
            int ran = Random.Range(0, 3);
            rowCodes[row].CodeArray[ran] = char.ToUpper(rowCodes[row].CodeArray[ran]);
        }

    }
    #endregion

    #region MONOBEHAVIOUR
    #endregion
}

public enum PotHoleOptions { Pit, Obstacle, TallObstacle, Mixed}