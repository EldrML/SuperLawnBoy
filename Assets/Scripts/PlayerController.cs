using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform movePoint;
    // public LayerMask stopLayer;
    public Animator animator;
    public Vector2 input;
    // public GameObject[] colliderList;

    bool canMove;

    void Start()
    {
        movePoint.parent = null;
    }

    void Update()
    {
        GetMove();
    }

    void GetMove()  //Get's the movement input from the player.
    {
        //Move the player.
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        //Get a new input for the next movement.
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //If the distance between the player and the movepoint is less than 0.05 units...
        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            if (Mathf.Abs(input.x) == 1f)           //Horizontal Movement
            {
                input.y = 0f;
                MoveExecute(input);
            }
            else if (Mathf.Abs(input.y) == 1f)      //Vertical Movement
            {
                input.x = 0f;
                MoveExecute(input);
            }
            else                                    //No Movement
            {
                AnimatorControl(input);
            }
        }
    }

    void MoveExecute(Vector2 input) //Executes the Movement input.
    {
        Vector3 moveVector = new Vector3(input.x, input.y, 0f);

        if (CanMove())  //If no collision, move the player.
        {
            AnimatorControl(input);
            movePoint.position += moveVector;
        }
        else //If the player can't move, just set the animation to walk in that direction, like Pokemon.
        {
            AnimatorControl(input);
        }
    }

    void AnimatorControl(Vector2 input)
    {
        animator.SetFloat("Horizontal", input.x);
        animator.SetFloat("Vertical", input.y);
    }

    bool CanMove() // Checks if there is a collision object in the direction of desired movement.
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(input), 1f);

        //Debug.DrawRay(transform.position, 1f * transform.TransformDirection(input), Color.red, 0.25f);
        if (hit)
        {
            if (hit.collider.tag == "Wall")
            {
                Debug.Log("Hit Something : " + hit.collider.tag);
                return false;
            }
            if (hit.collider.tag == "Sign")
            {
                Debug.Log("Hit Something : " + hit.collider.tag);
                return false;
            }
        }
        return true;
    }

}