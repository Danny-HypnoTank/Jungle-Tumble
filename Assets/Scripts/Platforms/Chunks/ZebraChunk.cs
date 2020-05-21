using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZebraChunk : PlatformChunk
{
    #region VARIABLES
    int xRowVal;

    [SerializeField]
    ChunkLength lengthOption;

    [SerializeField]
    bool wide, withObs;
    #endregion

    #region PROPERTIES
    #endregion

    #region METHODS
    protected override List<RowData> GenerateRows()
    {
        base.GenerateRows();    // JRH v0.2.0: Called to reset the list of rows

        xRowVal = wide ? 4 : 3;
        // JRH v0.2.0: Sets the number of platforms before each new row to 3 (4 if wide is true)

        length = ((int)lengthOption * xRowVal) + (xRowVal - 1);
        // JRH v0.2.0: Set the length so the chunk produces one pit row for every value inside LengthOption

        for (int i = 0; i < length; i++)
        {
            // JRH v0.2.0: For each row the chunk is meant to generate, initialise and manipulate the code to have a pit row in between platform rows

            char[] codes;

            if ((xRowVal + i + 1) % xRowVal != 0)
            {
                codes = new char[] { 'p', 'p', 'p' };
                // JRH v0.2.0: If the row is a regular platform row, load the code with platforms

                if (withObs)
                {
                    // JRH v0.2.0: If the chunk is generating with obstacles ( aka hard mode aka heaven forbid ), add some obstacles

                    int obsCount = Random.Range(0, 3);

                    List<int> obsI = new List<int>();

                    while (obsI.Count < obsCount)
                    {
                        // JRH v0.2.0: Load up the obsI list with platform indexes to put obstacles on until as many as obsCount

                        int ran = Random.Range(0, 3);
                        // JRH v0.2.0: Sets the random index to put an obstacle on
                        if (!obsI.Contains(ran)) obsI.Add(ran);
                        // JRH v0.2.0: If the random index isn't already contained insided obsI, add it 
                        //    ( This avoids multiple identical i and fewer obstacles than should generating)
                    }

                    foreach (int j in obsI) codes[j] = 'o';
                    // JRH v0.2.0: Add as many obstacles as there are returned in the obsI list
                }
            }
            else
            {
                codes = new char[] { 'x', 'x', 'x' };
                // JRH v0.2.0: If the row is a pit row, load the code with pit modifiers
            }

            rowCodes.Add(new RowData(codes[0], codes[1], codes[2]));
            // JRH v0.2.0: Finally, add a new RowData to RowCodes using the now manipulated code
        }

        return rowCodes;
    }

    protected override void AddRewards()
    {
        // JRH v0.2.5: Add a reward to a lane over each gap that is part of the critical path
        foreach(RowData data in rowCodes)
        {
            // JRH v0.2.5: Add a reward only if the index of the data indicates a row filled with pits
            if ((rowCodes.IndexOf(data) + 1) % xRowVal == 0)
            {
                List<int> lanes = new List<int>();

                // JRH v0.2.5: hoo boy this line's a doozy
                for (int i = 0; i < 3; i++)
                    if (data.CritArray[i] && (!ObstacleLibrary.DecipherCode(rowCodes[rowCodes.IndexOf(data)-1].CodeArray[i])
                        && !ObstacleLibrary.DecipherCode(rowCodes[rowCodes.IndexOf(data)+1].CodeArray[i]))) lanes.Add(i);
                // JRH v0.2.5: If each platform is both on the critical path and the platforms both in front and behind it aren't pits or obstacles, 
                // add it to the list of potential lanes we're putting a reward into

                int ran = Random.Range(0, lanes.Count);
                // JRH v0.2.5: Out of the rows specified above, choose a random one

                data.CodeArray[lanes[ran]] = char.ToUpper(data.CodeArray[lanes[ran]]);
                // JRH v0.2.5: Put a reward into that one
            }
        }
    }
    #endregion

    #region MONOBEHAVIOUR
    #endregion
}
