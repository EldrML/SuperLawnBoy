using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variable Definitions
    public Transform movePoint;
    public Animator animator;

    public Interactable interactiveObj;

    public Vector2 lookDirection;

    public float moveSpeed;

    public enum PlayerState
    {
        move, hurt, dash, Action
    }
    public enum PlayerType
    {
        nm, wm, carry     // With/without lawnmower
    }
    public PlayerState currentState;
    public PlayerType currentType;

    Vector2 input;
    float playerViewRange = 1f;
    public RaycastHit2D frontData;

    #endregion

    #region Main Logic Loop
    void Start()
    {
        movePoint.parent = null;            //Initializes the player's move-point,
        lookDirection = Vector2.down;       //Raycast direction,
        currentState = PlayerState.move;    //PlayerState (move, carry, etc),
        currentType = PlayerType.nm;        //PlayerType (mower, no mower)
    }

    void Update()
    {
        if (currentState == PlayerState.move)
        {
            if (Input.GetButtonDown("Action"))
            {
                Interact(1, currentState, currentType);
            }

            else if (Input.GetButtonDown("Action2"))
            {
                Debug.Log("Using second action button.");
                //Interact(2, currentState, currentType);
            }

            Debug.Log(frontData.collider.gameObject);

            //This logic deselects the interactive object if the player is no longer facing it.
            if (interactiveObj != null)
            {
                //Debug.Log(frontData.collider.gameObject);
                // if (frontData.collider.gameObject == interactiveObj)
                // {
                //     interactiveObj.isFocus = false;
                //     interactiveObj = null;
                // }
                // else{}
            }
            else{}              //Do nothing


            GetMove();
        }
    }
    #endregion

    void GetMove()
    //Moves the player and sets proper animations
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

            //If no collision, move the player.
            if (CanMove().Item1)
            {
                Vector3 moveVector = new Vector3(input.x, input.y, 0f);
                movePoint.position += moveVector;
            }

            // -- Look Direction Control -- //
            if (input != Vector2.zero)
            {
                lookDirection = input;                      //Direction of RayCast for collision checking
            }

            //Animation Control
            if (input != Vector2.zero)
            {
                animator.SetFloat("Horizontal", input.x);
                animator.SetFloat("Vertical", input.y);
                animator.SetBool("Moving", true);
            }
            else
            { animator.SetBool("Moving", false); }
        }
    }

    public (bool, RaycastHit2D) CanMove()
    // Checks if there is a collision object in the direction of desired movement.
    {
        if (Vector3.Distance(transform.position, movePoint.position) <= 0.0001f)
        {
            RaycastHit2D frontData = Physics2D.Raycast(transform.position, transform.TransformDirection(input), playerViewRange);
            Debug.DrawRay(transform.position, playerViewRange * transform.TransformDirection(lookDirection), Color.cyan, 0.25f);

            if (frontData)
            {
                if (frontData.transform.gameObject.layer == 6 || frontData.transform.gameObject.layer == 3) //Impassable or Interactable Objects
                { return (false, frontData); }           //Can't move.
                else
                { return (true, frontData); }            //Can move.
            }

            return (true, frontData);                    //No hit, can move.
        }

        return (false, frontData);                       //Too early to move.

    }

    void Interact(int buttonNum, PlayerState state, PlayerType type)
    //Interacts with things in front of the player. (NPCs, mowers, boxes, etc.)
    {
        //Cast a ray one tile in front of the player.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(lookDirection), playerViewRange);
        Debug.DrawRay(transform.position, playerViewRange * transform.TransformDirection(lookDirection), Color.red, 0.25f);
        Debug.Log(hit.transform.name);
        if (hit) //If the ray hits an object
        {
            interactiveObj = hit.collider.GetComponent<Interactable>();     //Make this the interactive object.
            interactiveObj.player = transform;                              //
            interactiveObj.isFocus = true;


            // if (interactiveObj != null) //Interactable has been set at least once.
            // {

            //     if (hit.collider.transform.name != interactiveObj.transform.name)   //Different Interactable.
            //     {
            //         interactiveObj.isFocus = false;
            //         interactiveObj.player = null;
            //         interactiveObj = hit.collider.GetComponent<Interactable>();
            //         interactiveObj.isFocus = true;
            //         interactiveObj.player = transform;
            //         //Debug.Log("SAME OBJECT");
            //     }
            //     else                                                                //Same Interactable
            //     {
            //         //interactiveObj = hit.collider.GetComponent<Interactable>();
            //     }
            // }
            // else //Set interactable for the first, if possible.
            // {
            //     interactiveObj = hit.collider.GetComponent<Interactable>();     //Make this the interactive object.
            //     interactiveObj.player = transform;                              //
            //     interactiveObj.isFocus = true;
            // }

            //Make that object the focused interactive.




            if (currentType == PlayerType.nm)   //Player does not have mower.
            {
                interactiveObj.Interact(buttonNum, 1);
            }
            else                                //Player has mower.
            {
                interactiveObj.Interact(buttonNum, 2);
            }
        }
    }

    // IEnumerator ActionCo()  // Performs an action (CURRENTLY JUST MOWER THUMBS UP)
    // {
    //     yield return new WaitUntil(() => CanMove());
    //     animator.SetBool("Action", true);
    //     currentState = PlayerState.Action;
    //     yield return null;
    //     animator.SetBool("Action", false);
    //     yield return new WaitForSeconds(1f);
    //     currentState = PlayerState.move;
    // }

    // #region Extra Functions
    // void AnimatorControl(Vector2 input) //May be useful later for more animation controlling...
    // {
    //     if (input != Vector2.zero)
    //     {
    //         animator.SetFloat("Horizontal", input.x);
    //         animator.SetFloat("Vertical", input.y);
    //         animator.SetBool("Moving", true);
    //     }
    //     else
    //     {
    //         animator.SetBool("Moving", false);
    //     }

    // }
    // #endregion
}