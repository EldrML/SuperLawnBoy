using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    # region Variable Definitions
    public PlayerController player;
    public SpriteRenderer sprite;
    public Vector3 carryHeight;

    public enum BoxStates
    {
        onGround, isHeld, isThrown     // With/without lawnmower
    }
    public BoxStates boxState;
    // public bool isHeld;
    // public bool isThrown; 
    public LayerMask ignoreGrass;

    //Throw Variables
    [SerializeField] float launchAngle = 45f;
    [SerializeField] float g = -9.81f;
    [SerializeField] float airtime = 1f;
    [SerializeField] RaycastHit2D throwHit;

    # endregion

    void Start()
    {
        // TODO : Improve the method of assigning the player controller. Needs to scale to SEVERAL objects.
        boxState = BoxStates.onGround;
        player = (PlayerController)FindObjectOfType(typeof(PlayerController));
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //Being carried update.
        if (boxState == BoxStates.isHeld)
        {
            transform.position = player.transform.position + carryHeight;
        }

        //Being thrown update.
        if (boxState == BoxStates.isThrown)
        {
            //transform.position = player.transform.position + carryHeight;
        }

    }

    public void PickUpAndDrop()
    {
        if (player.CanMove())
        {
            if (player.frontData.hit)
            {
                if (player.currentType == PlayerController.PlayerType.nm &&
                    player.frontData.hit.collider.tag == this.tag)
                {
                    boxState = BoxStates.isHeld;
                    sprite.sortingLayerName = "CarryItem";
                    player.currentType = PlayerController.PlayerType.carry;
                }
            }

            if (player.currentType == PlayerController.PlayerType.carry && player.frontData.isEmpty)
            {
                boxState = BoxStates.onGround;
                transform.position = player.transform.TransformPoint(player.lookDirection);
                player.currentType = PlayerController.PlayerType.nm;
                sprite.sortingLayerName = "Interactive";
            }
            else { }
        }
    }

    public void Throw()
    {
        if (player.CanMove())
        {
            //Check 1 block ahead of player.

            throwHit = Physics2D.Raycast(player.transform.position,
                                        player.transform.TransformDirection(player.lookDirection),
                                        2f, ~ignoreGrass); //#TODO This is where you can ignore layers when checking for a throw. Spikes and stuff might matter later.


            //If there is a collision within two tiles.
            if (throwHit)
            {
                if (throwHit.distance < 1) //Check first tile ahead.
                {
                    if (throwHit.collider.tag == "Mower")
                    {
                        Debug.Log("Throw one tile to destroy enemy.");
                    }
                    else
                    {
                        Debug.Log("Can't throw here.");
                    }
                }
                else if (throwHit.distance < 2) //Check second tile ahead.
                {
                    if (throwHit.collider.tag == "Mower")
                    {
                        Debug.Log("Throw two tiles to destroy enemy.");
                    }
                    else
                    {
                        Debug.Log("Throw one tile to avoid collision.");
                    }
                }
            }



            // if (throwHit)
            // {


            //     }
            //     else
            //     {
            //         Debug.Log("Can't throw here.");
            //     }
            // }
            // else if (!throwHit)
            // {
            //     Debug.Log("Checking two tiles ahead.");
            // }

            // else
            // {
            //     Debug.Log("BLEH");        
            // }

            //Check 2 blocks ahead of player.
            // throwHit = Physics2D.Raycast(player.transform.position,
            //                             player.transform.TransformDirection(player.lookDirection),
            //                             2f);

            // sprite.sortingLayerName = "Interactive";
            // isHeld = false;
        }
    }
}
