using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform movePoint;
    public LayerMask stopLayer;
    public Animator animator;
    //public bool isMoving;
    public Vector2 input;
    public Vector2 direction;
    public GameObject[] colliderList;

    bool canMove;


    void Start()
    {
        direction = Vector2.zero;     //Initialize with no direction.
        movePoint.parent = null;
    }

    void Update()
    {
        Move();
    }

    void Move()
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
                direction = new Vector2(input.x, 0);
                MoveHorizontal(input.x, CanMove());
            }
            else if (Mathf.Abs(input.y) == 1f)      //Vertical Movement
            {
                direction = new Vector2(0, input.y);
                MoveVertical(input.y, CanMove());
            }
            else
            {
                animator.SetFloat("Vertical", 0f);
                animator.SetFloat("Horizontal", 0f);
            }
        }
    }
    
    void MoveHorizontal(float xVal, bool canMove) // Horizontal movement manager.
    {
        Vector3 horizontal = new Vector3(xVal, 0f, 0f);

        //If no collision, move the player.
        if (canMove)
        {
            animator.SetFloat("Horizontal", xVal);
            animator.SetFloat("Vertical", 0f);
            movePoint.position += horizontal;
        }
        else //If the player can't move, just set the animation to walk in that direction, like Pokemon.
        {
            animator.SetFloat("Horizontal", xVal);
            animator.SetFloat("Vertical", 0f);
        }
    }

    void MoveVertical(float yVal, bool canMove) // Vertical movement manager.
    {
        Vector3 vertical = new Vector3(0f, yVal, 0f);

        //If no collision, move the player.
        if (canMove)
        {
            animator.SetFloat("Vertical", yVal);
            animator.SetFloat("Horizontal", 0f);
            movePoint.position += vertical;
        }
        else //If the player can't move, just set the animation to walk in that direction, like Pokemon.
        {
            animator.SetFloat("Vertical", yVal);
            animator.SetFloat("Horizontal", 0f);
        }
    }

    bool CanMove() // Checks if there is a collision object in the direction of desired movement.
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(direction), 1f);

        Debug.DrawRay(transform.position, 1f * transform.TransformDirection(direction), Color.red, 0.25f);
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