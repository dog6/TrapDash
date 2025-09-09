using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class SettingsUIHandler : MonoBehaviour
{

    public GameManager gameManager;
    public GameObject MainMenu, PauseMenu;
    private UIDocument ui;
    private VisualElement root;
    private DropdownField ResolutionDropdown;
    private DropdownField GraphicsDropdown;
    private Slider VolumeSlider, MusicVolumeSlider, UIVolumeSlider;
    private Toggle FullscreenToggle, MusicMuteToggle;
    private Button BackButton;
    private bool gameFullscreen;
    private bool musicMuted;
    private Resolution[] availableResolutions;
    public AudioMixer audioMixer;

    private void OnEnable()
    {
        if (MainMenu != null && MainMenu.activeInHierarchy)
        {
            MainMenu.SetActive(false);
        }

        if (PauseMenu != null && PauseMenu.activeInHierarchy)
        {
            PauseMenu.SetActive(false);
        }

        ReferenceUIElements();
        PopulateResolutionDropdown();
        SyncMenuWithSettings();
        RegisterCallbacks();
    }
    private void OnDisable()
    {
        UnregisterCallbacks();
    }
    private void SyncMenuWithSettings()
    {
        // Sync dropdowns
        GraphicsDropdown.value = QualitySettings.names[QualitySettings.GetQualityLevel()];
        ResolutionDropdown.value = $"{Screen.currentResolution.width}x{Screen.currentResolution.height}";

        // Sync volume sliders
        float masterSliderValue;
        audioMixer.GetFloat("volume", out masterSliderValue);
        VolumeSlider.value = masterSliderValue;

        float musicSliderValue;
        audioMixer.GetFloat("musicVolume", out musicSliderValue);
        MusicVolumeSlider.value = musicSliderValue;

        float uiSliderValue;
        audioMixer.GetFloat("uiVolume", out uiSliderValue);
        UIVolumeSlider.value = uiSliderValue;

        // Sync  fullscreen toggle
        gameFullscreen = SaveState.saveData.gameSettings.isFullscreen;
        musicMuted = SaveState.saveData.gameSettings.isMusicMuted;
        FullscreenToggle.value = gameFullscreen;
        MusicMuteToggle.value = musicMuted;
    }
    private void PopulateResolutionDropdown()
    {
        ResolutionDropdown.choices.Clear();
        availableResolutions = Screen.resolutions;
        foreach (var res in availableResolutions)
        {
            ResolutionDropdown.choices.Add(res.width + " x " + res.height);
        }
    }
    private void ReferenceUIElements()
    {
        ui = GetComponent<UIDocument>();
        root = ui.rootVisualElement;

        ResolutionDropdown = root.Q<DropdownField>("ResolutionDropdown");
        GraphicsDropdown = root.Q<DropdownField>("GraphicsDropdown");

        VolumeSlider = root.Q<Slider>("VolumeSlider");
        MusicVolumeSlider = root.Q<Slider>("MusicVolumeSlider");
        UIVolumeSlider = root.Q<Slider>("UIVolumeSlider");

        FullscreenToggle = root.Q<Toggle>("FullscreenToggle");
        MusicMuteToggle = root.Q<Toggle>("MutedToggle");

        BackButton = root.Q<Button>("BackButton");
    }
    private void RegisterCallbacks()
    {
        // Dropdown callbacks
        GraphicsDropdown.RegisterCallback<ChangeEvent<int>>(OnGraphicsDropdownChanged);
        ResolutionDropdown.RegisterCallback<ChangeEvent<int>>(OnResolutionDropdownChanged);

        // Slider callbacks
        VolumeSlider.RegisterCallback<ChangeEvent<float>>(OnVolumeSliderChange);
        MusicVolumeSlider.RegisterCallback<ChangeEvent<float>>(OnMusicVolumeSliderChange);
        UIVolumeSlider.RegisterCallback<ChangeEvent<float>>(OnUIVolumeSliderChange);

        // Button callbacks
        BackButton.RegisterCallback<ClickEvent>(OnBackButtonPressed);

        // Toggle callback
        // FullscreenToggle.RegisterCallback<PointerDownEvent>(OnFullscreenToggleChanged);
        MusicMuteToggle.RegisterValueChangedCallback(OnMutedToggleChanged);
        MusicMuteToggle.RegisterValueChangedCallback(OnMutedToggleChanged);
    }
    private void UnregisterCallbacks()
    {
        // Dropdown callbacks
        GraphicsDropdown.UnregisterCallback<ChangeEvent<int>>(OnGraphicsDropdownChanged);
        ResolutionDropdown.UnregisterCallback<ChangeEvent<int>>(OnResolutionDropdownChanged);

        // Slider callbacks
        VolumeSlider.UnregisterCallback<ChangeEvent<float>>(OnVolumeSliderChange);
        MusicVolumeSlider.UnregisterCallback<ChangeEvent<float>>(OnMusicVolumeSliderChange);
        UIVolumeSlider.UnregisterCallback<ChangeEvent<float>>(OnUIVolumeSliderChange);

        // Button callbacks
        BackButton.UnregisterCallback<ClickEvent>(OnBackButtonPressed);

        // Toggle callback
        FullscreenToggle.UnregisterValueChangedCallback(OnFullscreenToggleChanged);
        MusicMuteToggle.UnregisterValueChangedCallback(OnMutedToggleChanged);
    }
    private void OnResolutionDropdownChanged(ChangeEvent<int> t)
    {
        var newResolution = availableResolutions[t.newValue];
        Screen.SetResolution(newResolution.width, newResolution.height, gameFullscreen);
    }
    private void OnVolumeSliderChange(ChangeEvent<float> t) => audioMixer.SetFloat("volume", t.newValue);
    private void OnMusicVolumeSliderChange(ChangeEvent<float> t)
    {
        gameManager.musicManager.SetMusicVolume(t.newValue);
    }
    private void OnUIVolumeSliderChange(ChangeEvent<float> t)
    {
        SaveState.saveData.gameSettings.uiVolume = (int)t.newValue;
        audioMixer.SetFloat("uiVolume", SaveState.saveData.gameSettings.uiVolume);
    } 
    private void OnGraphicsDropdownChanged(ChangeEvent<int> t) => QualitySettings.SetQualityLevel(t.newValue);
    private void OnFullscreenToggleChanged(ChangeEvent<bool> evt) => gameFullscreen = FullscreenToggle.value;
    private void OnMutedToggleChanged(ChangeEvent<bool> evt)
    {
        gameManager.musicManager.SetMuted(MusicMuteToggle.value);

    }
    private void OnBackButtonPressed(ClickEvent evt)
    {
        SaveState.SaveGameData(); // save new settings
        if (MainMenu != null)
        {
            MainMenu.SetActive(true);
        }

        if (PauseMenu != null)
        {
            PauseMenu.SetActive(true);
        }

        Debug.Log("Returning to main menu..");
    }

}
