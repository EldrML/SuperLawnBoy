// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class Sign : InteractableTalk
// {
//     public string dialog;
//     private DialogueManager dMan;

//     void Start()
//     {
//         //SuperLawnBoyEvents.current.onPlayerReadSign += OnReadSign; //Add this to your list of subscribed events.
//         //SuperLawnBoyEvents.current.onPlayerReadSign += OnReadSign; //Add this to your list of subscribed events.
//         dMan = FindObjectOfType<DialogueManager>();
//     }
//     public override void InteractEmpty(bool frontHasObject)
//     {
//         ReadSign();
//     }

//     public override void InteractCarry(bool frontHasObject)
//     {
//         ReadSign();
//     }

//     private void OnReadSign()
//     {
//         ReadSign();
//     }

//     void ReadSign()
//     {
//         dMan.ShowBox(dialog);
//     }
//     void Update()
//     {
//         //base.Update();
//         if (player != null)
//         {
//             if (this.transform.position - this.player.transform.position != new Vector3(lookDir.x, lookDir.y, 0f))
//             {
//                 dMan.dBox.SetActive(false);
//                 player = null;

//             }
//         }
//     }

// }