using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader
{
    private static string sceneToLoad;

    public static string SceneToLoad { get => sceneToLoad; }

    //Load
    public static void Load (string sceneName)
    {
        SceneManager.LoadScene (sceneName);
        ProgressLoad(sceneName);
    }

    //Progress load
    public static void ProgressLoad(string sceneName)
    {
        sceneToLoad = sceneName;
        SceneManager.LoadScene("LoadingProgress");
    }

    //Reload Level
    public static void ReloadLevel()
    {
        var currentScene = SceneManager.GetActiveScene().name;
        ProgressLoad(currentScene);
    }

    //Load Next Level
    public static void LoadNextLevel()
    {
        var currentSceneName = SceneManager.GetActiveScene().name;
        var nextLevel = int.Parse(currentSceneName.Split("Level")[1]) + 1;
        string nextSceneName = "Level" + nextLevel;

        if (SceneUtility.GetBuildIndexByScenePath(nextSceneName) == -1)
        {
            Debug.LogError(nextSceneName + " Does Not Exists ");
            return;
        }
        ProgressLoad(nextSceneName);   
    }

}
