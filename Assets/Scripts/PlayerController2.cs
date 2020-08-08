using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    //These are private variables for now.
    Vector3 up = Vector3.zero,
    right = new Vector3(0,90,0),
    down = new Vector3(0,180,0),
    left = new Vector3(0,270,0),
    currentDirection = Vector3.zero;

    Vector3 nextPosition, destination, direction;

    float speed = 5f;
    float rayLength = 1f;

    bool canMove;

    void Start()
    {
        currentDirection = up;              //Initial direction is up. We might want it to be down.
        nextPosition = Vector3.forward;     //
        destination = transform.position;   //
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, speed*Time.deltaTime);

        //We might be able to change this later to be the input direction
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            nextPosition = Vector3.forward;
            // currentDirection = up;
            canMove = true;
        }
        if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            nextPosition = Vector3.back;
            // currentDirection = down;
            canMove = true;
        }
        //if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            nextPosition = Vector3.left;
            // currentDirection = right;
            canMove = true;
        }
        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            nextPosition = Vector3.right;
            // currentDirection = left;
            canMove = true;
        }

        if(Vector3.Distance(destination, transform.position) <= 0.00001f)
        {
            transform.localEulerAngles = currentDirection;
            
            if(canMove)
            {
                if(Valid())
                {
                    destination = transform.position + nextPosition;
                    direction = nextPosition;
                    canMove = false;
                }
            }
            
        }
    }

    bool Valid()
    {
        Ray myRay = new Ray(transform.position + new Vector3(0,0.25f,0), transform.forward);
        RaycastHit hit;
        Debug.Log("faLSE");
        Debug.DrawRay(myRay.origin, myRay.direction, Color.red);

        if(Physics.Raycast(myRay, out hit, rayLength))
        {
            if(hit.collider.tag == "Wall")
            {
                return false;
            }
        }
        return true;
    }
}
