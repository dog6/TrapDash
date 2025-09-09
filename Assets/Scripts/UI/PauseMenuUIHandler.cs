using System.ComponentModel.Design.Serialization;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UIElements;


[RequireComponent(typeof(UIDocument))]
public class PauseMenuUIHandler : MonoBehaviour
{

    public GameObject SettingsUI;
    private UIDocument ui;
    private VisualElement root;
    private Button resumeButton, settingsButton, menuButton, quitButton;
    private bool pauseState;
    void OnEnable()
    {
        ReferenceUIElements();
        if (SettingsUI.activeInHierarchy)
        {
            SettingsUI.SetActive(false);
        }
        RegisterCallbacks();

    }
    void OnDisable() => UnregisterCallbacks();

    void OnPauseStateChanged()
    {
        root.style.display = pauseState ? DisplayStyle.Flex : DisplayStyle.None;
    }

    void Update()
    {
        if (pauseState != PauseController.GamePaused)
        {
            pauseState = PauseController.GamePaused;
            OnPauseStateChanged();
        }
    }

    void ReferenceUIElements()
    {
        Debug.Log("PauseMenuUI elements referenced.");
        ui = GetComponent<UIDocument>();
        root = ui.rootVisualElement;
        resumeButton = root.Q<Button>("ResumeButton");
        settingsButton = root.Q<Button>("SettingsButton");
        menuButton = root.Q<Button>("MenuButton");
        quitButton = root.Q<Button>("QuitButton");
        OnPauseStateChanged();
    }


    void RegisterCallbacks()
    {
        Debug.Log("PauseMenuUI Callbacks Registered");
        resumeButton.RegisterCallback<ClickEvent>(OnResumeButtonClicked);
        settingsButton.RegisterCallback<ClickEvent>(OnSettingsButtonClicked);
        menuButton.RegisterCallback<ClickEvent>(OnMenuButtonClicked);
        quitButton.RegisterCallback<ClickEvent>(OnQuitButtonClicked);
    }

    void UnregisterCallbacks()
    {
        Debug.Log("PauseMenuUI Callbacks Unregistered");
        resumeButton.UnregisterCallback<ClickEvent>(OnResumeButtonClicked);
        settingsButton.UnregisterCallback<ClickEvent>(OnSettingsButtonClicked);
        menuButton.UnregisterCallback<ClickEvent>(OnMenuButtonClicked);
        quitButton.UnregisterCallback<ClickEvent>(OnQuitButtonClicked);
    }

    void OnResumeButtonClicked(ClickEvent evt)
    {
        Debug.Log("Resume button clicked");
        PauseController.TogglePause();
    }

    void OnSettingsButtonClicked(ClickEvent evt)
    {
        SettingsUI.SetActive(true);
    }

    void OnMenuButtonClicked(ClickEvent evt)
    {
        Debug.Log("Menu button clicked");
        // save game
        SaveState.SaveGameData();
        SceneLoader.LoadScene(Scene.MainMenu);
    }

    void OnQuitButtonClicked(ClickEvent evt) => SceneLoader.CloseGame();


}
