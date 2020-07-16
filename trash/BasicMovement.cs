using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BasicMovement : MonoBehaviour
{
    public Animator animator;

    // private Vector3 RemoveDiagonal (Vector3 inputVector)
    // {
    //     float X = inputVector.x;
    //     float Y = inputVector.y;
    //     if (X*X > Y*Y){
    //         return new Vector3 (X,0,0);
    //     } else {
    //         return new Vector3 (0,Y,0);
    //     }
    // }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        animator.SetFloat("Vertical", Input.GetAxis("Vertical"));

        float horizontal = Input.GetAxis ("Horizontal") * Time.deltaTime * 2;
        float vertical = Input.GetAxis ("Vertical") * Time.deltaTime * 2;

        if (Mathf.Abs(horizontal) > 0)
        {
            transform.Translate (horizontal , 0, 0);
        }
        else
        {
            transform.Translate (0 , vertical, 0);
        }
        
        
        // Vector3 horizontal  = new Vector3(Input.GetAxis("Horizontal"), 0.0f, 0.0f);
        // Vector3 vertical    = new Vector3(0.0f, Input.GetAxis("Vertical"), 0.0f);
        // transform.position = transform.position + horizontal * Time.deltaTime + vertical * deltaTime;
    }
}
