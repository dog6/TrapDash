using System.Collections.Generic;
using UnityEngine;


// Saved information from a playthrough
public class RunInfo
{
    public int CoinsCollected; // coins player collected
    public int TotalDeaths; // total deaths
    public List<string> DeathTimestamps; // time for each death
    public List<string> CheckpointTimestamps; // time for each checkpoint
    public string TotalTime; // time to complete level

}