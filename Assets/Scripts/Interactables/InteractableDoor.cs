using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDoor : Interactable
{   
    [SerializeField] private Transform outputLocation;
    [SerializeField] private RoomManager outputRoom;

    void Start()
    {

    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        GameEvents.current.ScreenFade(other.transform.GetInstanceID(), outputLocation.position);    //Signal to GameController.cs.
        Debug.Log("Inside the door.");
    }
}
