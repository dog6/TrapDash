using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class EventTrigger : MonoBehaviour
{

    private Collider2D collider;
    public List<UnityEvent> OnEnterActions, OnExitActions;

    void InvokeAllActions(List<UnityEvent> actions)
    {
        foreach (var action in actions)
        {
            action.Invoke();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform != null && col.transform.CompareTag("Player"))
        {
            InvokeAllActions(OnEnterActions);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.transform != null && col.transform.CompareTag("Player"))
        {
            InvokeAllActions(OnExitActions);
        }
    }

    void Start()
    {
        collider = GetComponent<Collider2D>();
        collider.isTrigger = true;
    }

}