using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scene
{
    GameScene,
    MainMenu,
    // LoadingScene
}

/// <summary>
/// Responsible for loading scenes, including MainMenu OnAppStart()<br/>
/// </summary>
public static class SceneLoader
{

    public static void LoadScene(Scene scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }

    public static void CloseGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false; // allow quitting in Editor
        #endif
    }
}
