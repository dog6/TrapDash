using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public Unlockables unlockables;
    public Transform Spawnpoint;
    public BackgroundController controller;
    public MusicManager musicManager;
    private bool firstAwake = true;
    void Awake()
    {
        #if UNITY_EDITOR
                if (!Application.isPlaying) return;
        #endif
        Debug.Log($"Loaded scene: {SceneManager.GetActiveScene().name}");
        if (SceneManager.GetActiveScene().name == Scene.GameScene.ToString())
        {
            Debug.Log("Loading game scene");
            LoadPlayer();
        }
        else
        {
            Debug.Log("Loading main menu & save data");
            SaveState.LoadGameData();
        }

        // Game first started
        if (firstAwake)
        {
            // Apply settings 
            ApplyVideoSettings();
            musicManager.ApplySavedSettings();
            Debug.Log("Game started");
            firstAwake = false;
        }
    }

    public void ApplyVideoSettings()
    {
        QualitySettings.SetQualityLevel(SaveState.saveData.gameSettings.selectedGraphicsIndex);
        var savedResolution = Screen.resolutions[SaveState.saveData.gameSettings.selectedResolutionIndex];
        Screen.SetResolution(savedResolution.width, savedResolution.height, SaveState.saveData.gameSettings.isFullscreen);
    }

    public void OnApplicationQuit()
    {
        Debug.Log("Closing TDA..");
        SaveState.SaveGameData();
    }

    void SpawnPlayer()
    {
        if (unlockables.playablePrefabs.Count > 0) {
            // select playerPrefab based on selected skin
            GameObject playerPrefab = unlockables.playablePrefabs[0];
            if (SaveState.saveData.SelectedSkinIndex >= 0 && SaveState.saveData.SelectedSkinIndex < unlockables.playablePrefabs.Count)
            {
                playerPrefab = unlockables.playablePrefabs[SaveState.saveData.SelectedSkinIndex];
            }
            Instantiate(playerPrefab, Spawnpoint); // spawn player        
        }
        else {
            Debug.LogError("No unlockables or no playable prefabs in unlockables SO referenced from GameManager.cs");
        }
    }

    void FindSpawnpoint()
    {
        // get spawnpoint
        Spawnpoint = GameObject.FindWithTag("Spawnpoint").transform;

        if (Spawnpoint == null)
        {
            Debug.LogError("No Spawnpoint in game scene");
        }
    }

    /// <summary>
    /// Loads & spawns playable player in scene
    /// </summary>
    public void LoadPlayer()
    {
        FindSpawnpoint();
        SpawnPlayer();
        controller.OnStart(); // call OnStart() method on BackgroundController
    }
}