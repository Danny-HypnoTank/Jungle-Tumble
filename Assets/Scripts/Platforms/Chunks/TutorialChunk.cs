using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialChunk : PlatformChunk
{
    bool sideStart;

    protected override List<RowData> GenerateRows()
    {
        base.GenerateRows();

        sideStart = Random.value > 0.5 ? true : false;

        for (int i = 0; i < 3; i++)
        {
            rowCodes.Add(new RowData('p', 'P', 'p'));
            rowCodes.Add(new RowData('p', 'p', 'p'));
        }

        rowCodes.Add(new RowData('p', 'p', 'p'));
        rowCodes.Add(new RowData('p', 'p', 'p'));

        for (int i = 0; i < 4; i++)
        {
            char[] nextRow = new char[3] { 'p', 'p', 'p' };
            switch (i)
            {
                case 0: nextRow[1] = 'o'; break;
                case 1: nextRow[sideStart ? 0 : 2] = 'o'; break;
                case 2: nextRow[sideStart ? 2 : 0] = 'o'; break;
                case 3: nextRow[1] = 't'; break;
            }
            rowCodes.Add(new RowData(nextRow[0], nextRow[1], nextRow[2]));
            rowCodes.Add(new RowData('p', 'p', 'p'));
            rowCodes.Add(new RowData('p', 'p', 'p'));
        }

        rowCodes.Add(new RowData('p', 'p', 'p'));
        rowCodes.Add(new RowData('x', 'x', 'x'));
        rowCodes.Add(new RowData('p', 'p', 'p'));

        return rowCodes;
    }
}
