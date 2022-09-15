using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    //load scene by index
    public static void LoadScene(int index)
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(index);
    }
    //reload current scene
    public static void ReloadScene()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    //load next scene
    public static void NextSceneLoad()
    {
        Time.timeScale = 1.0f;
        int index = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(index);
    }
    //get index of current scene
    public static int SceneIdnex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }
}
