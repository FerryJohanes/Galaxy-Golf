using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageLoader : MonoBehaviour
{
    public void Loadlevel(string levelName)
    {
        SceneLoader.Load("Level"+levelName);
    }
}
