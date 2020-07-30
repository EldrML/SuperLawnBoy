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



    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    
        //If the distance between the player and the movepoint is less than 0.05 units...
        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            if (Mathf.Abs(input.x) == 1f)
            {
                MoveHorizontal(input.x);
            }
            else if (Mathf.Abs(input.y) == 1f)
            {
                MoveVertical(input.y);
            }
            else
            {
                animator.SetFloat("Vertical", 0f);
                animator.SetFloat("Horizontal", 0f);
            }


        }
    }

    void MoveHorizontal(float x_val)
    {
        Vector3 horizontal = new Vector3(x_val, 0f, 0f);

        if (!Physics2D.OverlapCircle(movePoint.position + horizontal, .2f, stopLayer)) //If no collision, move the player.
        {
            animator.SetFloat("Horizontal", x_val);
            animator.SetFloat("Vertical", 0f);
            movePoint.position += horizontal;
        }
        else //If the player is colliding with a wall, just set the animation to walk, don't actually move.
        {
            animator.SetFloat("Horizontal", x_val);
            animator.SetFloat("Vertical", 0f);
        }
    }

    void MoveVertical(float y_val)
    {
        Vector3 vertical = new Vector3(0f, y_val, 0f);

        if (!Physics2D.OverlapCircle(movePoint.position + vertical, .2f, stopLayer))
        {
            animator.SetFloat("Vertical", y_val);
            animator.SetFloat("Horizontal", 0f);
            movePoint.position += vertical;
        }
        else
        {
            animator.SetFloat("Vertical", y_val);
            animator.SetFloat("Horizontal", 0f);
        }
    }

}