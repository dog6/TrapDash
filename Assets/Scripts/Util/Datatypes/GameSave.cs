using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class GameSave
{

    // Game settings
    public Settings gameSettings = new Settings();
    // Stats
    public int TotalCoinsCollected; // all coins ever collected
    public List<RunInfo> PreviousRuns;
    public List<int> UnlockedHats;
    public List<int> UnlockedSkins;

    // Customization variables
    public int SelectedHatIndex;
    public int SelectedSkinIndex;
    public int SpendableCoins;
    public GameSave() { }
    public GameSave(int _totalCoinsCollected, int _selectedHatIndex, int _selectedSkinIndex, int _spendableCoins, List<RunInfo> _previousRuns, List<int> _unlockedSkins, List<int> _unlockedHats, Settings _gameSettings)
    {
        this.TotalCoinsCollected = _totalCoinsCollected;
        this.SelectedHatIndex = _selectedHatIndex;
        this.SelectedSkinIndex = _selectedSkinIndex;
        this.SpendableCoins = _spendableCoins;
        this.PreviousRuns = _previousRuns;
        this.UnlockedSkins = _unlockedSkins;
        this.UnlockedHats = _unlockedHats;
        this.gameSettings = _gameSettings;
    }

    /// <summary>
    /// Serializes and writes savedata into a JSON file<br/>
    /// at a specified path.
    /// </summary>
    /// <param name="path">Path to write savedata.json file</param>
    public void Save(string path = "./savedata.json")
    {
        Debug.Log("Serializing GameSave..");
        string jsonData = JsonUtility.ToJson(this);
        Debug.Log("Writing save data..");
        File.WriteAllText(path, jsonData); // write save data
    }

    /// <summary>
    /// Syncs this objects data with another GameSave
    /// </summary>
    /// <param name="saveData">GameSave to sync up with</param>
    void SyncWithSaveData(GameSave saveData)
    {
        if (saveData != null)
        {
            Debug.Log("Parsing JSON savedata..");
            this.TotalCoinsCollected = saveData.TotalCoinsCollected;
            this.SelectedHatIndex = saveData.SelectedHatIndex;
            this.SelectedSkinIndex = saveData.SelectedSkinIndex;
            this.SpendableCoins = saveData.SpendableCoins;
            this.PreviousRuns = saveData.PreviousRuns;
            this.UnlockedSkins = saveData.UnlockedSkins;
            this.UnlockedHats = saveData.UnlockedHats;
        }
        else
        {
            Debug.Log("Failed to parse save data");
        }
    }

    /// <summary>
    /// Attempts to load savedata from a given path.<br/>
    /// Creates new empty save if unable to find savedata.json file
    /// </summary>
    /// <param name="path">Path to savedata.json file</param>
    public void Load(string path = "./savedata.json")
    {
        if (File.Exists(path)) ReadSaveData(path); else CreateNewSave(path);
    }

    /// <summary>
    /// Reads, parses, and syncs with savedata.json file<br/>
    /// at a specified path
    /// </summary>
    /// <param name="path">Path to savedata.json file</param>
    void ReadSaveData(string path)
    {
        // Read save data
        string data = File.ReadAllText(path);
        Debug.Log($"Reading savedata.. [{path}]");
        var saveData = JsonUtility.FromJson<GameSave>(data);
        SyncWithSaveData(saveData);
    }

    /// <summary>
    /// Creates and writes a new empty savedata.json file<br/>
    /// at a specified path.
    /// </summary>
    /// <param name="path">Path to savedata.json file</param>
    void CreateNewSave(string path)
    {
        // Create new save
        Debug.Log($"Unable to find prior save data. [{path}]");
        PreviousRuns = new List<RunInfo>(); // create new list to store previous runs
        UnlockedSkins = new List<int>() { 0 };
        UnlockedHats = new List<int>() { 0 };
        GameSave newSaveData = new GameSave(0, 0, 0, 0, PreviousRuns, UnlockedSkins, UnlockedHats, new Settings());
        SyncWithSaveData(newSaveData);
        Save(); // save newSaveData to .json 
    }

}