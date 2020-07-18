using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform movePoint;
    public LayerMask stopLayer;
    public Animator animator;
    public string lastDirection;
    public string currentDirection;

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        float x_val;
        float y_val;
        // transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime); // Perform the movement.
        // If the player has finished moving one grid tile...
        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            //Get the current direction. ----------------------------
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                // float y_val = Input.GetAxisRaw("Vertical");
                // MoveVertical(y_val);
                currentDirection = "Up";
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                // float y_val = Input.GetAxisRaw("Vertical");
                // MoveVertical(y_val);
                currentDirection = "Down";
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                // float x_val = Input.GetAxisRaw("Horizontal");
                // MoveHorizontal(x_val);
                currentDirection = "Right";
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                // float x_val = Input.GetAxisRaw("Horizontal");
                // MoveHorizontal(x_val);
                currentDirection = "Left";
            }
            else
            {
                currentDirection = null;
            }
            // -----------------------------------------------------

            if (currentDirection == null) // If there is no new direction, but no direction keys have been released...
            {
                // Move in the previous direction. -----
                if (lastDirection == "Up")
                {
                    y_val = 1f;
                    MoveVertical(y_val);
                    return;
                }
                else if (lastDirection == "Down")
                {
                    y_val = -1f;
                    MoveVertical(y_val);
                    return;
                }
                else if (lastDirection == "Right")
                {
                    x_val = 1f;
                    MoveHorizontal(x_val);
                    return;
                }
                else if (lastDirection == "Left")
                {
                    x_val = -1f;
                    MoveHorizontal(x_val);
                    return;
                }
                else
                {
                    animator.SetFloat("Vertical", 0f);
                    animator.SetFloat("Horizontal", 0f);
                }
                //-------------------------------------
            }
            else //If there IS a new direction...
            {
                // Move in the new direction. -----
                if (currentDirection == "Up")
                {
                    y_val = 1f;
                    MoveVertical(y_val);
                    return;
                }
                else if (currentDirection == "Down")
                {
                    y_val = -1f;
                    MoveVertical(y_val);
                    return;
                }
                else if (currentDirection == "Right")
                {
                    x_val = 1f;
                    MoveHorizontal(x_val);
                    return;
                }
                else if (currentDirection == "Left")
                {
                    x_val = -1f;
                    MoveHorizontal(x_val);
                    return;
                }
                else
                {
                    animator.SetFloat("Vertical", 0f);
                    animator.SetFloat("Horizontal", 0f);
                }
                //-------------------------------------
            }

            // Check if a key is released...
            if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
            {
                animator.SetFloat("Vertical", 0f);
                animator.SetFloat("Horizontal", 0f);
                lastDirection = null;
                return;
            }
            else
            {
                lastDirection = currentDirection; //Set the lastDirection for the next iteration to be the current direction.
            }
        }




            //     else
            //     {
            //         float x_val = Input.GetAxisRaw("Horizontal");
            //         float y_val = Input.GetAxisRaw("Vertical");

            //         if (Mathf.Abs(x_val) == 1f)
            //         {
            //             if (Mathf.Abs(y_val) == 1f)
            //             {
            //                 MoveVertical(y_val);
            //                 return;
            //             }
            //             MoveHorizontal(x_val);
            //             return;
            //         }
            //         else if (Mathf.Abs(y_val) == 1f)
            //         {
            //             if (Mathf.Abs(x_val) == 1f)
            //             {
            //                 MoveHorizontal(x_val);
            //                 return;
            //             }
            //             MoveVertical(y_val);
            //             return;
            //         }
            //         else //if (Mathf.Abs(y_val) == 0f && Mathf.Abs(x_val) == 0f)
            //         {
            //             animator.SetFloat("Vertical", 0f);
            //             animator.SetFloat("Horizontal", 0f);
            //             return;
            //         }
            //     }
    }


        void HandleMovement()
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

            float x_val = Input.GetAxisRaw("Horizontal");
            float y_val = Input.GetAxisRaw("Vertical");

            // If the distance between the player and the movepoint is less than 0.05 units...
            if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
            {
                if (Mathf.Abs(x_val) == 1f)
                {
                    MoveHorizontal(x_val);
                }
                else if (Mathf.Abs(y_val) == 1f)
                {
                    MoveVertical(y_val);
                }
                else
                {
                    animator.SetFloat("Vertical", 0f);
                    animator.SetFloat("Horizontal", 0f);
                    return;
                }
            }
        }

        void MoveHorizontal(float x_val)
        {
            Vector3 horizontal = new Vector3(x_val, 0f, 0f);

            if (!Physics2D.OverlapCircle(movePoint.position + horizontal, .2f, stopLayer))
            {
                animator.SetFloat("Horizontal", x_val);
                animator.SetFloat("Vertical", 0f);
                movePoint.position += horizontal;
                return;
            }
            else
            {
                animator.SetFloat("Horizontal", x_val);
                animator.SetFloat("Vertical", 0f);
                return;
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
                return;
            }
            else
            {
                animator.SetFloat("Vertical", y_val);
                animator.SetFloat("Horizontal", 0f);
                return;
            }
        }

    }