using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    # region Variable Definitions
    public PlayerController player;
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
        // TODO : Improve the method of assigning the player controller. Needs to scale to SEVERAL objects.
        boxState = BoxStates.onGround;
        player = (PlayerController)FindObjectOfType(typeof(PlayerController));
        sprite = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // void Update()
    // {
    //     //Being carried update.
    //     if (boxState == BoxStates.isHeld)
    //     {
    //         transform.position = player.transform.position + carryHeight;
    //     }

    //     // //Being thrown update.
    //     if (boxState == BoxStates.isThrown)
    //     {
    //         ThrowBox(player.transform.position + carryHeight, throwLocation, boxState == BoxStates.isThrown);
    //     }
    // }

    // public void PickUpAndDrop()
    // {
    //     if (player.CanMove())
    //     {
    //         //Pick up box.
    //         if (player.frontData)
    //         {
    //             if (player.currentType == PlayerController.PlayerType.nm &&
    //                 player.frontData.collider.tag == this.tag)
    //             {
    //                 //Adjust the object's parameters.
    //                 boxState = BoxStates.isHeld;                                //Switch states.
    //                 sprite.sortingLayerName = "CarryItem";
    //                 player.currentType = PlayerController.PlayerType.carry;     //
    //                 player.animator.SetBool("IsCarrying", true);
    //                 boxCollider.enabled = !boxCollider.enabled;                 //Turn off BoxCollider while being held.
    //             }
    //         }

    //         //Set down box.
    //         if (player.currentType == PlayerController.PlayerType.carry && !player.frontData)
    //         {
    //             boxState = BoxStates.onGround;
    //             transform.position = player.transform.TransformPoint(player.lookDirection);
    //             player.currentType = PlayerController.PlayerType.nm;
    //             sprite.sortingLayerName = "Interactive";
    //             boxCollider.enabled = !boxCollider.enabled;                 //Turn on BoxCollider while being released.
    //             player.animator.SetBool("IsCarrying", false);
    //         }
    //         else { }
    //     }
    // }

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
