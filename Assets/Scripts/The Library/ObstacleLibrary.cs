using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleLibrary : MonoBehaviour
{
    Dictionary<string, Obstacle> obstacleDictionary;
    List<string> obstacleKeys;

    [SerializeField]
    Obstacle Generic, Vase;


    [SerializeField]
    Obstacle Gappy, Hatty, Lanky, Sleepy, Smashy, Twisty, Winky;

    public List<string> Keys { get { return obstacleKeys; } }

    // JRH v0.2.0:
    /// <summary>
    /// Returns true if the char code passed in represents an obstacle or pit
    /// </summary>
    /// <param name="code"></param>
    /// <returns></returns>
    public static bool DecipherCode(char code)
    {
        return code == 'X' || code == 'x' || code == 'O' || code == 'o' || code == 't' || code == 'h' || code == 'H' ? true : false;
    }

    // JRH v0.1.10:
    /// <summary>
    /// Returns true if the char code passed in represents the obstacle option passed in
    /// </summary>
    /// <param name="code"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool DecipherCode(char code, PlatformType type)
    {
        switch (type)
        {
            case PlatformType.PIT:
                return code == 'X' || code == 'x' ? true : false;
            case PlatformType.OBS:
                return code == 'O' || code == 'o' || code == 't' || code == 'h' || code == 'H' ? true : false;
            case PlatformType.REW:
                return code == 'P' || code == 'X' || code == 'O' || code == 'H' ? true : false;
            case PlatformType.TALL:
                return code == 't' || code == 'h' || code == 'H' ? true : false;
            case PlatformType.HIGH:
                return code == 'h' || code == 'H' ? true : false;
            case PlatformType.LOW:
                return code == 'o' || code == 'O' || code == 'x' || code == 'X' || code == 't' ? true : false;
            default: return false;
        }
    }

    public static bool CompareCodes(char a, char b)
    {
        return a == b || a == char.ToUpper(b) || a == char.ToLower(b) ? true : false;
    }

    public static char RandomCode()
    {
        List<char> codeList = new List<char>() { 'x', 'o', 't', 'h' };
        return codeList[Random.Range(0, codeList.Count)];
    }

    public Obstacle LookUpObstacle(string index = "Random")
    {
        return obstacleDictionary.ContainsKey(index) ? obstacleDictionary[index] : obstacleDictionary[obstacleKeys[Random.Range(0, obstacleKeys.Count)]];
    }

    private void Awake()
    {
        obstacleDictionary = new Dictionary<string, Obstacle>()
        {
            {"Generic", Generic },
            {"Vase", Vase },
            {"Gappy", Gappy },
            {"Hatty", Hatty },
            {"Lanky", Lanky },
            {"Sleepy", Sleepy },
            {"Smashy", Smashy },
            {"Twisty", Twisty },
            //{"Winky", Winky }
        };

        obstacleKeys = new List<string>();
        foreach (string key in obstacleDictionary.Keys) obstacleKeys.Add(key);
    }
}
