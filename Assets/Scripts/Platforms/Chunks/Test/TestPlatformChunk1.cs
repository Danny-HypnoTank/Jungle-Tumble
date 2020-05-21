using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlatformChunk1 : PlatformChunk
{
    // JRH v0.1.10: 
    protected override List<RowData> GenerateRows()
    {
        for (int i = 0; i < length; i++)
        {
            char codeA, codeB, codeC;

            if (i == 0 || i == length - 1) { codeA = 'P'; codeB = 'P'; codeC = 'P'; }    // JRH v0.1.10: If the row is first or last, set it to flat platforms
            else if (i == 0 + 1 || i == length - 2) { codeA = 'X'; codeB = 'O'; codeC = 'X'; }     // JRH v0.1.10: If the row is the second first or second last, set it to Pit, Obs, Pit
            else { codeA = 'X'; codeB = 'P'; codeC = 'X'; }    // JRH v0.1.10: Otherwise set the row to Pit, Plat, Pit

            rowCodes.Add(new RowData(codeA, codeB, codeC));    // JRH v0.1.10: Add the new code entry to the returning list
        }

        return rowCodes;
    }
}

/* LEGACY CODE v0.1.10
[SerializeField]
List<PlatformCode> codeOptions;

public override void LoadRows()
{
    base.LoadRows();

    TempChunkGenerator gen = GameObject.Find("Generator").GetComponent<TempChunkGenerator>();

    rowCodes = new List<PlatformCode>();

    for (int i = 0; i < length; i++)
    {
        PlatformCode newCode = PlatformCode.XXX;

        if (i == 0) Debug.Log("FIRST LINE");
        if (i == length - 1) Debug.Log("LAST LINE");
        if (i == 0 || i == length - 1) newCode = PlatformCode.PPP;
        else gen.RollNewCode(ref newCode, codeOptions);

        rowCodes.Add(newCode);
    }
}
*/
