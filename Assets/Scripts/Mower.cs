using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mower : MonoBehaviour
{
    public PlayerController player;
    public Vector2 mowerDirection;
    public Animator mowerAnimator;
    // Start is called before the first frame update

    void Start()
    {
        mowerDirection = Vector2.left;
        //mowerPosition = this.transform.position;
        player = (PlayerController)FindObjectOfType(typeof(PlayerController));
    }

    // Update is called once per frame
    // void Update()
    // {
    //     if (this.isActiveAndEnabled)
    //     {
    //         mowerAnimator.SetFloat("MowerHorDir", mowerDirection.x);
    //         mowerAnimator.SetFloat("MowerVerDir", mowerDirection.y);
    //     }
    // }

    // public void PickUpAndDropMower()
    // {
    //     if (player.CanMove())
    //     {
    //         //Pick Up Mower logic.
    //         if (player.frontData)
    //         {
    //             if (player.currentType == PlayerController.PlayerType.nm &&
    //                 player.frontData.collider.tag == "Mower")
    //             {
    //                 this.gameObject.SetActive(false);
    //                 player.currentType = PlayerController.PlayerType.wm;
    //                 player.animator.SetBool("HasMower", true);
    //             }
    //             else { }
    //         }

    //         //Drop Mower logic.
    //         if (player.currentType == PlayerController.PlayerType.wm && 
    //             !player.frontData)
    //         {
    //             this.transform.position = player.transform.TransformPoint(player.lookDirection);
    //             this.mowerDirection = player.lookDirection;
    //             this.gameObject.SetActive(true);
    //             player.currentType = PlayerController.PlayerType.nm;
    //             player.animator.SetBool("HasMower", false);
    //         }
    //         else { }

    //     }
    // }
}
