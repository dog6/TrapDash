using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveToggle : MonoBehaviour
{
    public void SetActive(bool state) => transform.gameObject.SetActive(state);
}

