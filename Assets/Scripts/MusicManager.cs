using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Timeline;


/// <summary>
/// Manages music sound fx
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private List<AudioClip> tracks = new List<AudioClip>(); // available background music tracks to play
    public int selectedTrackIndex; // which track is currently selected?
    public bool shuffleSongs; // true: randomly pick between songs, false: play songs sequentially
    private bool muted; // stops playing music
    private bool playingTrack;
    private AudioSource audioSource;

    public static MusicManager Instance;

    private void Awake()
    {
        // If there's an existing instance, destroy this duplicate
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Make this the singleton instance and don't destroy it on scene load
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (tracks.Count == 0)
        {
            Debug.LogError("No tracks in found MusicManager");
        }
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!muted)
        {
            // if we're not still playing a track
            if (!playingTrack)
            {
                // select next song
                selectedTrackIndex = shuffleSongs ? Random.Range(0, tracks.Count - 1) : (int)Mathf.Clamp((float)selectedTrackIndex, 0, tracks.Count - 1);

                // play song
                audioSource.PlayOneShot(tracks[selectedTrackIndex]);
                StartCoroutine(WaitForTrackToFinish());

            }
        }
    }

    public void ApplySavedSettings()
    {
        // Apply saved volume settings
        SetMasterVolume(Mathf.Lerp(-80, 0, SaveState.saveData.gameSettings.masterVolume / 100));
        SetMusicVolume(Mathf.Lerp(-80, 0, SaveState.saveData.gameSettings.musicVolume / 100));
        SetUIVolume(Mathf.Lerp(-80, 0, SaveState.saveData.gameSettings.uiVolume / 100));
        muted = SaveState.saveData.gameSettings.isMusicMuted;
    }

    IEnumerator WaitForTrackToFinish()
    {
        if (!playingTrack)
        {
            playingTrack = true;
        }

        if (muted)
        {
            audioSource.Stop();
            yield break; // stop playing music
        }

        yield return new WaitForSeconds(tracks[selectedTrackIndex].length);
        playingTrack = false;
    }

    public void SetMasterVolume(float vol)
    {
        SaveState.saveData.gameSettings.masterVolume = (int)vol;
        audioMixer.SetFloat("volume", vol);
    }
    public void SetMusicVolume(float vol)
    {
        SaveState.saveData.gameSettings.musicVolume = (int)vol;
        audioMixer.SetFloat("musicVolume", vol);
    }
    public void SetUIVolume(float vol)
    {
        SaveState.saveData.gameSettings.uiVolume = (int)vol;
        audioMixer.SetFloat("uiVolume", vol);
    }

    public void SetMuted(bool isMuted)
    {
        Debug.Log($"Music muted: {isMuted}");
        SaveState.saveData.gameSettings.isMusicMuted = isMuted;
        muted = SaveState.saveData.gameSettings.isMusicMuted;
    }

    
}
