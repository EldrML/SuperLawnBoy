using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableCarry : Interactable
{
    /*
    An extension of the base Interactable class for objects you can pick up in one way or another.
    */

    #region Variables

    protected float throwTime = 0.25f, heightCheck = 0.5f, incrementor = 0f, arcSpeed = 2.0f, startTime;

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
        SLBEvents.current.onPlayerPickUpCarryable   -= PickUpCarryObj;
        SLBEvents.current.onPlayerPutDownCarryable  -= PutDownCarryObj;
        SLBEvents.current.onPlayerThrowCarryable    -= ThrowCarryObj;

        SLBEvents.current.onPlayerPickUpCarryable   += PickUpCarryObj;      //Subscribe listeners.
        SLBEvents.current.onPlayerPutDownCarryable  += PutDownCarryObj;  
        SLBEvents.current.onPlayerThrowCarryable    += ThrowCarryObj;   
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
        if (SLBEvents.current != null)
        {
            //Unsubscribe the listeners first to avoid double triggering.
            SLBEvents.current.onPlayerPickUpCarryable   -= PickUpCarryObj;
            SLBEvents.current.onPlayerPutDownCarryable  -= PutDownCarryObj;
            SLBEvents.current.onPlayerThrowCarryable    -= ThrowCarryObj;

            SLBEvents.current.onPlayerPickUpCarryable   += PickUpCarryObj;      //Subscribe listeners.
            SLBEvents.current.onPlayerPutDownCarryable  += PutDownCarryObj;
            SLBEvents.current.onPlayerThrowCarryable    += ThrowCarryObj;
        }
    }

    private void OnDisable()
    //Remove the listener when the object is disabled.
    {
        SLBEvents.current.onPlayerPickUpCarryable   -= PickUpCarryObj;
        SLBEvents.current.onPlayerPutDownCarryable  -= PutDownCarryObj;
        SLBEvents.current.onPlayerThrowCarryable    -= ThrowCarryObj;
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
            carryState          = CarryStates.onGround;
            transform.position  = parentObject.transform.TransformPoint(lookDirection);
            spriteRenderer.sortingLayerName = "Interactive";
            boxCollider.enabled = !boxCollider.enabled;                 //Turn on BoxCollider while being released.

            transform.SetParent(initialParent);
            playerTransform = null;
        }
    }


    void ThrowCarryObj(GameObject parentObject, Vector2 throwDirection, int id)
    //Sets up the throw conditions for the item the player is carrying.
    {
        if (id == this.GetInstanceID())
        {
            throwTime = 0.125f * throwDirection.magnitude;
            Debug.Log(throwTime);
            boxEndPosition      = parentObject.transform.TransformPoint(throwDirection);     //
            startTime           = Time.time;                                                      //
            transform.SetParent(initialParent);
            playerTransform     = null;
            carryState          = CarryStates.isThrown;
            //boxCollider.offset  = throwDirection;
            //boxCollider.enabled = !boxCollider.enabled;                 //Turn off BoxCollider while being held.

            StartCoroutine(ThrowMotion(transform.position, boxEndPosition));
        }
    }


    // protected virtual void ThrowUpdateMotion(Vector3 startPos, Vector3 endPos)
    // {
    //     //Implement charge throw: https://forum.unity.com/threads/find-how-long-a-button-is-being-held.482430/

    //     if (carryState == CarryStates.isThrown)
    //     {
    //         //Slerp throw implementation.
    //         Vector3 center = (startPos + endPos) * 0.5f;
    //         center -= new Vector3(0,1,0);

    //         Vector3 startRelCenter = startPos - center;
    //         Vector3 endRelCenter = endPos - center;


    //         float fracComplete = (Time.time - startTime) / throwTime * arcSpeed;
    //         Debug.Log("start time: " + startTime + " frac complete: " + fracComplete);

    //         transform.position = Vector3.Slerp(startRelCenter, endRelCenter, fracComplete);
    //         transform.position += center;

    //         // boxCollider.offset = new Vector2 ( Vector3.Slerp(startRelCenter, endRelCenter, fracComplete).x,
    //         //                                     Vector3.Slerp(startRelCenter, endRelCenter, fracComplete).y-1f );
            

    //         // if (transform.position == endPos)
    //         // //End conditions for the Slerp.
    //         // {
    //         //     carryState = CarryStates.onGround;
    //         //     boxCollider.enabled = !boxCollider.enabled;                 //Turn off BoxCollider while being held.
    //         //     spriteRenderer.sortingLayerName = "Interactive";
    //         //     //player.currentType = PlayerController.PlayerType.nm;

    //         //     transform.SetParent(initialParent);
    //         //     playerTransform = null;

    //         // }
    //     }

    // }



    // protected virtual void ThrowCarryObj2(GameObject parentObject, Vector2 throwDirection, int id)
    // {
    //     //Implement charge throw: https://forum.unity.com/threads/find-how-long-a-button-is-being-held.482430/

    //     if (id == this.GetInstanceID())
    //     {
    //         StartCoroutine(ThrowMotion(transform.position, parentObject.transform.TransformPoint(throwDirection), throwTime));
    //     }

    //     transform.SetParent(initialParent);
    //     playerTransform = null;
    // }

    private IEnumerator ThrowMotion(Vector3 startPos, Vector3 endPos)
    //Coroutine that moves player by one square when called.
    {

        carryState = CarryStates.isThrown;

        float fracComplete = 0f;

        //startPos = transform.position;
        // Vector3 endPos = startPos + directionVec;
        // Debug.Log(directionVec);
        // Debug.Log(startPos);
        // Debug.Log(endPos);

        //while (elapsedTime < throwTime)
        while (fracComplete < 1)
        {
            //Slerp throw implementation.
            Vector3 center = (startPos + endPos) * 0.5f;
            center -= new Vector3(0,1,0);

            Vector3 startRelCenter = startPos - center;
            Vector3 endRelCenter = endPos - center;

            fracComplete = (Time.time - startTime) / throwTime;// * arcSpeed;
            //Debug.Log("start time: " + startTime + " frac complete: " + fracComplete);

            transform.position = Vector3.Slerp(startRelCenter, endRelCenter, fracComplete);
            transform.position += center;
            
            //ThrowUpdateMotion(startPos, endPos);

            //Vector3 currentPos = Vector3.Lerp(startPos, endPos, (elapsedTime / throwTime));
            //currentPos.y += 0.5f * heightCheck * Mathf.Sin(Mathf.Clamp01(elapsedTime / throwTime) * Mathf.PI);
            //currentPos.z -= heightCheck * Mathf.Sin(Mathf.Clamp01(elapsedTime / throwTime) * Mathf.PI);
            //transform.position = Vector3.Lerp(startPos, endPos, (elapsedTime / throwTime));
            //transform.position = currentPos;

            //elapsedTime += Time.deltaTime;


            yield return null;
        }
        transform.position = endPos;            //Clamp to grid

        carryState                      = CarryStates.onGround;
        boxCollider.enabled             = !boxCollider.enabled;                 //Turn off BoxCollider while being held.
        spriteRenderer.sortingLayerName = "Interactive";

        transform.SetParent(initialParent);
        playerTransform                 = null;

    }

    #endregion
}
