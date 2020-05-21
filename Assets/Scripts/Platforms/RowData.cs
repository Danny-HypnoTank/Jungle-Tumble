using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RowData
{

    private char[] codeArray;
    public char[] CodeArray { get { return codeArray; } }

    private bool[] critArray;
    public bool[] CritArray { get { return critArray; } }

    private string[] valueArray;
    public string[] ValueArray { get { return valueArray; } }

    public RowData(char codeA = 'p', char codeB = 'p', char codeC = 'p', int valA = 0, int valB = 0, int valC = 0)
    {

        codeArray = new char[3] { codeA, codeB, codeC };
        valueArray = new string[3] { "", "", ""};

        critArray = new bool[3] { true, true, true };
    }

    // JRH v0.2.4
    /// <summary>
    /// Compares and sets the values of this struct to the most recent piece of data in-game
    /// </summary>
    /// <returns></returns>
    public bool CritCheck(RowData? lastRow = null)
    {
        RowData lastData;
        List<int> unresolved = new List<int>() { 0, 1, 2 };
        bool criticalResolution;
        int critCount = 0;

        if (lastRow == null)
        {
            PlatformRow lastInstance = GameManager.Instance.PlatformManager.LastRow;
            char[] codes = new char[3] { lastInstance.Platforms[0].Code, lastInstance.Platforms[1].Code, lastInstance.Platforms[2].Code };
            bool[] crits = new bool[3] { lastInstance.Platforms[0].IsCritical, lastInstance.Platforms[1].IsCritical, lastInstance.Platforms[2].IsCritical };

            lastData = new RowData(codes[0], codes[1], codes[2]);
            lastData.critArray = crits;
        }
        else lastData = (RowData)lastRow;

        for (int i = 0; i < 3; i++)
        {
            if (lastData.critArray[i])
            {
                critArray[i] = !ObstacleLibrary.DecipherCode(codeArray[i]) || !ObstacleLibrary.DecipherCode(lastData.codeArray[i]) ? true : false;
                // JRH v0.2.0: If the last platform was on the critical path and this one is not an obstacle or pit, set it on the critical path
            }
            else critArray[i] = lastData.critArray[i];
            // JRH v0.2.0: Otherwise, let it's critical path follow the platform previous to it's critical path
            //Debug.Log(i + ":" + lastCode[i] + "," + lastCrit[i] + "," + codeArray[i] + "," + critArray[i]);

            if (ObstacleLibrary.DecipherCode(lastData.codeArray[i], PlatformType.TALL)) critArray[i] = false;
            // JRH v0.2.9: If the current code is tall, remove the platform from the critical path. There is no way above a tall obstacle.
        }

        for (int i = 0; i < critArray.Length; i++) if (critArray[i]) unresolved.Remove(i);
        // JRH v0.2.0: If each platform is critical, whoohoo! We don't need to do anything with it in the next step


        criticalResolution = unresolved.Count < 1 ? true : false;
        // If there are any unresolved platforms, set the conditions for the next loop starting

        /* JRH v0.3.2: Due to the increase in time to switch lanes, 
        while (criticalResolution == false)
        {
            // While there are platforms unresolved, run this loop
            criticalResolution = true;
            // JRH v0.2.0: Resets the critical resolution pre-emptively, to be changed again if any platforms have their critical value changed
            List<int> toResolve = new List<int>();

            foreach (int i in unresolved)
            {
                bool leftCrit, rightCrit;

                leftCrit = i > 0 && !GameManager.Instance.PlatformGenerator.DecipherCode(codeArray[i - 1]) && critArray[i - 1] ? true : false;
                rightCrit = i < critArray.Length - 1 && !GameManager.Instance.PlatformGenerator.DecipherCode(codeArray[i + 1]) && critArray[i + 1] ? true : false;
                // JRH v0.2.0: Checks the platforms to the left and right of this platform (if they exist) and returns a bool for each if they can transfer critical value

                if ((leftCrit || rightCrit) && !GameManager.Instance.PlatformGenerator.DecipherCode(codeArray[i]))
                {
                    // JRH v0.2.0: If critical value can be transfered (i.e. if the platform isn't a danger and the left or right platforms can transfer critical value
                    critArray[i] = true;
                    // JRH v0.2.0: Transfer the critical value
                    criticalResolution = false;
                    // JRH v0.2.0: Change the critical resolution so the loop repeats at least once more
                    toResolve.Add(i);
                    // JRH v0.2.0: Add the platform index to the list of platforms to resolve
                }
            }

            foreach (int i in toResolve) unresolved.Remove(i);
            // JRH v0.2.0: Remove any platforms that have been resolved from the list of platforms needing to be resolved
        }*/

        List<int> toResolve = new List<int>();
        foreach (int i in unresolved)
        {
            bool leftCrit, rightCrit;

            leftCrit = i > 0 && !ObstacleLibrary.DecipherCode(codeArray[i - 1]) && critArray[i - 1] ? true : false;
            rightCrit = i < critArray.Length - 1 && !ObstacleLibrary.DecipherCode(codeArray[i + 1]) && critArray[i + 1] ? true : false;
            // JRH v0.2.0: Checks the platforms to the left and right of this platform (if they exist) and returns a bool for each if they can transfer critical value

            if ((leftCrit || rightCrit) && !ObstacleLibrary.DecipherCode(codeArray[i]))
            {
                // JRH v0.2.0: If critical value can be transfered (i.e. if the platform isn't a danger and the left or right platforms can transfer critical value
                toResolve.Add(i);
                // JRH v0.2.0: Add the platform index to the list of platforms to resolve
            }
        }
        foreach (int i in toResolve)
        {
            critArray[i] = true;
        }

        foreach (bool b in critArray) if (b) critCount++;
        // JRH v0.2.0: Count the number of platforms that have had their critical value checked and are indeed part of the critical path

        return critCount > 0 ? true : false;
        // JRH v0.2.0: Return true as long as there is one critical path available on this row, otherwise return false

    }
}
