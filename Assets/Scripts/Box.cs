using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    // Variables
    public PlayerController player;
    public Transform destination;
    public bool isHeld;

    void Start()
    {
        // TODO : Improve the method of assigning the player controller. Needs to scale to SEVERAL objects.
        //player = (PlayerController)FindObjectOfType(typeof(PlayerController));
    }

    void Update()
    {
        if (Input.GetButtonDown("Action2"))
        {
            if (player.currentType == PlayerController.PlayerType.nm &&
                        player.frontData.hit)
            {
                if (player.frontData.hit.collider.tag == "Box")
                {
                    isHeld = true;
                    player.currentType = PlayerController.PlayerType.carry;
                }
            }

            if (player.currentType == PlayerController.PlayerType.carry && player.frontData.isEmpty)
            {
                isHeld = false;
                this.transform.position = player.transform.TransformPoint(player.lookDirection);
                //this.mowerDirection = player.lookDirection;
                //mower.gameObject.SetActive(true);
                player.currentType = PlayerController.PlayerType.nm;
                //animator.SetBool("HasMower", false);
            }
            else { }
        }

        if (isHeld)
        {
            this.transform.position = destination.transform.position;
        }

    }
}
