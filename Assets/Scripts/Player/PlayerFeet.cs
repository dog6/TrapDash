using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFeet : MonoBehaviour
{
    public bool IsGrounded; // Is the player on the ground?
    public bool OnIce; // Is the player standing on ice?
    [SerializeField] private float jumpWidth; // gap between left and right ray
    [SerializeField] private LayerMask groundLayers;

    bool CheckIf_OnIce()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.01f);
        return hit.transform != null && hit.transform.CompareTag("Ice");
    }

    bool CheckIf_IsGrounded() => Physics2D.OverlapCircle(transform.position, jumpWidth, groundLayers);

    void Update()
    {
        IsGrounded = CheckIf_IsGrounded();
        OnIce = CheckIf_OnIce();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, jumpWidth);
        Gizmos.DrawLine(transform.position, transform.TransformPoint(Vector2.down * 0.01f));
    }

}
