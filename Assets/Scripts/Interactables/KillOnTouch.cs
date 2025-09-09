using System.Collections.Generic;
using UnityEngine;

public class KillOnTouch : MonoBehaviour
{

    public bool diesAfterTouch;
    public List<string> diesTouchingTags;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            var playerObject = col.transform.gameObject;
            RespawnHandler respawnHandler;
            if (playerObject.TryGetComponent<RespawnHandler>(out respawnHandler))
            {
                respawnHandler.KillPlayer(); // return player to last checkpoint
                Debug.Log($"Player hit {transform.name} and was returned to last checkpoint (#{respawnHandler.GetLastCheckpointNumber()})");
            }
        }

        if (diesAfterTouch && diesTouchingTags.Contains(col.tag))
        {
            Destroy(this.gameObject);
        }
    }

}