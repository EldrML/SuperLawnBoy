using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using ExtensionMethods;

public class InteractableCarry : Interactable
{
    /*
    An extension of the base Interactable class for objects you can pick up in one way or another.
    */

    #region Variables

    protected float heightCheck = 0.5f, incrementor = 0f, startTime, throwAdjust = 0.1f;

    protected Vector3 boxEndPosition;

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
        PlayerEvents.current.onPlayerPickUpCarryable   -= PickUpCarryObj;
        PlayerEvents.current.onPlayerPutDownCarryable  -= PutDownCarryObj;
        PlayerEvents.current.onPlayerThrowCarryable    -= ThrowCarryObj;

        PlayerEvents.current.onPlayerPickUpCarryable   += PickUpCarryObj;      //Subscribe listeners.
        PlayerEvents.current.onPlayerPutDownCarryable  += PutDownCarryObj;  
        PlayerEvents.current.onPlayerThrowCarryable    += ThrowCarryObj;   
    }

    protected virtual void Update()
    {
        //Throw animation.
        // if (carryState == CarryStates.isThrown)
        // {
        //ThrowUpdateMotion(transform.position, boxEndPosition);
        // }
    }

    void OnEnable()
    //Set up events.
    {
        if (PlayerEvents.current != null)
        {
            //Unsubscribe the listeners first to avoid double triggering.
            PlayerEvents.current.onPlayerPickUpCarryable   -= PickUpCarryObj;
            PlayerEvents.current.onPlayerPutDownCarryable  -= PutDownCarryObj;
            PlayerEvents.current.onPlayerThrowCarryable    -= ThrowCarryObj;

            PlayerEvents.current.onPlayerPickUpCarryable   += PickUpCarryObj;      //Subscribe listeners.
            PlayerEvents.current.onPlayerPutDownCarryable  += PutDownCarryObj;
            PlayerEvents.current.onPlayerThrowCarryable    += ThrowCarryObj;
        }
    }

    private void OnDisable()
    //Remove the listener when the object is disabled.
    {
        PlayerEvents.current.onPlayerPickUpCarryable   -= PickUpCarryObj;
        PlayerEvents.current.onPlayerPutDownCarryable  -= PutDownCarryObj;
        PlayerEvents.current.onPlayerThrowCarryable    -= ThrowCarryObj;
    }

    #endregion


    #region Carry Logic

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
            carryState          = CarryStates.onGround;
            transform.position  = parentObject.transform.TransformPoint(lookDirection);
            spriteRenderer.sortingLayerName = "CarryItem";
            boxCollider.enabled = !boxCollider.enabled;                 //Turn on BoxCollider while being released.

            transform.SetParent(initialParent);
            playerTransform = null;
        }
    }

    #endregion


    #region Throw Logic

    void ThrowCarryObj(GameObject parentObject, Vector2 throwDirection, int id, float throwtime)
    //Sets up the throw conditions for the item the player is carrying.
    {
        if (id == this.GetInstanceID())
        {
            //throwTime = throwtime * throwDirection.magnitude;                                  //Increase speed of throw depending on length of throw.
            boxEndPosition      = parentObject.transform.TransformPoint(throwDirection);    
            startTime           = Time.time;

            //Remove parent from box.
            transform.SetParent(initialParent);
            playerTransform     = null;

            //Set throw state for enemy checking.
            carryState          = CarryStates.isThrown;

            StartCoroutine(ThrowMotion(transform.position, boxEndPosition, throwtime * throwDirection.magnitude, throwDirection));
        }
    }


    private IEnumerator ThrowMotion(Vector3 startPos, Vector3 endPos, float throwTime, Vector2 throwDirection)
    //Coroutine that moves player by one square when called.
    {
        float fracComplete = 0f;

        while (fracComplete < 1)
        {
            //Slerp throw implementation. #TODO I don't really understand what this math does to be honest.
            Vector3 center = (startPos + endPos) * 0.5f;                                    //Find the midpoint between the start/end.
            center -= new Vector3(0,1,0);                                                   

            Vector3 startRelCenter = startPos - center;
            Vector3 endRelCenter = endPos - center;

            fracComplete = (Time.time - startTime) / throwTime;                             //Bigger throw time = 

            

            //Adjust the height of the box arc.
            if (throwDirection.x != 0)   //Throwing left or right.
            {
                transform.position = Vector3.Slerp(startRelCenter, endRelCenter, fracComplete); //Perform the Slerp
                transform.position -= new Vector3(  0f,           
                                                    throwAdjust * transform.position.y,
                                                    0f);
                transform.position += center;
            }
            else                        //Throwing up or down.
            {
                transform.position = Vector3.Slerp(startRelCenter, endRelCenter, fracComplete); //Perform the Slerp
                transform.position += center;
            }

            yield return null;
        }

        transform.position = endPos;    //Clamp to grid

        carryState                      = CarryStates.onGround;
        boxCollider.enabled             = !boxCollider.enabled;                 //Turn off BoxCollider while being held.
        spriteRenderer.sortingLayerName = "CarryItem";

        //Remove parent from object so it stays in place.
        transform.SetParent(initialParent);
        playerTransform                 = null;

        //Check if there is an interaction below the item.
        if (carryState == CarryStates.isThrown)
        {
            CheckBelowObject();
        }
    }


    protected private virtual void CheckBelowObject()
    {
        //If nothing below the object:
        carryState = CarryStates.onGround;

        Debug.Log("Not yet implemented.");
    }

    #endregion
}