﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass1 : MonoBehaviour
{
    public PlayerController player;
    public Animator grassAnimator;
    // Start is called before the first frame update
    void Start()
    {
        
        // TODO : Improve the method of assigning the player controller. Needs to scale to MANY grass objects.
        player = (PlayerController)FindObjectOfType(typeof(PlayerController));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D otherObj)
    {
        if(otherObj.CompareTag("Player") && player.currentType == PlayerController.PlayerType.wm)
        {
            //this.GetComponent<SpriteRenderer>().sortingLayerID = 6;
            StartCoroutine(CutGrassCo());
        }
    }

    void OnTriggerStay2D(Collider2D otherObj)
    {
        if(otherObj.CompareTag("Player") && player.currentType == PlayerController.PlayerType.wm && this.gameObject.activeInHierarchy == true)
        {
            //this.GetComponent<SpriteRenderer>().sortingLayerID = 6;
            StartCoroutine(CutGrassCo()); 
        }
    }

    IEnumerator CutGrassCo()  // Performs an action (CURRENTLY JUST MOWER THUMBS UP)
    {
        this.GetComponent<SpriteRenderer>().sortingLayerName = "Animation";
        grassAnimator.SetBool("IsCut", true);
        Debug.Log("Grass Destroyed");
        yield return new WaitForSeconds(0.25f);
        this.gameObject.SetActive(false);
        this.GetComponent<SpriteRenderer>().sortingLayerName = "Interactive";
    }
}
