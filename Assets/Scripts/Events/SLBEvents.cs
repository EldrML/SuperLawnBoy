using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Events;

public class SLBEvents : MonoBehaviour
{
    #region Setup Logic
    public static SLBEvents current;

    private void Awake() 
    { current = this; }
    #endregion


    public event Action<GameObject, int> onPlayerPickUpCarryable;
    public void PlayerPickUpCarryable(GameObject gameObject, int id)
    { 
        onPlayerPickUpCarryable?.Invoke(gameObject, id);
    }


    public event Action<GameObject, Vector2, int> onPlayerPutDownCarryable;
    public void PlayerPutDownCarryable(GameObject gameObject, Vector2 lookDirection, int id)
    { 
        onPlayerPutDownCarryable?.Invoke(gameObject, lookDirection, id);
    }


    public event Action<GameObject, Vector2, int> onPlayerThrowCarryable;
    public void PlayerThrowsCarryable(GameObject gameObject, Vector2 lookDirection, int id)
    { 
        onPlayerThrowCarryable?.Invoke(gameObject, lookDirection, id);
    }


    public event Action<int> onPlayerReadInteractable;
    public void PlayerReadInteractable(int id)
    { 
        onPlayerReadInteractable?.Invoke(id); 
    }
    
}
