using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : InteractableCarry
{
    # region Variable Definitions
    //public PlayerController player;
    public SpriteRenderer sprite;
    public Vector3 carryHeight;
    public BoxCollider2D boxCollider;
    public Vector3 throwLocation;

    public enum BoxStates
    {
        onGround, isHeld, isThrown     // With/without lawnmower
    }
    public BoxStates boxState;
    public LayerMask ignoreGrass;
    [SerializeField] RaycastHit2D throwHit;

    float height = 1f;
    // bool startThrow = false;
    float incrementor = 0;

    # endregion

    void Start()
    {
        boxState = BoxStates.onGround;
        sprite = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        //player = (PlayerController)FindObjectOfType(typeof(PlayerController));
        
    }

    void Update()
    {
        //Box is being carried above player.
        if (boxState == BoxStates.isHeld)
        {
            transform.position =  player.position + carryHeight;
        }
        //Debug.Log("Just make the box position a parent instead of constantly updating it.");

        // //Being thrown update.
        // if (boxState == BoxStates.isThrown)
        // {
        //     ThrowBox(player.transform.position + carryHeight, throwLocation, boxState == BoxStates.isThrown);
        // }

        // if (boxState == BoxStates.onGround)
        // {
        //     if (player != null)
        //     {
        //         if (this.transform.position - this.player.transform.position != new Vector3(lookDir.x, lookDir.y, 0f))
        //         {
        //             Debug.Log("No Longer In Range");
        //             player = null;
        //         }
        //     }
        // }
    }

    public override void InteractEmpty(bool frontHasObject)
    {
        //Debug.Log(frontHasObject);

        PickUpBox();
        
        // if(buttonNum == 1 && state == 0)
        // //Button 1, state: move
        // {
        //     if(type == 0)
        //     {
        //         PickUpBox();
        //     }
        //     else if(type == 2 && frontHasObject)
        //     {
        //         Debug.Log("tic");
        //         DropBox();
        //     }
                
        // }
        
    }

    public override void InteractCarry(bool frontHasObject)
    {
        Debug.Log(frontHasObject);
        if(!frontHasObject)
        {
            DropBox();
        }
        
        // if(buttonNum == 1 && state == 0)
        // //Button 1, state: move
        // {
        //     if(type == 0)
        //     {
        //         PickUpBox();
        //     }
        //     else if(type == 2 && frontHasObject)
        //     {
        //         Debug.Log("tic");
        //         DropBox();
        //     }
                
        // }
        
    }

    public void PickUpBox()
    {
        if (boxState == BoxStates.onGround)
        {
            boxState = BoxStates.isHeld;                            
            boxCollider.enabled = !boxCollider.enabled;             //Turn off BoxCollider while being held.
            sprite.sortingLayerName = "CarryItem";                  //Make the box visible above other objects.

            playerScript.type = PlayerMovement.PlayerType.carry;
            playerScript.animator.SetBool("IsCarrying", true);
        }

        //Set down box.
        // if (boxState == BoxStates.isHeld && playerScript.CheckInFront)
        // {
        //     boxState = BoxStates.onGround;
        //     transform.position = player.transform.TransformPoint(player.lookDirection);
        //     player.currentType = PlayerController.PlayerType.nm;
        //     sprite.sortingLayerName = "Interactive";
        //     boxCollider.enabled = !boxCollider.enabled;                 //Turn on BoxCollider while being released.
        //     player.animator.SetBool("IsCarrying", false);
        // }
        // else { }
        // }
    }

    void DropBox()
    {
        if (boxState == BoxStates.isHeld)
        {   
            // RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(lookDir), 1f);
            // Debug.DrawRay(transform.position, 1f * transform.TransformDirection(lookDir), Color.red, 0.25f);
            // Debug.Log("DONT FORGET TO UNHARDCODE THIS");

            // if(hit)
            // {
            //     //Collision here, do not include
            // }
            // else
            // {

            // }

            boxState = BoxStates.onGround;
            transform.position = player.TransformPoint(lookDir);
            playerScript.type = PlayerMovement.PlayerType.nm;
            sprite.sortingLayerName = "Interactive";
            boxCollider.enabled = !boxCollider.enabled;                 //Turn on BoxCollider while being released.
            playerScript.animator.SetBool("IsCarrying", false);

            // boxState = BoxStates.onGround;
            // sprite.sortingLayerName = "CarryItem";
            // //boxState = BoxStates.isHeld;                                //Switch states.
            // //sprite.sortingLayerName = "CarryItem";
            // playerScript.type = PlayerMovement.PlayerType.carry;     //
            // playerScript.animator.SetBool("IsCarrying", true);
            // boxCollider.enabled = !boxCollider.enabled;                 //Turn off BoxCollider while being held.
        }
    }

    // public void CheckForThrow()
    // {
    //     if (player.CanMove())
    //     {
    //         throwHit = Physics2D.Raycast(player.transform.position,
    //                                     player.transform.TransformDirection(player.lookDirection),
    //                                     2f, ~ignoreGrass); //#TODO This is where you can ignore layers when checking for a throw. Spikes and stuff might matter later.


    //         //Check if there is a collision within two tiles.
    //         if (throwHit)
    //         {
    //             if (throwHit.distance < 1) //Check first tile ahead.
    //             {
    //                 if (throwHit.collider.tag == "Mower")
    //                 {
    //                     Debug.Log("Throw one tile to destroy enemy.");
    //                     throwLocation = player.transform.position + new Vector3(player.lookDirection.x, player.lookDirection.y, 0);
    //                     boxState = BoxStates.isThrown;
    //                 }
    //                 else
    //                 {
    //                     Debug.Log("Can't throw here.");
    //                 }
    //             }
    //             else if (throwHit.distance < 2) //Check second tile ahead.
    //             {
    //                 if (throwHit.collider.tag == "Mower")
    //                 {
    //                     Debug.Log("Throw two tiles to destroy enemy.");
    //                     throwLocation = player.transform.position + new Vector3(2 * player.lookDirection.x, 2 * player.lookDirection.y, 0);
    //                     boxState = BoxStates.isThrown;
    //                 }
    //                 else
    //                 {
    //                     Debug.Log("Throw one tile to avoid collision.");
    //                     throwLocation = player.transform.position + new Vector3(player.lookDirection.x, player.lookDirection.y, 0);
    //                     boxState = BoxStates.isThrown;
    //                 }
    //             }
    //         }
    //         else
    //         {
    //             Debug.Log("Throw two tiles.");
    //             throwLocation = player.transform.position + new Vector3(2 * player.lookDirection.x, 2 * player.lookDirection.y, 0);
    //             boxState = BoxStates.isThrown;
    //         }

    //     }
    // }

    // void ThrowBox(Vector3 startPos, Vector3 endPos, bool startThrow)
    // {
    //     player.animator.SetBool("IsCarrying", false);

    //     if (startThrow)
    //     {
    //         incrementor += 0.04f;
    //         Vector3 currentPos = Vector3.Lerp(startPos, endPos, incrementor);
    //         currentPos.y += 0.5f * height * Mathf.Sin(Mathf.Clamp01(incrementor) * Mathf.PI);
    //         currentPos.z -= height * Mathf.Sin(Mathf.Clamp01(incrementor) * Mathf.PI);
    //         Debug.Log(currentPos.z);
    //         transform.position = currentPos;
    //     }
    //     if (transform.position == endPos)
    //     {
    //         startThrow = false;
    //         incrementor = 0;
    //         Vector3 tempPos = startPos;
    //         startPos = transform.position;
    //         endPos = tempPos;

    //         boxState = BoxStates.onGround;
    //         boxCollider.enabled = !boxCollider.enabled;                 //Turn off BoxCollider while being held.
    //         player.currentType = PlayerController.PlayerType.nm;
    //         sprite.sortingLayerName = "Interactive";
    //     }
    // }

}
