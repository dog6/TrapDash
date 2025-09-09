using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Platform : MonoBehaviour
{
    public bool followingPath; // is this platform stationary or is it following a path?
    public float speed; // speed of platform
    public float dwellTime; // how long a platform stays at a point before going to the next point
    public List<Transform> points; // points platform moves betweens
    public Transform currentPoint;
    private Animator animator;
    private bool inTransit; // is this platform following a set path?
    private bool isDwelling; // is this platform currently dwelling at a point?

    private int currentPointIndex = 0;

    void LogDeparture()
    {
        if (!inTransit)
        {
            // Debug.Log($"Platform is moving to point {points[currentPointIndex].name}");
        }
    }

    void LogArrival()
    {
        if (inTransit)
        {
            // Debug.Log($"Platform arrived at point {points[currentPointIndex].name}");
        }
    }

    void Start()
    {
        currentPoint = points[0];
        // Debug.Log($"Found {points.Count} points");
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        if (transform.position != currentPoint.position)
        {
            // move towards next point
            LogDeparture();
            transform.position = Vector3.MoveTowards(transform.position, currentPoint.position, speed * Time.deltaTime);
            inTransit = true;

        }
        else if (!isDwelling && transform.position == currentPoint.position)
        {
            // at point
            LogArrival();
            inTransit = false;

            if (!isDwelling)
            {
                StartCoroutine(Dwell()); // dwell at point for a few seconds
            }

        }

        UpdateAnimator();

    }

    IEnumerator Dwell()
    {
        // Debug.Log($"Dwelling at point {points[currentPointIndex].name}");
        isDwelling = true;
        yield return new WaitForSeconds(dwellTime);


        int previousPoint = currentPointIndex;
        currentPointIndex = (currentPointIndex + 1) % points.Count;
        // Debug.Log($"Changing current point from {points[previousPoint].name} to {points[currentPointIndex].name}");

        currentPoint = points[currentPointIndex];
        isDwelling = false;
    }

    void UpdateAnimator()
    {
        animator.SetBool("InTransit", inTransit);
    }

}
