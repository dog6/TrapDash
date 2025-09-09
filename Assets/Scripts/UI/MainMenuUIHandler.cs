using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MainMenuUIHandler : MonoBehaviour
{

    public GameObject CustomizeMenu, SettingsMenu;
    private UIDocument menuUI;
    private VisualElement root;
    private Button playButton, customizeButton, settingsButton, quitButton;
    void OnEnable()
    {
        DisableOtherMenus();
        ReferenceUIElements();
        RegisterCallbacks();
    }

    void DisableOtherMenus()
    {
        if(CustomizeMenu.activeInHierarchy){
            CustomizeMenu.SetActive(false); // disable customization menu
        }

        if(SettingsMenu.activeInHierarchy){
            SettingsMenu.SetActive(false); // disable settings menu
        }
    }

    void OnDisable() => UnregisterCallbacks();

    void ReferenceUIElements()
    {
        menuUI = GetComponent<UIDocument>();
        root = menuUI.rootVisualElement;
        playButton = root.Q<Button>("PlayButton");
        customizeButton = root.Q<Button>("CustomizeButton");
        settingsButton = root.Q<Button>("SettingsButton");
        quitButton = root.Q<Button>("QuitButton");
        PauseController.Unpause(); // unpause game if in main menu
    }

    void RegisterCallbacks()
    {
        playButton.RegisterCallback<ClickEvent>(OnPlayButtonClicked);
        customizeButton.RegisterCallback<ClickEvent>(OnCustomizeButtonClicked);
        settingsButton.RegisterCallback<ClickEvent>(OnSettingsButtonClicked);
        quitButton.RegisterCallback<ClickEvent>(OnExitButtonClicked);
    }

    void UnregisterCallbacks()
    {
        playButton.UnregisterCallback<ClickEvent>(OnPlayButtonClicked);
        customizeButton.UnregisterCallback<ClickEvent>(OnCustomizeButtonClicked);
        settingsButton.UnregisterCallback<ClickEvent>(OnSettingsButtonClicked);
        quitButton.UnregisterCallback<ClickEvent>(OnExitButtonClicked);
    }

    // Button callbacks
    void OnPlayButtonClicked(ClickEvent evt)
    {
        SceneLoader.LoadScene(Scene.GameScene);
    }

    void OnCustomizeButtonClicked(ClickEvent evt) => CustomizeMenu.SetActive(true);

    void OnSettingsButtonClicked(ClickEvent evt) => SettingsMenu.SetActive(true);
    void OnExitButtonClicked(ClickEvent evt) => SceneLoader.CloseGame();

}
