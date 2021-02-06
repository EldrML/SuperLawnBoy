using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass1 : InteractableFloor
{
    [SerializeField] Animator grassAnimator;
    [SerializeField] SpriteRenderer sprite;
    public bool grassHasBeenCut = false;

    // Start is called before the first frame update
    void Start()
    {
        sprite = this.GetComponent<SpriteRenderer>();
        GameEvents.current.onEnterGrassSquare -= CheckForMower;
        GameEvents.current.onEnterGrassSquare += CheckForMower; //Add this to your list of subscribed events.
    }

    void OnEnable()
    //Set up events.
    {
        if (GameEvents.current != null)
        {
            GameEvents.current.onEnterGrassSquare -= CheckForMower;
            GameEvents.current.onEnterGrassSquare += CheckForMower; //Add this to your list of subscribed events.
        }
    }

    private void OnDisable()
    //Remove the listener when the object is disabled.
    {
        GameEvents.current.onEnterGrassSquare -= CheckForMower;
    }

    void CheckForMower(int playerType, int id)
    {
        if (playerType == 1)
        {
            if(id == this.transform.GetInstanceID() && !grassHasBeenCut)
            {
                grassHasBeenCut = true;
                GameEvents.current.GrassIsCut(transform.parent.parent.transform.GetInstanceID()); //Sends the room ID to 
                StartCoroutine(CutGrassCo());
            }
        }
        else
        {
            // Debug.Log("Mower not detected!");
        }
    }

    IEnumerator CutGrassCo()  // Performs an action (CURRENTLY JUST MOWER THUMBS UP)
    {
        sprite.sortingLayerName = "GrassAnimation";
        grassAnimator.SetBool("IsCut", true);

        yield return new WaitForSeconds(0.25f);
        this.gameObject.SetActive(false);
        sprite.sortingLayerName = "Interactive";
    }

}
