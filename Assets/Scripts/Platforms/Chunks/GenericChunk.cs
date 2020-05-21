using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericChunk : PlatformChunk
{
    [SerializeField] ChunkLength option;

    protected override List<RowData> GenerateRows()
    {
        base.GenerateRows();

        switch (option) { case ChunkLength.Regular: length = 7; break; case ChunkLength.Short: length = 3; break; }

        for (int i = 0; i < length; i++)
        {
            char[] codes = new char[3] { 'p', 'p', 'p' };

            //int ran = Random.Range(0, 3);
            //codes[ran] = 'P';

            rowCodes.Add(new RowData(codes[0], codes[1], codes[2]));
        }

        return rowCodes;
    }

    protected override void AddRewards()
    {
        for (int i = 0; i < length; i++)
        {
            if ((i + 2) % 2 == 0)
            {
                int ran = Random.Range(0, 3);
                rowCodes[i].CodeArray[ran] = char.ToUpper(rowCodes[i].CodeArray[ran]);
            }
        }
    }
}