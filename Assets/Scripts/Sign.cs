using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sign : Interactable
{
    //public PlayerController player;
    public GameObject dialogBox;
    public Text dialogText;
    public string dialog;
    //public bool playerInRange;

    void Start()
    {
        // TODO : Improve the method of assigning the player controller. Needs to scale to SEVERAL objects.
        //player = (PlayerController)FindObjectOfType(typeof(PlayerController));
    }

    public override void Interact(int buttonNum, int state)
    {
        base.Interact(buttonNum, state);
        
        //if (inRange && isFocus)
        if (isFocus)
        {
            if (buttonNum == 1 && state == 1)
            {
                ReadSign();
            }
        }
    }

    void ReadSign()
    {
        if (dialogBox.activeInHierarchy)
        {
            dialogBox.SetActive(false);
        }
        else
        {
            dialogBox.SetActive(true);
            dialogText.text = dialog;
        }
    }

    public override void Update()
    {
        base.Update();
        if (!isFocus)
        {
            dialogBox.SetActive(false);
            player = null;
        }
    }
}