using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Events;

public class SLBEvents : MonoBehaviour
{
    public static SLBEvents current;

    private void Awake()
    {
        current = this;
    }

    public event Action<GameObject> onPlayerPickUpCarryable;
    public void PlayerPickUpCarryable(GameObject gameObject)
    {
        if (onPlayerPickUpCarryable != null)
        {
            onPlayerPickUpCarryable(gameObject);
        }
    }
    
}
