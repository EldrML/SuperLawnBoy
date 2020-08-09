using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variable Definitions
    public float moveSpeed = 5f;
    public Transform movePoint;
    // public LayerMask stopLayer;
    public Animator animator;
        // public GameObject[] colliderList;
    public GameObject interactiveObject;

    Vector2 input;
    Vector2 lookDirection;
    float playerViewRange = 1f;
    #endregion

    #region Main Logic Loop
    void Start()
    {
        movePoint.parent = null;
        lookDirection = Vector2.down;
    }

    void Update()
    {
        GetMove();

        // if(Input.GetKeyDown(KeyCode.Space))
        // {
        //     CheckInFront();
        // }
    }
    #endregion

    #region Player Movement and Animation Control
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
                lookDirection = input;
                MoveExecute(input);
            }
            else if (Mathf.Abs(input.y) == 1f)      //Vertical Movement
            {
                input.x = 0f;
                lookDirection = input;
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
        else // If the player can't move
        {
            AnimatorControl(input); // Set the animation to walk in that direction, like Pokemon.
        }
    }

    void AnimatorControl(Vector2 input)
    {
        animator.SetFloat("Horizontal", input.x);
        animator.SetFloat("Vertical", input.y);
    }
    #endregion

    bool CanMove() // Checks if there is a collision object in the direction of desired movement.
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(input), playerViewRange);

        //Debug.DrawRay(transform.position, playerViewRange * transform.TransformDirection(input), Color.red, 0.25f);
        if (hit)
        {
            return false;
            /* HERE IS WHERE TO ADD IN EXCEPTIONS TO THE HIT RULE!
            if (hit.collider.tag == "Wall")
            {
                //Debug.Log("Hit Something : " + hit.collider.tag);
                return false;
            }
            */
        }
        return true;
    }

    void CheckInFront()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(lookDirection), playerViewRange);
        Debug.DrawRay(transform.position, playerViewRange * transform.TransformDirection(lookDirection), Color.red, 0.25f);
        if (hit)
        {
            if (hit.collider.tag == "Wall")
            {
                Debug.Log("That's a " + hit.collider.tag + ".");
            }
            if (hit.collider.tag == "Sign")
            {
                //Debug.Log("That's a " + hit.collider.tag + ".");
                interactiveObject = hit.collider.gameObject;
                //interactiveObject.ReadSign();
            }
        }
    }

}