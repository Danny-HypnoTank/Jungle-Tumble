using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlatformChunk2 : PlatformChunk
{
    ZigZagStart start;
    PlatformType obsType;

    protected override List<RowData> GenerateRows()
    {
        /*
        base.LoadRows();

        rowCodes = new List<PlatformCode>();

        int[] wideRows = new int[] { 2, 6 };
        int[] obsRows = new int[] { 1, 3, 5, 7 };

        start = (ZigZagStart)RandomBool();
        obsType = (ObstacleType)RandomBool();

        Debug.Log(start + ", " + obsType);

        PlatformCode newCode;

        for (int i = 0; i < length; i++)
        {
            newCode = start == ZigZagStart.LEFT ? PlatformCode.PXX : PlatformCode.XXP;
            if (i >= 3 && i <= 5) newCode = newCode == PlatformCode.PXX ? PlatformCode.XXP : PlatformCode.PXX;
            if ((i == 1 || i == 4 || i == 7) && obsType == ObstacleType.PIT) newCode = PlatformCode.XXX;
            if (i == 2 || i == 6) newCode = PlatformCode.PPP;

            rowCodes.Add(newCode);
        }
        */

        return null;
    }

    int RandomBool()
    {
        if (Random.value > 0.5) return 1;
        else return 0;
    }
}

public enum ZigZagStart { LEFT, RIGHT }
public enum PlatformType { PIT, OBS, REW, TALL, HIGH, LOW}