using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class BackgroundController : MonoBehaviour
{
    [SerializeField] private bool LerpOverTime;
    [SerializeField] private float colorCycleDuration;
    [SerializeField] private float backgroundSpeed; // how fast background follows player
    [SerializeField] private Gradient colorGradient;
    [SerializeField] private Transform topOfLevel;
    private Transform player;
    private Tilemap tilemap;
    float timer = 0f;

    /// <summary>
    ///  Called by GameManager
    /// </summary>
    public void OnStart()
    {
        tilemap = GetComponent<Tilemap>();

        if (SceneManager.GetActiveScene().name == Scene.GameScene.ToString())
        {
            player = GameObject.FindWithTag("Player").transform;
            SnapToPlayer();
        }

    }

    public void SnapToPlayer()
    {
        if (player != null)
        transform.position = player.transform.position;
    }

    void FollowPlayer()
    {
        if (player != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, backgroundSpeed * Time.deltaTime);
        }
    }

    void Update()
    {
        FollowPlayer();
        HandleColorShift();
    }

    void HandleColorShift()
    {
        float t;
        if (!LerpOverTime)
        {
            if (player == null)
            {
                Debug.Log("No player reference");
            }

            if (topOfLevel == null)
            {
                Debug.Log("No top of level reference");
            }
      
            t = player.position.y / topOfLevel.position.y * 100;
        }
        else
        {
            timer += Time.deltaTime;
            t = (timer % colorCycleDuration) / colorCycleDuration;
        }
        ShiftColor(t);
    }

    void ShiftColor(float t)
    {
        Color newColor = colorGradient.Evaluate(t);
        if (tilemap != null)
        {
            BoundsInt bounds = tilemap.cellBounds;

            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    if (tilemap.HasTile(pos))
                    {
                        tilemap.SetColor(pos, newColor);
                    }
                }
            }
        }

    }

}
