using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

// Describes a save state of the game
[System.Serializable]
public static class SaveState
{
    // Stats
    public static GameSave saveData = new GameSave();
    /// <summary>
    /// Adds to TotalCoinsCollected and SpendableCoins
    /// </summary>
    public static void PickupCoins(int amnt)
    {
        saveData.TotalCoinsCollected += amnt;
        saveData.SpendableCoins += amnt;
    }

    /// <summary>
    /// Attempts to spend amount of coins from players spendable amount
    /// </summary>
    /// <param name="amnt">amount of coins to 'spend'</param>
    /// <returns>True if coins were spent, otherwise returns false</returns>
    public static bool SpendCoins(int amnt)
    {
        if (saveData.SpendableCoins - amnt >= 0)
        {
            saveData.SpendableCoins -= amnt;
            saveData.Save(); // save after spending coins
            Debug.Log($"Spent {amnt} coins");
            return true;
        }
        return false;
    }

    /// <summary>
    ///  Adds runs to PreviousRuns
    /// </summary>
    /// <param name="run">The run to add</param>
    public static void AddRun(RunInfo run) => saveData.PreviousRuns.Add(run);

    public static void ApplyCustomization(int selectedHatIndex, int selectedSkinIndex)
    {
        saveData.SelectedHatIndex = selectedHatIndex;
        saveData.SelectedSkinIndex = selectedSkinIndex;
    }

    public static void SaveGameData() => saveData.Save();

    public static void LoadGameData()
    {
        saveData.Load();
    }

    public static void UnlockSkin(CharacterSkin skin)
    {
        saveData.UnlockedSkins.Add(skin.id);
    }

    public static void UnlockHat(Hat hat)
    {
        saveData.UnlockedHats.Add(hat.id);
    }

}
