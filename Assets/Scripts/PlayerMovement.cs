using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    #region Variables

    [SerializeField] bool isMoving, pathBlocked, isTalking = false, waitForInteract = false;

    public float timeToMove = 0.25f;
    [SerializeField] private int playerViewRange = 1, buttonNum = 0;

    [SerializeField] private Vector2 input, lookDirection;

    private Vector3 origPos, targetPos;

    public Animator animator;

    enum PlayerState
    {
        move, hurt, dash, action,
    }

    enum PlayerType
    {
        nm, wm, carry     // With/without lawnmower
    }

    [SerializeField] PlayerState state;
    [SerializeField] PlayerType type;

    [SerializeField] InteractableCarry heldObject;

    #endregion


    #region Update Logic

    void Update()
    //Core Logic Loop for player.
    {
        if (isMoving && InteractInput() != 0)
        //Logic used to queue up an action to do when reaching the next grid square.
        {
            buttonNum = InteractInput();
            waitForInteract = true;
            return;
        }

        else if (isMoving) { return; }

        //If statement triggers if a button is queued during the move coroutine.
        if (waitForInteract)
        {
            InteractHandling(buttonNum);
            buttonNum = 0;
            waitForInteract = false;
            return;
        }
        else
        {
            InteractHandling(InteractInput());
        }

        if (!isTalking)
        //Prevents a player from moving while the dialogue box is open.
        {
            input = GetMoveInput();     //Get the input from the player.
            GetMoveAndLookDir(input);   //Move the player if possible.
        }
        else
        {
            input = new Vector2(0f, 0f);
        }

        UpdateAnimations(input);    //Update player animations.
    }

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

    #endregion


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
                StartCoroutine(MovePlayerCo(new Vector3(input.x, input.y, 0f)));
            }
        }
    }

    private IEnumerator MovePlayerCo(Vector3 direction)
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


    #region Interaction Logic

    int InteractInput()
    //Gets the input from the two action buttons.
    {
        if (Input.GetButtonDown("Action"))
        //First action button. Reads signs, dashes, throws
        {
            return 1;
        }
        else if (Input.GetButtonDown("Action2"))
        //Second action button. Picks things up and puts them down.
        {
            return 2;
        }
        else
        //No button pressed, so input is false;
        {
            return 0;
        }
    }

    void InteractHandling(int buttonNum)
    //Takes the input button and calls the proper interaction.
    {
        if(buttonNum == 1)
        {
            Interact1();
        }
        else if(buttonNum == 2)
        {
            Interact2();    
        }
        else
        {}
    }

    private (bool, RaycastHit2D) CheckInFront(int playerViewRange)
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

    void Interact1()
    //Interaction using the first button.
    {
        //Cast a ray one tile in front of the player.
        (bool itemInFront, RaycastHit2D hit) = CheckInFront(playerViewRange); //Physics2D.Raycast(transform.position, transform.TransformDirection(lookDirection), playerViewRange);
        //Debug.DrawRay(transform.position, playerViewRange * transform.TransformDirection(lookDirection), Color.red, 0.25f);

        if (itemInFront)    //Ray hits an object.
        {
            if (hit.transform.gameObject.GetComponent<InteractableTalk>())
            {
                isTalking = !isTalking;
                SLBEvents.current.PlayerReadInteractable(hit.transform.gameObject.GetInstanceID());
            }
        }
        
        //Box throwing logic
        if (heldObject != null && heldObject.tag == "Box")
        {
            bool throwCheckFar = CheckInFront(2*playerViewRange).Item1;     //True if object 2 tiles ahead of player.
            
            if (!throwCheckFar)         //If there is no object within 2 tiles of player.
            {
                Debug.Log("2 tile throw.");
            }
            else
            {
                bool throwCheckClose = CheckInFront(playerViewRange).Item1; //True if object 1 tiles ahead of player.

                if (!throwCheckClose)   //If there is no object in front of the player.
                {
                    Debug.Log("1 tile throw.");
                }
                else
                {
                    Debug.Log("No throw.");
                    //Don't do anything with the box.
                }
                
            }
            
        }

    }

    void Interact2()
    //Interaction using the second button.
    {
        //Cast a ray one tile in front of the player.
        (bool itemInFront, RaycastHit2D hit) = CheckInFront(playerViewRange); //Physics2D.Raycast(transform.position, transform.TransformDirection(lookDirection), playerViewRange);
        Debug.DrawRay(transform.position, playerViewRange * transform.TransformDirection(lookDirection), Color.red, 0.25f);

        if (itemInFront)
        //Ray hits an object.
        {
            if (hit.transform.gameObject.GetComponent<InteractableCarry>() && type == PlayerType.nm)
            {
                heldObject = hit.transform.gameObject.GetComponent<InteractableCarry>();
                SLBEvents.current.PlayerPickUpCarryable(this.gameObject, heldObject.GetInstanceID());

                //Update player animations.
                if (heldObject.transform.tag == "Mower")
                {
                    type = PlayerType.wm;
                    animator.SetBool("HasMower", true);
                }
                else
                {
                    type = PlayerType.carry;
                    animator.SetBool("IsCarrying", true);
                }
            }
        }
        else
        //Ray does not hit an object.
        {
            if (type == PlayerType.wm)
            //Debug.Log("Put down mower in front of player.");
            {
                SLBEvents.current.PlayerPutDownCarryable(this.gameObject, lookDirection, heldObject.GetInstanceID());
                type = PlayerType.nm;
                animator.SetBool("HasMower", false);
                
            }
            else if (type == PlayerType.carry)
            //Debug.Log("Put down heldObject in front of player.");
            {
                SLBEvents.current.PlayerPutDownCarryable(this.gameObject, lookDirection, heldObject.GetInstanceID());
                heldObject = null;

                //Update player animations.
                type = PlayerType.nm;
                animator.SetBool("IsCarrying", false);

            }
        }

    }
    #endregion


    #region Trigger Logic
    void OnTriggerEnter2D(Collider2D otherObj)
    {
        if (otherObj.CompareTag("Grass") && this.gameObject.activeInHierarchy == true)
        {
            GameEvents.current.EnterGrassSquare((int)type, otherObj.transform.GetInstanceID());
        }
    }

    void OnTriggerStay2D(Collider2D otherObj)
    {
        if (otherObj.CompareTag("Grass") && this.gameObject.activeInHierarchy == true)
        {
            GameEvents.current.EnterGrassSquare((int)type, otherObj.transform.GetInstanceID());
        }
    }
    #endregion

}