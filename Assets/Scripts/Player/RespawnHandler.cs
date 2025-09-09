using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerHUD))]
[RequireComponent(typeof(PlayerSoundHandler))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(CameraShakeController))]
public class RespawnHandler : MonoBehaviour
{

    private Transform spawnPoint; // start of scene
    private Transform lastCheckpoint;
    private PlayerHUD playerHUD;
    private PlayerSoundHandler playerSound;
    private PlayerMovement playerMovement;
    private CameraShakeController cameraShake;
    private BackgroundController backgroundController;
    private bool holdingResetKey;
    [SerializeField] private float resetRunDelay; // how long the player has to hold the reset run key to reset their run
    private RunTimer runTimer;
    private bool isAlive; // is the player 'alive'?

    void Start()
    {
        lastCheckpoint = GameObject.FindWithTag("Spawnpoint").transform;
        backgroundController = GameObject.FindWithTag("Background").GetComponent<BackgroundController>();
        runTimer = GameObject.FindWithTag("HUD").GetComponent<RunTimer>();
        playerHUD = GetComponent<PlayerHUD>();
        playerSound = GetComponent<PlayerSoundHandler>();
        playerMovement = GetComponent<PlayerMovement>();
        cameraShake = GetComponent<CameraShakeController>();
        isAlive = true;
    }

    void Update()
    {
        holdingResetKey = Input.GetKey(InputBinds.resetRunKey);
        ListenForRunReset();

        if (!isAlive)
        {
            ListenForRespawn();
        }

    }

    void ListenForRespawn()
    {
        // Waiting for key press to 'respawn'
        if (Input.anyKeyDown)
        {
            Debug.Log("Respawning player");
            playerHUD.hud.ShowPAKUI(false);
            transform.position = lastCheckpoint != null ? lastCheckpoint.position : spawnPoint.position;
            playerMovement.Unfreeze();
            isAlive = true;
            runTimer.OnRespawn();
            backgroundController.SnapToPlayer();
        }
    }


    public void SetCheckpoint(Transform checkpoint)
    {
        if (lastCheckpoint != null && lastCheckpoint != spawnPoint)
        {
            Checkpoint point;
            if (lastCheckpoint.TryGetComponent<Checkpoint>(out point))
            {
                point.DeactivateCheckpoint();
            }
        }
        lastCheckpoint = checkpoint;
        Debug.Log($"Player reached checkpoint #{lastCheckpoint.GetComponent<Checkpoint>().checkpointNumber}");
        runTimer.OnCheckpoint();
    }

    public void KillPlayer()
    {
        playerHUD.AddDeath();
        playerSound.PlayHitSoundFX();
        runTimer.OnDeath();
        playerMovement.Freeze(); // freeze the player
        cameraShake.ShakeOnDeath();
        playerHUD.hud.ShowPAKUI(true);
        isAlive = false;
    }

    public int GetLastCheckpointNumber()
    {
        if (lastCheckpoint != null && lastCheckpoint != spawnPoint)
        {
            return lastCheckpoint.GetComponent<Checkpoint>().checkpointNumber;
        }
        else
        {
            return 0;
        }
    }
    IEnumerator HoldToResetRun()
    {

        if (holdingResetKey)
        {
            Debug.Log("Resetting run..");
        }
        else
        {
            Debug.Log("Player canceled resetting the current run");
            yield break;
        }

        yield return new WaitForSeconds(resetRunDelay);
        // reset run
        if (holdingResetKey)
        {
            Debug.Log("Player resetting current run..");
            ResetRun();
        }
        else
        {
            Debug.Log("Player canceled resetting the current run");
        }
    }

    void ListenForRunReset() {
        if (holdingResetKey)
        {
            cameraShake.ShakeOnDeath();
            StartCoroutine(HoldToResetRun());
        }
    }

    // Reloads the current scene
    void ResetRun() => SceneLoader.LoadScene(Scene.GameScene);

}
