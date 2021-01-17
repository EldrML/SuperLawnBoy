using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCarry : Interactable
{
    [SerializeField] int id;
    Transform playerTransform;
    enum CarryStates
    {
        onGround, isHeld, isThrown
    }
    [SerializeField] CarryStates carryState;
    Vector3 carryHeight = new Vector3(0f, 0.6f, 0f);
    
    SpriteRenderer sprite;
    BoxCollider2D boxCollider;

//----------------------------------------------------

    protected virtual void Start()
    //Called before the first frame.
    {
        carryState = CarryStates.onGround;
        sprite = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        SLBEvents.current.onPlayerPickUpCarryable += PickUpCarryObj; //Add this to your list of subscribed events.
    }

    void Update()
    {
        //Box is being carried above player.
        if (carryState == CarryStates.isHeld)
        {
            transform.position = playerTransform.position + carryHeight;
        }
    }

    private void PickUpCarryObj(GameObject parentObject)
    {
        playerTransform         = parentObject.transform;
        carryState              = CarryStates.isHeld;
        boxCollider.enabled     = !boxCollider.enabled;             //Turn off BoxCollider while being held.
        sprite.sortingLayerName = "CarryItem";                  //Make the box visible above other objects.

        transform.SetParent(parentObject.transform);
        Debug.Log("Event here to change animation state");
        //}
    }

    private void OnDestroy()
    //Remove the listener when the object is destroyed.
    {
        SLBEvents.current.onPlayerPickUpCarryable -= PickUpCarryObj;
    }

}
