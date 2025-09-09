using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HUDHandler))]
public class RunTimer : MonoBehaviour
{

    public float runTime; // duration of playthrough
    public List<float> CheckpointTimemstamps; // time taken to get to each checkpoint
    public List<float> DeathTimestamps; // times player died and was reset to a checkpoint
    public bool isRunning;
    private HUDHandler hud;
    private string timeFormat = @"mm\:ss\.ff";

    public void OnCheckpoint() => CheckpointTimemstamps.Add(runTime);

    public void OnDeath()
    {
        DeathTimestamps.Add(runTime);
        isRunning = false;
    }

    public void OnRespawn() => isRunning = true;

    void UpdateTimer() => runTime += isRunning && !PauseController.GamePaused ? Time.deltaTime : 0f;

    string FormatTime(float time)
    {
        System.TimeSpan t = System.TimeSpan.FromSeconds(time);
        return t.ToString(timeFormat);
    }

    void TryUpdatingTimerUI()
    {
        if (hud != null)
        {
            hud.SetTime(FormatTime(runTime));
        }
    }

    void Start() => hud = GetComponent<HUDHandler>();

    void Update() => UpdateTimer();
    void LateUpdate() => TryUpdatingTimerUI();

}