using UnityEngine;

// Contains settings for game in saveable object
[System.Serializable]
public class Settings
{

    // Default settings
    public int selectedResolutionIndex = 0;
    public int selectedGraphicsIndex = 0; // LOW, MED, HIGH
    public int masterVolume = 50;
    public int musicVolume = 100;
    public int uiVolume = 100;
    public bool isFullscreen = false;
    public bool isMusicMuted = false;

    public Settings() { }
    public Settings(int resolutionIndex, int graphicsIndex, int masterVol, int musicVol, int uiVol, bool fullscreen, bool musicMuted)
    {
        this.selectedResolutionIndex = resolutionIndex;
        this.selectedGraphicsIndex = graphicsIndex;
        this.masterVolume = masterVol;
        this.musicVolume = musicVol;
        this.uiVolume = uiVol;
        this.isFullscreen = fullscreen;
        this.isMusicMuted = musicMuted;
    }

}