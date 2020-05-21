using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonumentChunk : PlatformChunk
{
    #region VARIABLES
    static bool side;
    [SerializeField]
    bool wide;
    [SerializeField]
    ChunkLength lengthOption;
    [SerializeField] bool hardMode;
    #endregion

    #region PROPERTIES
    #endregion

    #region METHODS
    protected override List<RowData> GenerateRows()
    {
        base.GenerateRows();

        int blockLength = wide ? 4 : 3;
        length = (int)lengthOption * blockLength;

        for (int i = 0; i < length; i++)
        {
            char[] codes = new char[3] { 'p', 'p', 'p' };

            switch ((i + blockLength) % blockLength)
            {
                case 1:
                    codes[side ? 0 : 2] = 't';
                    codes[1] = Random.value > 0.60 ? 't' : 'h';
                    codes[side ? 2 : 0] = hardMode && UnityEngine.Random.value > 0.8 ? 'h' : 'p';
                    break;
                case 2:
                    codes[side ? 0 : 2] = Random.value > 0.4 ? 'o' : 'p';
                    side = !side;
                    break;
            }

            rowCodes.Add(new RowData(codes[0], codes[1], codes[2]));
        }

        return rowCodes;
    }

    protected override void AddRewards()
    {
        for (int i = 0; i < rowCodes.Count; i++)
        {
            int blocLength = wide ? 4 : 3;

            if ((i + blocLength) % blocLength == 2)
            {
                for (int j = 0; j < rowCodes[i].CodeArray.Length; j++)
                {
                    if (j != 1 && !ObstacleLibrary.DecipherCode(rowCodes[i].CodeArray[j])) rowCodes[i].CodeArray[j] = char.ToUpper(rowCodes[i].CodeArray[j]);
                }
            }
        }
    }
    #endregion

    #region MONOBEHAVIOUR
    #endregion
}
