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
    public enum PlayerState
    {
        move, hurt, dash, Action,
    }
    public enum PlayerType
    {
        nm, wm     // With/Without lawnmower
    }
    public PlayerState currentState;
    public PlayerType currentType;
    public Mower mower;

    Vector2 input;
    public Vector2 lookDirection;
    float playerViewRange = 1f;
    #endregion

    #region Main Logic Loop
    void Start()
    {
        movePoint.parent = null;
        lookDirection = Vector2.down;
        currentState = PlayerState.move;
        currentType = PlayerType.nm;
    }

    void Update()
    {
        if (Input.GetButtonDown("Action") && currentState != PlayerState.Action && currentType == PlayerType.wm)
        {
            StartCoroutine(ActionCo());
        }

        // if (Input.GetButtonDown("Action2") && currentState == PlayerState.move && currentType == PlayerType.wm)
        // {
        //     Debug.Log("trying boss");
        //     TryToDrop();
        // }

        // if (Input.GetButtonDown("Action") && currentState == PlayerState.move && currentType == PlayerType.nm)
        // {
        //     CheckInFront();
        // }

        if (currentState == PlayerState.move)
        {
            if (Input.GetButtonDown("Action2") && currentType == PlayerType.wm)
            {
                Debug.Log("trying boss");
                TryToDropMower();
            }

            if (Input.GetButtonDown("Action") && currentType == PlayerType.nm)
            {
                CheckInFront();
            }

            GetMove();
        }
    }
    #endregion

    void GetMove()  //Moves the player and sets proper animations
    {
        //Move player
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        //Get movement input
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        //If the distance between the player and the movepoint is less than 0.05 units...
        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            // Limit movement to 4 directions.
            if (Mathf.Abs(input.x) == 1f)           //Horizontal Movement
            {
                input.y = 0f;
            }
            else if (Mathf.Abs(input.y) == 1f)      //Vertical Movement
            {
                input.x = 0f;
            }

            // // Update look direction if movement occurs
            // if(input != Vector2.zero)
            // {
            //     lookDirection = input;                  //Direction of RayCast for collision checking
            // }

            // Move if allowed
            if (CanMove())                          //If no collision, move the player.
            {
                Vector3 moveVector = new Vector3(input.x, input.y, 0f);
                movePoint.position += moveVector;
            }

            // -- Animation/Look Direction Control -- //
            if (input != Vector2.zero)
            {
                lookDirection = input;                  //Direction of RayCast for collision checking
                animator.SetFloat("Horizontal", input.x);
                animator.SetFloat("Vertical", input.y);
                animator.SetBool("Moving", true);
            }
            else
            {
                animator.SetBool("Moving", false);
            }
        }
    }

    IEnumerator ActionCo()  // Performs an action (CURRENTLY JUST MOWER THUMBS UP)
    {
        animator.SetBool("Action", true);
        currentState = PlayerState.Action;
        yield return null;
        animator.SetBool("Action", false);
        yield return new WaitForSeconds(1f);
        currentState = PlayerState.move;
    }

    bool CanMove() // Checks if there is a collision object in the direction of desired movement.
    {
        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(input), playerViewRange);
            //Debug.DrawRay(transform.position, playerViewRange * transform.TransformDirection(input), Color.red, 0.25f);

            if (hit)
            {
                // Exceptions to the hit raycast.
                if (hit.collider.tag == "Grass")
                {
                    //Debug.Log("Hit Something : " + hit.collider.tag);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        return false;

    }

    void CheckInFront() //Similar to CanMove function, but hopefully can be expanded if needed.
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
            if (hit.collider.tag == "Mower")
            {
                mower.gameObject.SetActive(false);
                currentType = PlayerType.wm;
                animator.SetBool("HasMower", true);
            }
        }
        else
        {

        }
    }

    void TryToDropMower() //Similar to CanMove function, but hopefully can be expanded if needed.
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(lookDirection), playerViewRange);
        Debug.DrawRay(transform.position, playerViewRange * transform.TransformDirection(lookDirection), Color.red, 0.25f);
        if (!hit && CanMove())
        {
            
            mower.transform.position = this.transform.TransformPoint(lookDirection);
            mower.mowerDirection = lookDirection;
            mower.gameObject.SetActive(true);
            //Debug.Log(interactiveObject.transform.TransformDirection);
            //Instantiate(mower, lookDirection, Quaternion.identity);
            currentType = PlayerType.nm;
            animator.SetBool("HasMower", false);
        }
        else {}
    }

    #region Extra Functions
    void AnimatorControl(Vector2 input) //May be useful later for more animation controlling...
    {
        if (input != Vector2.zero)
        {
            animator.SetFloat("Horizontal", input.x);
            animator.SetFloat("Vertical", input.y);
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
        }

    }
    #endregion
}