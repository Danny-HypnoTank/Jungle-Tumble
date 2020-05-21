using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    #region VARIABLES

    #region PRIVATE
    [SerializeField]
    bool floorLayer = true, bottomLayer = false, upperLayer = false;

    bool floor { get { return code == 'X' ? true : false; } }


    Obstacle obstacle;
    Reward reward;

    bool critPath;

    char code;

    [SerializeField]
    List<Texture> textures;
    #endregion

    #region PROPERTIES
    public char Code { get { return code; } }

    public Interactable BottomLayer { get { if (obstacle != null) return obstacle; else if (reward != null) return reward; else return null; } }
    public Interactable TopLayer { get { if (obstacle != null && reward != null) return reward; else return null; } }

    public bool BottomLayerTaken { get { if (BottomLayer != null) return true; else if (ObstacleLibrary.DecipherCode(code, PlatformType.PIT)) return true; else return false; } }
    public bool TopLayerTaken { get { if (TopLayer != null) return true; else return false; } }

    public bool IsCritical { get { return critPath; } }
    #endregion

    #endregion

    #region METHODS

    #region CONSTRUCTOR
    #endregion

    #region USER-DEFINED
    public void SetPlatform(char setCode)
    {
        code = setCode;
    }

    public void SetPit()
    {
        floorLayer = false;
        transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        transform.GetChild(1).gameObject.SetActive(false);
    }

    public void SetObstacle(Obstacle newObstacle)
    {
        obstacle = newObstacle;
    }

    public void SetReward(Reward newReward)
    {
        reward = newReward;
    }

    public void SetCritical(bool b)
    {
        critPath = b;
    }

    public void SetMaterial(Material mat)
    {
        GetComponent<Renderer>().material = mat;
    }
    #endregion

    #region MONOBEHAVIOUR
    #endregion

    #endregion
}