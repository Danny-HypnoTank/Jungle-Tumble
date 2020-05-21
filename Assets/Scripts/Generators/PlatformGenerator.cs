using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    #region VARIABLES
    [SerializeField]
    List<GameObject> platformPrefabs;

    [SerializeField] List<Material> platformMaterials;
    [SerializeField] Material ghostMaterial;

    [SerializeField]
    List<Obstacle> obstacleLib;

    [SerializeField]
    List<Reward> rewardLib;
    #endregion

    #region PROPERTIES
    Transform parentTransform { get { return GameObject.Find("Platforms").transform; } }
    #endregion

    #region METHODS

    #region USER-DEFINED

    // JRH v0.1.8:
    /// <summary>
    /// Called by PlatformManager, creates a default set of platforms and then sets them using the code specified (default code, if none specified)
    /// </summary>
    /// <param name="code A"></param>
    /// <param name="code B"></param>
    /// <param name="code C"></param>
    /// <returns></returns>
    public PlatformRow GenerateRow(char codeA = 'P', char codeB = 'P', char codeC = 'P')
    {
        PlatformRow newRow = InstantiateRow();
        SetRow(ref newRow, codeA, codeB, codeC);
        return newRow;
    }

    // JRH v0.1.10:
    /// <summary>
    /// Called by PlatformManager, creates a default set of platforms and then sets them using the data specified
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public PlatformRow GenerateRow(RowData data)
    {
        PlatformRow newRow = InstantiateRow();
        SetRow(ref newRow, data);
        return newRow;
    }

    // JRH v0.1.2
    /// <summary>
    /// Instantiates a default row of three platforms
    /// </summary>
    /// <returns></returns>
    PlatformRow InstantiateRow()
    {
        GameObject rowObj = new GameObject("Platform Row #");   // JRH v0.1.2: Creates the initial object
        rowObj.transform.SetParent(parentTransform);   // JRH v0.1.2: Attaches the just created row object to the parent transform
        PlatformRow row = rowObj.AddComponent<PlatformRow>();   // JRH v0.1.2: Adds a fresh PlatformRow script to the row object
        Platform[] platforms = new Platform[3];   // JRH v0.1.2: Used to hold the platforms the following loop creates so they can be passed through Initialize

        for (int i = 0; i < 3; i++)
        {
            // JRH v0.1.2: Instantiate as many platform prefabs as there are lanes in-game

            GameObject platformObj = Instantiate(platformPrefabs[UnityEngine.Random.Range(0, platformPrefabs.Count)]);
            // JRH v0.2.9: Instantiates a random platform from the prefabs list;
            platformObj.transform.GetChild(0).GetComponent<Renderer>().material = platformMaterials[UnityEngine.Random.Range(0, platformMaterials.Count)];
            // JRH 0.3.10: Apply a random material to the platform
            platformObj.transform.SetParent(rowObj.transform);
            // JRH v0.1.2: Attach that GameObject to the parent transform
            platformObj.transform.Translate(new Vector3((i - 1) * GameManager.Instance.LaneDistance, 0f, 0f));
            // JRH v0.1.2: Position it accordingly based on the current value of i
            platforms[i] = platformObj.AddComponent<Platform>();
            // JRH v0.1.2: Add a Platform component to the object
        }

        row.Initialize(platforms[0], platforms[1], platforms[2]);
        // JRH v0.1.2: Initialize the row with each new Platform under it

        return row;
    }

    // JRH v0.1.8
    /// <summary>
    /// Modifies a newly instantiated row using a specified code to add pits, obstacles and rewards
    /// </summary>
    /// <param name="row"></param>
    /// <param name="codeA"></param>
    /// <param name="codeB"></param>
    /// <param name="codeC"></param>
    void SetRow(ref PlatformRow row, char codeA, char codeB, char codeC)
    {
        char[] code = new char[3] { codeA, codeB, codeC };    // JRH v0.1.8: Put the three codes into an array

        // JRH v0.1.8: Set functionality to PlatformRow / gameObject

        row.gameObject.name += row.transform.GetSiblingIndex();
        // JRH v0.1.8: Add the row's in-scene position relative to the other rows to the gameObject's name

        if (GameManager.Instance.PlatformManager.LastRow != null)
            row.transform.position = new Vector3(0f, 0f, GameManager.Instance.PlatformManager.LastRow.transform.position.z - GameManager.Instance.LaneDistance);
        // JRH v0.1.8: Move the new Row to one laneDistance unit behind the last row

        for (int i = 0; i < 3; i++)
        {
            row.Platforms[i].SetPlatform(code[i]);

            row.Platforms[i].SetCritical(true);

            if (ObstacleLibrary.DecipherCode(row.Platforms[i].Code, PlatformType.PIT)) row.Platforms[i].SetPit();
            //if (DecipherCode(row.Platforms[i].Code, ItemType.OBS)) row.Platforms[i].SetObstacle(GenerateObstacle(row.Platforms[i]));
        }
    }

    // JRH v0.2.0
    /// <summary>
    /// Sets the row passed in as a reference using the values contained in a Data parameter
    /// </summary>
    /// <param name="row"></param>
    /// <param name="data"></param>
    public void SetRow(ref PlatformRow row, RowData data)
    {
        // JRH v0.2.0: Set functionality to PlatformRow / gameObject
        row.gameObject.name += row.transform.GetSiblingIndex();
        // JRH v0.2.0: Add the row's in-scene position relative to the other rows to the gameObject's name

        row.SetCode(data.CodeArray);
        // JRH v0.2.0: Set the row's code to the code array held in Data

        if (GameManager.Instance.PlatformManager.LastRow != null)
            row.transform.position = new Vector3(0f, 0f, GameManager.Instance.PlatformManager.LastRow.transform.position.z - GameManager.Instance.LaneDistance);
        // JRH v0.2.0: Move the new Row to one laneDistance unit behind the last row

        // JRH v0.2.0: Set functionality to each Platform component
        for (int i = 0; i < 3; i++)
        {
            row.Platforms[i].SetPlatform(data.CodeArray[i]);
            // JRH v0.2.0: Set the platform's code to the corresponding code in data
            row.Platforms[i].SetCritical(data.CritArray[i]);
            // JRH v0.2.0: Set the platform's critical value to the corresponding critical value in data
            if (ObstacleLibrary.DecipherCode(row.Platforms[i].Code, PlatformType.PIT)) row.Platforms[i].SetPit();
            // JRH v0.2.0: If the code returns a pit, call the SetPit method in that platform to turn it into a pit

            if (GameManager.Instance.GhostMode)
            {
                if (row.Platforms[i].IsCritical && (row.Platforms[i].Code == 'p' || row.Platforms[i].Code == 'P')) row.Platforms[i].transform.GetChild(0).GetComponent<Renderer>().material = ghostMaterial;
            }
        }

        GenerateObstacles(row, data);

        for (int i = 0; i < 3; i++)
        {
            if (ObstacleLibrary.DecipherCode(row.Platforms[i].Code, PlatformType.REW)) row.Platforms[i].SetReward(GenerateReward(row.Platforms[i]));
        }
    }

    // JRH v0.2.9: 
    /// <summary>
    /// A terribly long and convoluted method that accurately decides on which obstacles to generate given the rowdata parameter
    /// </summary>
    /// <param name="row"></param>
    /// <param name="data"></param>
    void GenerateObstacles(PlatformRow row, RowData data)
    {
        //int rowCount = 0;
        List<Obstacle> obstacles = new List<Obstacle>();
        List<int> obstacleLanes = new List<int>();
        List<int> lanesNeedingObstacles = new List<int>() { 0, 1, 2 };
        //int width, height;
        char[] codeArray = data.CodeArray;

        // JRH v0.2.9: For each lane that does require a an obstacle, remove it from the lane that requires obstacles REDUNDANCY HURR DURR
        for (int i = 0; i < data.CodeArray.Length; i++) if (!ObstacleLibrary.DecipherCode(data.CodeArray[i], PlatformType.OBS)) lanesNeedingObstacles.Remove(i);

        // JRH v0.2.9: Determine which obstacles are going to be put into the row and the leftmost lane they cover, and return it to the obstacles and obsLanes lists
        while (lanesNeedingObstacles.Count > 0)
        {
            int width = 1;
            int nextLaneCheck = 0;
            Obstacle nextObs;
            char code, nextCode;

            code = codeArray[lanesNeedingObstacles[0]];
            // JRH v0.2.9: Using the next row needing an obstacle, get a code consisting of an upper and lower bool

            // JRH v0.2.9: Determine the width of the next obstacle through how many adjascent lane match codes with the current lane
            while (lanesNeedingObstacles.Count - nextLaneCheck > 1)
            {
                nextCode = codeArray[lanesNeedingObstacles[1]];
                if (code == nextCode) { width++; nextLaneCheck++; } else break;
            }

            //height = ObstacleLibrary.DecipherCode(code, PlatformType.TALL) ? 2 : 1;
            // Determine the height of the next obstacle based on the code

            // JRH v0.2.9: Find a random obstacle that fits into the height and width requirements
            do
            {
                Obstacle ran = GetComponent<ObstacleLibrary>().LookUpObstacle();
                nextObs = ObstacleLibrary.CompareCodes(code, ran.Code) && ran.Width <= width ? ran : null;
                //nextObs = ran.Height == height && ran.Width <= width ? ran : null;
            } while (nextObs == null);


            obstacles.Add(nextObs);
            // JRH v0.2.9: Add the current obstacle to the list of obstacles
            obstacleLanes.Add(lanesNeedingObstacles[0]);
            // JRH v0.2.9: Add the current lane to the list of lanes
            lanesNeedingObstacles.RemoveRange(0, nextObs.Width);
            // JRH v0.2.9: Remove the lanes that have just been resolved from the list of lanes that need obstacles
        }

        // JRH v0.2.9: For each obstacle rolled, instantiate it into the scene as appropriate
        for (int i = 0; i < obstacleLanes.Count; i++)
        {
            Obstacle obs = obstacles[i];
            GameObject obj;
            float leftPos, rightPos;
            List<int> lanesCovered;

            obj = Instantiate(obs.gameObject);
            // JRH v0.2.9: Instantiate the obstacle object in-scene
            leftPos = obstacleLanes[i] - 1;
            rightPos = leftPos + obs.Width - 1;
            // JRH v0.2.9: Determine the leftmost and rightmost lane that the obstacle is meant to cover

            obj.transform.position = new Vector3(((leftPos + rightPos) / 2) * GameManager.Instance.LaneDistance, 0, row.transform.position.z);
            // JRH v0.2.9: Set the object's position to the midpoint of the left and right lanes covered and the row it's on's Z position

            for (int j = obstacleLanes[i]; j <= obstacleLanes[i] + obs.Width - 1; j++) row.Platforms[j].SetObstacle(obj.GetComponent<Obstacle>());
            // JRH v0.2.9: Determine which platforms the object is meant to cover and assign them said obstacle

            obj.transform.SetParent(row.transform);
            // JRH v0.2.9: Finally, set the parent transform so it treadmills along with the row
        }
    }

    // JRH v0.2.0: 
    /// <summary>
    /// Returns an obstacle from the ObstacleLibrary based on the parameters
    /// </summary>
    /// <param name="maxWidth"></param>
    /// <param name="maxHeight"></param>
    /// <returns></returns>
    Obstacle RollNewObstacle(int maxWidth, int maxHeight)
    {
        bool success = false;
        Obstacle newObstacle;

        do
        {
            // JRH v0.2.0: Find a suitable obstacle to return
            newObstacle = obstacleLib[UnityEngine.Random.Range(0, obstacleLib.Count)];
            // JRH v0.2.0: Call a random obstacle from the obstacle library
            //if (newObstacle.Height <= maxHeight && newObstacle.Width <= maxWidth) success = true;
            // JRH v0.2.0: If said obstacle fits the max width and height, the loop is successfully completed
        } while (success != true);

        return newObstacle;
    }

    // JRH v0.2.2:
    public Reward GenerateReward(Platform platform, int index = 0)
    {
        Reward reward;

        // JRH v0.2.2: Identify which reward needs to be generated
        switch (index)
        {
            case 0:
                reward = RollNextReward(); break;
            // JRH v0.2.2: If the method has been called without passing in a reward value, roll a random one
            default:
                reward = index < rewardLib.Count ? rewardLib[index-1] : rewardLib[0]; break;
                // JRH v0.2.2: If the method has been called passing in a reward value, return that value
        }

        // JRH v0.2.2: Now that the reward has been identified, let's put it in the game
        GameObject rewardObj = Instantiate(reward.gameObject);
        // JRH v0.2.2: Instantiate the chosen reward
        rewardObj.transform.position = platform.transform.position;
        // JRH v0.2.2: Set the reward's position to the platform's position and let it sit a little higher rather than clipping through the floor
        if (platform.BottomLayerTaken) rewardObj.transform.Translate(0, GameManager.Instance.LaneDistance, 0);
        //if (rewardObj.name == "Obstacle (Gappy)(Clone)") rewardObj.transform.Translate(0, -GameManager.Instance.LaneDistance, 0);
        // JRH v0.2.2: If the bottom layer already has an obstacle on it, or it is a pit, increase the Y position of the reward so the player can jump into it
        rewardObj.transform.SetParent(platform.transform);
        // JRH v0.2.2: Set the parent transform so the reward moves alongside the environment when treadmilling

        return reward;
    }

    public Reward GenerateReward(Transform trans, int index = 0)
    {
        Reward reward;

        // JRH v0.2.2: Identify which reward needs to be generated
        switch (index)
        {
            case 0:
                reward = RollNextReward(); break;
            // JRH v0.2.2: If the method has been called without passing in a reward value, roll a random one
            default:
                reward = index - 1 < rewardLib.Count ? rewardLib[index - 1] : rewardLib[0]; break;
                // JRH v0.2.2: If the method has been called passing in a reward value, return that value
        }

        // JRH v0.2.2: Now that the reward has been identified, let's put it in the game
        GameObject rewardObj = Instantiate(reward.gameObject);
        // JRH v0.2.2: Instantiate the chosen reward
        rewardObj.transform.position = trans.position;
        // JRH v0.2.2: Set the reward's position to the platform's position and let it sit a little higher rather than clipping through the floor
        rewardObj.transform.SetParent(trans);
        // JRH v0.2.2: Set the parent transform so the reward moves alongside the environment when treadmilling

        return reward;
    }

    Reward RollNextReward()
    {
        Reward ranReward;
        float ranVal = UnityEngine.Random.value;
        if (ranVal > 0.98) ranReward = rewardLib[2];
        else if (ranVal > 0.9) ranReward = rewardLib[1];
        else ranReward = rewardLib[0];

        return ranReward;
    }
    #endregion

    #region MONOBEHAVIOUR
    #endregion

    #endregion
}