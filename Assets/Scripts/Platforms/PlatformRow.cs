using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformRow : MonoBehaviour
{
    #region VARIABLES

    #region PRIVATE
    List<Platform> platforms = new List<Platform>();
    char[] rowCode = new char[3];
    #endregion

    #region PUBLIC
    public List<Platform> Platforms { get { return platforms; } }
    #endregion

    #endregion

    #region METHODS

    #region CONSTRUCTOR
    #endregion

    #region USER-DEFINED
    public void Initialize(Platform a, Platform b, Platform c)
    {
        platforms.AddRange(new Platform[3] { a, b, c });

        for (int i = 0; i < 3; i++)
        {
            rowCode[i] = platforms[i].Code;
        }
    }

    public void SetCode(char[] newCode)
    {
        for (int i = 0; i < 3; i++)
        {
            rowCode[i] = newCode[i];
        }
    }
    #endregion

    #region MONOBEHAVIOUR
    #endregion

    #endregion
}
