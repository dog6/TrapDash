using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class CustomizeUIHandler : MonoBehaviour
{

    public GameObject MainMenu;
    [SerializeField] private Unlockables unlockables;
    private CharacterSkin selectedSkin;
    private Hat selectedHat;
    private int selectedHatIndex;
    private int selectedSkinIndex;
    private UIDocument ui;
    private VisualElement root, HatPreview, SkinPreview, SkinLocked, HatLocked;
    private Button NextHatButton, NextSkinButton, PreviousHatButton, PreviousSkinButton, SaveButton, BackButton, BuyHatButton, BuySkinButton;
    private Label HatLabel, SkinLabel, CoinLabel;

    void OnEnable()
    {
        if (MainMenu.activeInHierarchy)
        {
            MainMenu.SetActive(false); // disable main menu
        }

        ReferenceUIElements();
        selectedHatIndex = SaveState.saveData.SelectedHatIndex;
        selectedSkinIndex = SaveState.saveData.SelectedSkinIndex;
        UpdateHatPreview();
        UpdateSkinPreview();
        UpdateSpendableCoins();
        RegisterCallbacks();
    }
    bool IsSelectedHatUnlocked() => selectedHat != null && SaveState.saveData.UnlockedHats.Exists(hat => hat== selectedHat.id);
    bool IsSelectedSkinUnlocked() => selectedSkin != null && SaveState.saveData.UnlockedSkins.Exists(skin => skin == selectedSkin.id);
    void UpdateHatPreview()
    {
        selectedHat = unlockables.hats[selectedHatIndex] != null ? unlockables.hats[selectedHatIndex] : unlockables.hats[0];

        // Update hat preview and check if selected hat is unlocked
        HatPreview.style.backgroundImage = Background.FromSprite(selectedHat.previewSprite);
        bool hatUnlocked = IsSelectedHatUnlocked();

        // Enable buy button if player has enough coins
        BuyHatButton.SetEnabled(!IsSelectedHatUnlocked() && SaveState.saveData.SpendableCoins >= selectedHat.cost);

        HatLocked.style.display = hatUnlocked ? DisplayStyle.None : DisplayStyle.Flex; // display/hide lock
        HatLabel.text = hatUnlocked ? $"HAT:\n{selectedHat.name}" : $"HAT:\n{selectedHat.name}\nCOST: ${selectedHat.cost}"; // update hat label
    }
    void UpdateSkinPreview()
    {
        selectedSkin = unlockables.skins[selectedSkinIndex] != null ? unlockables.skins[selectedSkinIndex] : unlockables.skins[0];

        SkinPreview.style.backgroundImage = Background.FromSprite(unlockables.skins[selectedSkinIndex].previewSprite);

        bool skinUnlocked = IsSelectedSkinUnlocked();

        BuySkinButton.SetEnabled(!skinUnlocked && SaveState.saveData.SpendableCoins >= selectedSkin.cost);

        // Lock skin if not unlocked
        SkinLocked.style.display = skinUnlocked ? DisplayStyle.None : DisplayStyle.Flex;
        // Update skin label
        SkinLabel.text = skinUnlocked ? $"SKIN:\n{selectedSkin.name}" : $"SKIN:\n{selectedSkin.name}\nCOST: ${selectedSkin.cost}";
    }
    void UpdateSpendableCoins() => CoinLabel.text = $"Coins: ${SaveState.saveData.SpendableCoins}";
    void OnDisable() => UnregisterCallbacks();
    void ReferenceUIElements()
    {
        ui = GetComponent<UIDocument>();
        root = ui.rootVisualElement;

        // Reference buttons
        NextHatButton = root.Q<Button>("NextHatButton");
        NextSkinButton = root.Q<Button>("NextSkinButton");
        PreviousHatButton = root.Q<Button>("PreviousHatButton");
        PreviousSkinButton = root.Q<Button>("PreviousSkinButton");
        BackButton = root.Q<Button>("BackButton");
        BuyHatButton = root.Q<Button>("PurchaseHatButton");
        BuySkinButton = root.Q<Button>("PurchaseSkinButton");

        // Reference Visual Elements
        HatPreview = root.Q<VisualElement>("Hat");
        SkinPreview = root.Q<VisualElement>("Character");
        SkinLocked = root.Q<VisualElement>("PurchaseSkinElement");
        HatLocked = root.Q<VisualElement>("PurchaseHatElement");

        // Reference labels
        HatLabel = root.Q<Label>("HatLabel");
        SkinLabel = root.Q<Label>("SkinLabel");
        CoinLabel = root.Q<Label>("SpendableCoinsLabel");

    }
    void RegisterCallbacks()
    {
        NextHatButton.RegisterCallback<ClickEvent>(OnNextHatButtonClicked);
        PreviousHatButton.RegisterCallback<ClickEvent>(OnPreviousHatButtonClicked);
        NextSkinButton.RegisterCallback<ClickEvent>(OnNextSkinButtonClickedClicked);
        PreviousSkinButton.RegisterCallback<ClickEvent>(OnPreviousSkinButtonClicked);
        BackButton.RegisterCallback<ClickEvent>(OnBackButtonClicked);
        BuyHatButton.RegisterCallback<ClickEvent>(OnPurchaseHatButtonClicked);
        BuySkinButton.RegisterCallback<ClickEvent>(OnPurchaseSkinButtonClicked);

    }
    void UnregisterCallbacks()
    {
        NextHatButton.UnregisterCallback<ClickEvent>(OnNextHatButtonClicked);
        PreviousHatButton.UnregisterCallback<ClickEvent>(OnPreviousHatButtonClicked);
        NextSkinButton.UnregisterCallback<ClickEvent>(OnNextSkinButtonClickedClicked);
        PreviousSkinButton.UnregisterCallback<ClickEvent>(OnPreviousSkinButtonClicked);
        BackButton.UnregisterCallback<ClickEvent>(OnBackButtonClicked);
        BuyHatButton.UnregisterCallback<ClickEvent>(OnPurchaseHatButtonClicked);
        BuySkinButton.UnregisterCallback<ClickEvent>(OnPurchaseSkinButtonClicked);
    }
    void OnNextHatButtonClicked(ClickEvent evt)
    {
        selectedHatIndex = (int)Mathf.Repeat(selectedHatIndex + 1, unlockables.hats.Count);
        OnSelectedHatChanged();
    }
    void OnPreviousHatButtonClicked(ClickEvent evt)
    {
        selectedHatIndex = (int)Mathf.Repeat(selectedHatIndex - 1, unlockables.hats.Count);
        OnSelectedHatChanged();
    }
    void OnNextSkinButtonClickedClicked(ClickEvent evt)
    {
        selectedSkinIndex = (int)Mathf.Repeat(selectedSkinIndex + 1, unlockables.skins.Count);
        OnSelectedSkinChanged();
    }
    void OnPreviousSkinButtonClicked(ClickEvent evt)
    {
        selectedSkinIndex = (int)Mathf.Repeat(selectedSkinIndex - 1, unlockables.skins.Count);
        OnSelectedSkinChanged();
    }
    void OnSelectedHatChanged()
    {
        // Update selected hat
        selectedHat = unlockables.hats[selectedHatIndex];
        UpdateHatPreview();
    }
    void OnSelectedSkinChanged()
    {
        // Update selected skin
        selectedSkin = unlockables.skins[selectedSkinIndex];
        UpdateSkinPreview();

    }
    void OnBackButtonClicked(ClickEvent evt)
    {
        SaveCustomization();
        MainMenu.SetActive(true);
        Debug.Log("Returning to main menu..");
    }
    void OnPurchaseSkinButtonClicked(ClickEvent evt)
    {
        if (!IsSelectedSkinUnlocked()) // skin is locked
            // player spent coins & has enough to spend
            if (SaveState.SpendCoins(selectedSkin.cost))
            {
                Debug.Log($"Player unlocked the {selectedSkin.name} skin");
                SaveState.UnlockSkin(selectedSkin);
                UpdateSkinPreview();
            }
            else
                Debug.Log($"Player tried to buy skin {selectedSkin.name} but didn't have enough coins. Needs {selectedSkin.cost - SaveState.saveData.SpendableCoins} more coins");
    }
    void OnPurchaseHatButtonClicked(ClickEvent evt)
    {
        if (!IsSelectedHatUnlocked())
        {
            if (SaveState.SpendCoins(selectedHat.cost))
            {
                Debug.Log($"Player unlocked the {selectedHat.name} hat");
                SaveState.UnlockHat(selectedHat);
                UpdateHatPreview();
            }
            else
                Debug.Log($"Player tried to buy hat {selectedHat.name} but didn't have enough coins. Needs {selectedSkin.cost - SaveState.saveData.SpendableCoins} more coins");
        }
    }
    void SaveCustomization()
    {
        int hatIndex = SaveState.saveData.UnlockedHats.Exists(hat => hat == unlockables.skins[selectedHatIndex].id) ? selectedSkinIndex : 0;
        int skinIndex = SaveState.saveData.UnlockedSkins.Exists(skin => skin == unlockables.skins[selectedSkinIndex].id) ? selectedSkinIndex : 0;
        SaveState.ApplyCustomization(hatIndex, skinIndex);
    }
}
