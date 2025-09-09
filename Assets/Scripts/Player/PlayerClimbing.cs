using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerClimbing : MonoBehaviour
{
    private GameObject nearClimbable; // climable object
    private bool canClimb; // is the player able to start climbing?
    private Rigidbody2D body;
    private PlayerMovement playerMovement;
    private float defaultGravity = 0f;
    void ListenForClimb()
    {
        if (canClimb && nearClimbable != null)
        {
            playerMovement.isClimbing = Input.GetKey(InputBinds.climbKey);
        }
        else
        {
            playerMovement.isClimbing = false;
        }
        HandleClimbing();
    }

    public void SetClimable(GameObject climbable)
    {
        nearClimbable = climbable;
        canClimb = nearClimbable != null;
    }

    public void HandleClimbing() => body.gravityScale = playerMovement.isClimbing ? 0 : defaultGravity;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        defaultGravity = body.gravityScale;
        playerMovement = GetComponent<PlayerMovement>();
    }
    
    void Update() => ListenForClimb();

}