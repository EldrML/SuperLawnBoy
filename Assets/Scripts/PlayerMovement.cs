using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    [SerializeField] bool isMoving, pathBlocked;

    public float timeToMove = 0.25f;
    private int playerViewRange = 1;

    [SerializeField] private Vector2 input, lookDirection;

    private Vector3 origPos, targetPos;

    public Animator animator;

    public enum PlayerState
    {
        move, hurt, dash, Action
    }

    public enum PlayerType
    {
        nm, wm, carry     // With/without lawnmower
    }

    public PlayerState state;
    public PlayerType type;

    [SerializeField] InteractableCarry heldObject;

    #endregion

    void Update()
    //Core Logic Loop for player.
    {
        // if (state == PlayerState.move)
        // {
            //Player can't move unless they have hit the grid properly.
            if (isMoving) { return; }

            input = GetMoveInput();     //Get the input from the player.
            GetMoveAndLookDir(input);   //Move the player if possible.


            if (Input.GetButtonDown("Action"))
            //First action button. Reads signs, dashes, throws
            {
                Debug.Log("Using first action button.");
                //Interact(1, state, type);
            }
            else if (Input.GetButtonDown("Action2"))
            //Check in front of player.
            {
                //Debug.Log("Using second action button.");
                //Debug.Log(CheckInFront());
                //Debug.Log(CheckInFront().Item1);
                Interact(2, state, type);
            }

            UpdateAnimations(input);    //Update player animations.
        //}
    }

//---------------------------------------------------------------------------------
    #region Move Logic
    Vector2 GetMoveInput()
    //Simply obtains the 4-Dir move input from the player.
    {
        //Get input of player.
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Mathf.Abs(input.x) == 1f)           //Horizontal Movement
        {
            input.y = 0f;
        }
        else if (Mathf.Abs(input.y) == 1f)      //Vertical Movement
        {
            input.x = 0f;
        }

        return input;
    }



    void GetMoveAndLookDir(Vector2 input)
    //Controls the player's directional movement.
    {
        //If the player has entered an input...
        if (input != Vector2.zero)
        {
            lookDirection = input;                      //Change the Look Direction first.

            pathBlocked = CheckInFront(playerViewRange).Item1;         //Check if there is a collision in the new look direction.

            if (!pathBlocked) //No object in front of player.
            {
                StartCoroutine(MovePlayer(new Vector3(input.x, input.y, 0f)));
            }
        }
    }

    private IEnumerator MovePlayer(Vector3 direction)
    //Coroutine that moves player by one square when called.
    {
        isMoving = true;                    //Disables input while the player is moving.

        float elapsedTime = 0f;

        origPos = transform.position;
        targetPos = origPos + direction;

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(origPos, targetPos, (elapsedTime / timeToMove));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;

        isMoving = false;
    }
    #endregion
//---------------------------------------------------------------------------------

    void UpdateAnimations(Vector2 input)
    //Updates the player's animations based on input, state, and other conditions.
    //Currently only input is implemented.
    {
        if (input != Vector2.zero)
        {
            animator.SetFloat("Horizontal", input.x);
            animator.SetFloat("Vertical", input.y);
            animator.SetBool("Moving", true);
        }
        else
        { animator.SetBool("Moving", false); }
    }

//---------------------------------------------------------------------------------

    public (bool, RaycastHit2D) CheckInFront(int playerViewRange)
    //Checks if there is a collision object in the direction of desired movement.
    //If TRUE: There is an object in front of the player.
    //If FALSE:There is NOT an object in front of the player.
    {
        //Send Raycast in direction that player is facing.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(lookDirection), playerViewRange);
        Debug.DrawRay(transform.position, playerViewRange * transform.TransformDirection(lookDirection), Color.cyan, 0.25f);

        if (hit)
        {
            if (hit.transform.gameObject.layer == 6 ||  //Impassable or
                hit.transform.gameObject.layer == 3)    //Interactable Objects
            {
                //Debug.Log("Good");
                return (true, hit);    //Object in front.
            }
            else                                        //Other layers like grass etc.
            {
                return (false, hit);   //No object in front.
            }
        }
        else
        {
            return (false, new RaycastHit2D());       //No object in front.
        }

    }

    void Interact(int buttonNum, PlayerState state, PlayerType type)
    //Interacts with things in front of the player. (NPCs, mowers, boxes, etc.)
    {
        //Cast a ray one tile in front of the player.
        (bool itemInFront, RaycastHit2D hit) = CheckInFront(playerViewRange); //Physics2D.Raycast(transform.position, transform.TransformDirection(lookDirection), playerViewRange);
        //Debug.DrawRay(transform.position, playerViewRange * transform.TransformDirection(lookDirection), Color.red, 0.25f);

        if (itemInFront)    //Ray hits an object.
        {
            if (hit.transform.gameObject.GetComponent<InteractableCarry>() && type == PlayerType.nm)
            {
                heldObject = hit.transform.gameObject.GetComponent<InteractableCarry>();
                type = PlayerType.carry;
                animator.SetBool("IsCarrying", true);
                SLBEvents.current.PlayerPickUpCarryable(this.gameObject);
            }
            else if(hit.transform.gameObject.GetComponent<InteractableTalk>())
            {
                Debug.Log("Send talk event for " + hit.transform.name);
            }
        }
        else                //Ray does not hit an object.
        {
            if(type == PlayerType.wm)
            {
                Debug.Log("Put down mower in front of player.");
            }
            if(type == PlayerType.carry)
            {
                Debug.Log("Put down carryableObject in front of player.");
            }
        }

    }

}
