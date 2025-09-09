using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used by PlayerMovement.cs
[RequireComponent(typeof(Animator))]
public class PlayerAnimator : MonoBehaviour
{

    private Animator animator;

    public bool Crouching;
    public bool Jumping;
    public bool Walking;
    public bool Animate;
    public bool Climbing;

    void Start() => animator = GetComponent<Animator>();

    void Update()
    {
        UpdateAnimator();
    }

    public void UpdatePlayerState(bool crouching, bool jumping, bool walking, bool climbing, bool animate = true)
    {
        this.Crouching = crouching;
        this.Jumping = jumping;
        this.Walking = walking;
        this.Animate = animate;
        this.Climbing = climbing;
    }

    void UpdateAnimator()
    {
        if (Animate)
        {
            animator.SetBool("Crouching", Crouching);
            animator.SetBool("Jumping", Jumping);
            animator.SetBool("Walking", Walking);
            animator.SetBool("Climbing", Climbing);
        }
    }


}
