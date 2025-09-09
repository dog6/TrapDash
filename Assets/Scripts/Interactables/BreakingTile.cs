using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BreakingTile : MonoBehaviour
{
    public GameObject Tile;
    [SerializeField] private float  lifespan, // how long it takes for the tile to fully break
                                    respawnDelay; // how long it takes for the tile to re-appear
    [SerializeField] private Transform topOfTile;
    [SerializeField] private List<Sprite> stages;
    /// <summary>
    /// If true, block respawns after respawn delay<br/>
    /// If false, block respawns after scene reload
    /// </summary>
    [SerializeField] private bool doesRespawn = true;
    private SpriteRenderer tileRenderer;
    private Collider2D tileCollider;
    private Vector2 tilePosition;
    private bool beingStoodOn; // is this tile currently breaking?
    private bool isBreaking; // is the tile destroying itself?
    private bool isActive; // is this tile active?
    private bool isHealing; // is the tile already healing?
    private bool isDamaged;
    void Start()
    {
        if (!Tile.TryGetComponent<SpriteRenderer>(out tileRenderer))
        {
            Debug.LogError($"Failed to get sprite renderer on tile GameObject {Tile.name}");
        }

        if (!Tile.TryGetComponent<Collider2D>(out tileCollider))
        {
            Debug.LogError($"Failed to get collider2D on tile GameObject {Tile.name}");
        }
    }
    void CheckIf_BeingStoodOn()
    {
        RaycastHit2D hit = CheckRays();
        beingStoodOn = hit.transform != null && hit.transform.CompareTag("Player");
    }
    void Handle_BeingStoodOn()
    {
        // Only breaks if player is on the tile
        if (beingStoodOn)
        {
            if (!isBreaking) // not already breaking, start breaking
            {
                StartCoroutine(BreakTile());
            }
        }
    }
    void Update()
    {
        isDamaged = tileRenderer.sprite != stages[0];
        CheckIf_BeingStoodOn();
        Handle_BeingStoodOn();
    }
    IEnumerator BreakTile()
    {
        for (int i = 0; i < 5; i++)
        {
            if (!beingStoodOn)
            {
                // stop breaking, not being stood on
                isBreaking = false;
                yield break;
            }
            else
            {
                // being stood on, keep breaking
                isBreaking = true;
                yield return new WaitForSeconds(lifespan / 5);
                // Update progress
                SetTileStage(i);
            }
        }
    }
    void SetTileEnabled(bool state)
    {
        isActive = state;
        tileCollider.enabled = state;
        tileRenderer.enabled = state;

        if (!state && doesRespawn)
        {
            StartCoroutine(StartRespawn());
            // Debug.Log("Killing breakable tile");
            // beingStoodOn = false;
            // isBreaking = false;
        }

    }
    IEnumerator StartRespawn()
    {
        yield return new WaitForSeconds(respawnDelay);
        Debug.Log("Tile has respawned");
        SetTileEnabled(true);
        SetTileStage(0); // reset tile
        isBreaking = false;
    }
    void SetTileStage(int stageIndex){
        if (stageIndex < 4 && stageIndex >= 0)
        {
            SetTileEnabled(true);
            tileRenderer.sprite = stages[stageIndex];
        }
        else if (stageIndex == 4)
        {
            SetTileEnabled(false); // disable tile
        }
    }
    RaycastHit2D CheckRays()
    {
        var leftPos = new Vector2(topOfTile.position.x - 0.3f, topOfTile.position.y);
        var rightPos = new Vector2(topOfTile.position.x + 0.3f, topOfTile.position.y);

        RaycastHit2D hit = Physics2D.Raycast(topOfTile.position, Vector3.up, 0.01f);
        RaycastHit2D leftHit = Physics2D.Raycast(leftPos, Vector3.up, 0.01f);
        RaycastHit2D rightHit = Physics2D.Raycast(rightPos, Vector3.up, 0.01f);

        if (hit.transform != null && hit.transform.gameObject.GetComponent<Rigidbody2D>())
        {
            return hit;
        }
        else if (leftHit.transform != null && leftHit.transform.gameObject.GetComponent<Rigidbody2D>())
        {
            return leftHit;
        }
        else if (rightHit.transform != null && rightHit.transform.gameObject.GetComponent<Rigidbody2D>())
        {
            return rightHit;
        }
        return hit;
    }

    

}
