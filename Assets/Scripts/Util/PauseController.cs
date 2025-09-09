

using UnityEngine;

public static class PauseController
{

    public static bool GamePaused; // is the game paused?

    public static void Pause()
    {
        GamePaused = true;
        Time.timeScale = 0f;
    }

    public static void Unpause()
    {
        GamePaused = false;
        Time.timeScale = 1f;
    }

    public static void TogglePause()
    {
        if (GamePaused) Unpause(); else Pause();
    }

}