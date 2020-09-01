using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variable Definitions
    public Transform movePoint;
    public Animator animator;
    public GameObject interactiveObject;
    public Mower mower;
    public Vector2 lookDirection;

    public float moveSpeed;
    public bool reading = false;

    public enum PlayerState
    {
        move, hurt, dash, Action
    }
    public enum PlayerType
    {
        nm, wm     // With/without lawnmower
    }
    public PlayerState currentState;
    public PlayerType currentType;


    Vector2 input;
    float playerViewRange = 1f;
    public (bool isEmpty, RaycastHit2D hit) frontData;
    // public bool isEmpty = true;
    // public RaycastHit2D hit;
    //(PlayerState currentState, PlayerType currentType)playerSwitches; Tuple for simplifying things later on?
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
        if (currentState == PlayerState.move)
        {
            frontData = CheckIfEmpty();

            if (Input.GetButtonDown("Action"))
            {
                if (currentType == PlayerType.wm && frontData.isEmpty)
                {
                    StartCoroutine(ActionCo());
                }
                // else if (currentType == PlayerType.wm && frontData.hit)
                // {
                //     EvaluateHit();
                // }
                else
                {
                    EvaluateHit();
                }
            }
            else if (Input.GetButtonDown("Action2")) // Pick up the mower.
            {
                PickAndDropMower();
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
        yield return new WaitUntil(() => CanMove());
        animator.SetBool("Action", true);
        currentState = PlayerState.Action;
        yield return null;
        animator.SetBool("Action", false);
        yield return new WaitForSeconds(1f);
        currentState = PlayerState.move;
    }

    bool CanMove() // Checks if there is a collision object in the direction of desired movement.
    {
        if (Vector3.Distance(transform.position, movePoint.position) <= 0.0001f)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(input), playerViewRange);
            Debug.DrawRay(transform.position, playerViewRange * transform.TransformDirection(input), Color.red, 0.25f);

            if (hit)
            {
                // Exceptions to the hit raycast.
                if (hit.collider.tag == "Grass")
                {
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

    (bool, RaycastHit2D) CheckIfEmpty() // Check if there is something in front of player when called.
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(lookDirection), playerViewRange);
        Debug.DrawRay(transform.position, playerViewRange * transform.TransformDirection(lookDirection), Color.red, 0.25f);

        if (frontData.hit)
        {
            // -- ADD HIT EXCEPTIONS HERE -- //
            if (frontData.hit.collider.tag == "Grass")
            {
                return (true, hit);
            }
            return (false, hit);
        }
        else
        {
            return (true, hit);
        }
    }

    void EvaluateHit() //Check things in front of the player.
    {
        //Useful for talking to NPCs and interacting with things...
        
        //#TODO: TRY TO MAKE THIS FUNCTION UNNECESSARY BY HAVING THE FUNCTIONALITY BE HANDLED IN OTHER OBJECT SCRIPTS, WHERE POSSIBLE.
        RaycastHit2D hit = frontData.hit;

        if (frontData.hit.collider.tag == "Wall")
        {}
        if (frontData.hit.collider.tag == "Sign")
        {}
        else
        {}
    }

    void PickAndDropMower() //Push a button to grab and release the mower.
    {
        if (CanMove())
        {
            if (currentType == PlayerType.nm && frontData.hit.collider.tag == "Mower")
            {
                mower.gameObject.SetActive(false);
                currentType = PlayerType.wm;
                animator.SetBool("HasMower", true);
            }
            else if (currentType == PlayerType.wm && frontData.isEmpty)
            {
                mower.transform.position = this.transform.TransformPoint(lookDirection);
                mower.mowerDirection = lookDirection;
                mower.gameObject.SetActive(true);
                currentType = PlayerType.nm;
                animator.SetBool("HasMower", false);
            }
            else { }
        }
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

    void TryToDropMower() //Similar to CanMove function, but hopefully can be expanded if needed.
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(lookDirection), playerViewRange);
        Debug.DrawRay(transform.position, playerViewRange * transform.TransformDirection(lookDirection), Color.red, 0.25f);
        if (!hit && CanMove())
        {

            mower.transform.position = this.transform.TransformPoint(lookDirection);
            mower.mowerDirection = lookDirection;
            mower.gameObject.SetActive(true);
            currentType = PlayerType.nm;
            animator.SetBool("HasMower", false);
        }
        else { }
    }
    #endregion
}