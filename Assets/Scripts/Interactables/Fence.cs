using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform != null && col.CompareTag("Player"))
        {

            PlayerClimbing playerClimbing;
            if (col.transform.gameObject.TryGetComponent<PlayerClimbing>(out playerClimbing))
            {
                playerClimbing.SetClimable(this.gameObject);
            }

        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.transform != null && col.CompareTag("Player"))
        {
            PlayerClimbing playerClimbing;
            if (col.transform.gameObject.TryGetComponent<PlayerClimbing>(out playerClimbing))
            {
                playerClimbing.SetClimable(null);
            }
        }
    }

}
