using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Authentication.ExtendedProtection;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;

[RequireComponent(typeof(Animator))]
public class Spring : MonoBehaviour
{

    [SerializeField] private float springEnergy; // force applied to character
    [SerializeField] private Transform topOfSpring; // top of spring
    private Animator animator;
    public bool springCompressed;
    public bool somethingOnSpring;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Debug.DrawRay(topOfSpring.position, Vector3.up * 0.01f, Color.red);
        somethingOnSpring = BeingCompressed();
        if (somethingOnSpring && !springCompressed)
        {
            springCompressed = true;
            UpdateAnimator();
            StartCoroutine(Extend());
        }
        else if (!somethingOnSpring && springCompressed)
        {
            springCompressed = false;
            UpdateAnimator();
        }

    }

    void UpdateAnimator() => animator.SetBool("Compressed", springCompressed);

    RaycastHit2D CheckRay()
    {
        var leftPos = new Vector2(topOfSpring.position.x - 0.3f, topOfSpring.position.y);
        var rightPos = new Vector2(topOfSpring.position.x + 0.3f, topOfSpring.position.y);

        RaycastHit2D hit = Physics2D.Raycast(topOfSpring.position, Vector3.up, 0.01f);
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

    bool BeingCompressed()
    {
        var hit = CheckRay();
        return hit.transform != null && hit.transform.gameObject.GetComponent<Rigidbody2D>();
    }

    IEnumerator Extend()
    {
        yield return new WaitForSeconds(0.25f);
        springCompressed = false;

        var hit = CheckRay();
        if (hit.transform != null)
        {
            Rigidbody2D body;
            hit.transform.TryGetComponent<Rigidbody2D>(out body);
            if (body != null)
            {
                body.velocity = new Vector2(body.velocity.x, 0); // reset y velocity
                // Launch body
                body.AddForce(transform.TransformDirection(Vector2.up) * springEnergy, ForceMode2D.Impulse);
            }
        }

    }

}
