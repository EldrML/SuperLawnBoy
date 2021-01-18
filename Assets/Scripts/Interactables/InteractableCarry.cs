using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCarry : Interactable
{
    /*
    An extension of the base Interactable class for objects you can pick up in one way or another.
    */

    #region Variables

    protected Transform playerTransform;

    [SerializeField] protected Transform initialParent;

    protected enum CarryStates
    { onGround, isHeld, isThrown }

    [SerializeField] protected CarryStates carryState;

    Vector3 carryHeight = new Vector3(0f, 0.6f, 0f);

    protected SpriteRenderer spriteRenderer;

    protected BoxCollider2D boxCollider;

    #endregion


    #region Setup Logic
    protected virtual void Start()
    //Called before the first frame.
    {
        //Initialize variables.
        carryState = CarryStates.onGround;
        initialParent = transform.parent;

        //Hook up components.
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        //Unsubscribe the listeners first to avoid double triggering.
        SLBEvents.current.onPlayerPickUpCarryable -= PickUpCarryObj;
        SLBEvents.current.onPlayerPutDownCarryable -= PutDownCarryObj;
        SLBEvents.current.onPlayerPickUpCarryable += PickUpCarryObj;    //Subscribe this listener.
        SLBEvents.current.onPlayerPutDownCarryable += PutDownCarryObj;  //Subscribe this listener.
    }

    void OnEnable()
    //Set up events.
    {
        if(SLBEvents.current != null)
        {
            //Unsubscribe the listeners first to avoid double triggering.
            SLBEvents.current.onPlayerPickUpCarryable -= PickUpCarryObj;
            SLBEvents.current.onPlayerPutDownCarryable -= PutDownCarryObj;
            SLBEvents.current.onPlayerPickUpCarryable += PickUpCarryObj;    //Subscribe this listener.
            SLBEvents.current.onPlayerPutDownCarryable += PutDownCarryObj;  //Subscribe this listener.
        }
    }

    private void OnDisable()
    //Remove the listener when the object is disabled.
    {
        SLBEvents.current.onPlayerPickUpCarryable -= PickUpCarryObj;
        SLBEvents.current.onPlayerPutDownCarryable -= PutDownCarryObj;
    }
    #endregion


    #region Event Logic
    protected virtual void PickUpCarryObj(GameObject parentObject, int id)
    {
        if (id == this.GetInstanceID())
        {
            carryState = CarryStates.isHeld;
            boxCollider.enabled = !boxCollider.enabled;             //Turn off BoxCollider while being held.
            spriteRenderer.sortingLayerName = "CarryItem";                  //Make the box visible above other objects.

            playerTransform = parentObject.transform;
            transform.SetParent(parentObject.transform);
            transform.position = playerTransform.position + carryHeight;
        }

    }

    protected virtual void PutDownCarryObj(GameObject parentObject, Vector2 lookDirection, int id)
    {
        if (id == this.GetInstanceID())
        {
            carryState = CarryStates.onGround;
            transform.position = parentObject.transform.TransformPoint(lookDirection);
            spriteRenderer.sortingLayerName = "Interactive";
            boxCollider.enabled = !boxCollider.enabled;                 //Turn on BoxCollider while being released.

            transform.SetParent(initialParent);
            playerTransform = null;
        }
    }

    protected virtual void ThrowCarryObj()
    {
        //Not yet implemented. Source: https://forum.unity.com/threads/find-how-long-a-button-is-being-held.482430/
    }
    #endregion
}
