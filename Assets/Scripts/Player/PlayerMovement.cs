using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerAnimator))]
[RequireComponent(typeof(PlayerSoundHandler))]
public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed, climbSpeed;
    public float jumpForce;
    public bool isGrounded;
    public bool isClimbing; // is the player climbing something?
    [SerializeField] private Transform playerFeet;
    [SerializeField] private LayerMask GroundLayers;
    [SerializeField] private PlayerFeet feet;
    private PlayerSoundHandler playerSound;
    private Rigidbody2D body;
    private PlayerAnimator animator;
    private bool isJumping;
    private bool isFrozen; // is the player frozen in place?
    private GameObject standingOnObject;
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<PlayerAnimator>();
        playerSound = GetComponent<PlayerSoundHandler>();
    }

    public void Freeze() => isFrozen = true;
    public void Unfreeze() => isFrozen = false;
    bool IsCrouching() => Input.GetKey(InputBinds.crouchKey) && !IsWalking() && !isClimbing;
    bool IsWalking() => Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f;
    void Jump() => body.AddForce(Vector3.up*jumpForce, ForceMode2D.Impulse);
    void ListenForJump()
    {
        if (feet.IsGrounded)
        {
            if (Input.GetKeyDown(InputBinds.jumpKey))
            {
                isJumping = true;
                Jump();
                playerSound.PlayJumpSFX();
                Debug.Log("Player jumped");
            }
            else
            {
                isJumping = false;
            }
        }
    }

    void ListenForPause()
    {
        if (Input.GetKeyDown(InputBinds.pauseKey))
        {
            PauseController.TogglePause();
        }
    }

    void HandleMovement()
    {
        if (!isClimbing && !feet.OnIce)
        {
            body.velocity = new Vector2(Input.GetAxis("Horizontal") * moveSpeed, body.velocity.y);
        }

        if (isClimbing)
        {
            body.velocity = new Vector2(Input.GetAxis("Horizontal") * climbSpeed, Input.GetAxis("Vertical") * climbSpeed);
            ShareVelocityWithPlatform();
        }

        if (feet.OnIce)
        {
            float input = Input.GetAxis("Horizontal");
            float modifier = input >= 0 ? 1 : -1;
            float adjustedSpeed = moveSpeed + modifier * (body.velocity.x / 2);

            body.velocity = new Vector2(input * adjustedSpeed, body.velocity.y);
            //body.velocity = new Vector2(Input.GetAxis("Horizontal") * (moveSpeed+(body.velocity.x/2)), body.velocity.y);
        }

    }

    void ShareVelocityWithPlatform()
    {
        // Rigidbody2D platformBody;
        // standingOnObject.transform.parent.TryGetComponent<Rigidbody2D>(out platformBody);
        // if (standingOnObject != null && platformBody != null)
        
        // {
        // body.velocity += platformBody.velocity;
        // }
    }

    void GetObjectBeingStoodOn()
    {
        RaycastHit2D hit = Physics2D.Raycast(feet.transform.position, Vector2.down, 0.01f, GroundLayers);
        if (hit.transform != null)
        {
            standingOnObject = hit.transform.gameObject;
            transform.SetParent(standingOnObject.transform);
        }
        else
        {
            standingOnObject = null;
            transform.SetParent(null);
        }
    }

    void Update()
    {
        if (!isFrozen && !PauseController.GamePaused)
        {
            HandleMovement();
            ListenForJump();
            body.simulated = true;
        }
        else
        {
            body.simulated = false;
        }

        ListenForPause();
        GetObjectBeingStoodOn();
        animator.UpdatePlayerState(IsCrouching(), isJumping, IsWalking(), isClimbing);
    }

}
