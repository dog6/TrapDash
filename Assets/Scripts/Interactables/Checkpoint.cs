using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Checkpoint : MonoBehaviour
{
    public int checkpointNumber; // which checkpoint is this?
    public bool activeCheckpoint; // is this the active checkpoint?
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void UpdateAnimator()
    {
        animator.SetBool("FlagRaised", activeCheckpoint);
    }

    void Update() => UpdateAnimator();

    public void DeactivateCheckpoint()
    {
        activeCheckpoint = false;
    }

    private void ActivateCheckpoint(RespawnHandler respawnHandler)
    {
        respawnHandler.SetCheckpoint(this.transform); // set this checkpoint as active checkpoint
        activeCheckpoint = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.CompareTag("Player"))
        {
            GameObject playerObject = col.transform.gameObject;
            RespawnHandler respawnHandler;
            if (playerObject.TryGetComponent<RespawnHandler>(out respawnHandler))
                ActivateCheckpoint(respawnHandler);
        }

    }

}
