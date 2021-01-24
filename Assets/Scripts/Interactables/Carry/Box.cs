using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : InteractableCarry
{
    # region Variables

    //public Vector3 throwLocation;

    //public LayerMask ignoreGrass;

    //[SerializeField] RaycastHit2D throwHit;

    //float height = 1f;
    //float incrementor = 0;

    # endregion


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
    //     //player.animator.SetBool("IsCarrying", false);

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

    //         carryState = CarryStates.onGround;
    //         boxCollider.enabled = !boxCollider.enabled;                 //Turn off BoxCollider while being held.
    //         spriteRenderer.sortingLayerName = "Interactive";
    //         //player.currentType = PlayerController.PlayerType.nm;
            
    //     }
    // // }

}
