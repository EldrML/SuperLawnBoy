using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    # region Variable Definitions
    public PlayerController player;
    public SpriteRenderer sprite;
    public Vector3 carryHeight;
    public Rigidbody2D gravityBody;
    public Vector3 throwLocation;

    public enum BoxStates
    {
        onGround, isHeld, isThrown     // With/without lawnmower
    }
    public BoxStates boxState;
    // public bool isHeld;
    // public bool isThrown; 
    public LayerMask ignoreGrass;

    //Throw Variables
    [SerializeField] float launchAngle;
    // [SerializeField] float g = -9.81f;
    // [SerializeField] float airtime = 1f;
    // [SerializeField] float launchSpeed;
    [SerializeField] RaycastHit2D throwHit;

    float height = 4f;
    bool startThrow = false;
    float incrementor = 0;

    # endregion

    void Start()
    {
        // TODO : Improve the method of assigning the player controller. Needs to scale to SEVERAL objects.
        boxState = BoxStates.onGround;
        player = (PlayerController)FindObjectOfType(typeof(PlayerController));
        sprite = GetComponent<SpriteRenderer>();
        gravityBody = GetComponent<Rigidbody2D>();
        launchAngle = Mathf.PI * 45 / 180;
    }

    void Update()
    {
        //Being carried update.
        if (boxState == BoxStates.isHeld)
        {
            transform.position = player.transform.position + carryHeight;
            //throwLocation = player.transform.position + new Vector3(2 * player.lookDirection.x, 2 * player.lookDirection.y, 0);
        }



        // //Being thrown update.
        if (boxState == BoxStates.isThrown)
        {
            ThrowBox(player.transform.position + carryHeight, throwLocation, boxState == BoxStates.isThrown);
        }

        //     // Vector3 test = transform.position - throwLocation;
        //     // Debug.Log(test.magnitude);

        //     // // if (test.magnitude <= 1.5)
        //     // // {
        //     //     boxState = BoxStates.onGround;
        //     //     player.currentType = PlayerController.PlayerType.nm;
        //     //     sprite.sortingLayerName = "Interactive";
        //     // // }
        // }

    }

    public void PickUpAndDrop()
    {
        if (player.CanMove())
        {
            if (player.frontData.hit)
            {
                if (player.currentType == PlayerController.PlayerType.nm &&
                    player.frontData.hit.collider.tag == this.tag)
                {
                    boxState = BoxStates.isHeld;
                    sprite.sortingLayerName = "CarryItem";
                    player.currentType = PlayerController.PlayerType.carry;
                }
            }

            if (player.currentType == PlayerController.PlayerType.carry && player.frontData.isEmpty)
            {
                boxState = BoxStates.onGround;
                transform.position = player.transform.TransformPoint(player.lookDirection);
                player.currentType = PlayerController.PlayerType.nm;
                sprite.sortingLayerName = "Interactive";
            }
            else { }
        }
    }

    public void CheckForThrow()
    {
        if (player.CanMove())
        {
            throwHit = Physics2D.Raycast(player.transform.position,
                                        player.transform.TransformDirection(player.lookDirection),
                                        2f, ~ignoreGrass); //#TODO This is where you can ignore layers when checking for a throw. Spikes and stuff might matter later.


            //If there is a collision within two tiles.
            if (throwHit)
            {
                if (throwHit.distance < 1) //Check first tile ahead.
                {
                    if (throwHit.collider.tag == "Mower")
                    {
                        Debug.Log("Throw one tile to destroy enemy.");
                        throwLocation = player.transform.position + new Vector3(player.lookDirection.x, player.lookDirection.y, 0);
                        boxState = BoxStates.isThrown;
                    }
                    else
                    {
                        Debug.Log("Can't throw here.");
                    }
                }
                else if (throwHit.distance < 2) //Check second tile ahead.
                {
                    if (throwHit.collider.tag == "Mower")
                    {
                        Debug.Log("Throw two tiles to destroy enemy.");
                        throwLocation = player.transform.position + new Vector3(2 * player.lookDirection.x, 2 * player.lookDirection.y, 0);
                        boxState = BoxStates.isThrown;
                    }
                    else
                    {
                        Debug.Log("Throw one tile to avoid collision.");
                        throwLocation = player.transform.position + new Vector3(player.lookDirection.x, player.lookDirection.y, 0);
                        boxState = BoxStates.isThrown;
                    }
                }
            }
            else
            {
                Debug.Log("Throw two tiles.");
                throwLocation = player.transform.position + new Vector3(2 * player.lookDirection.x, 2 * player.lookDirection.y, 0);
                boxState = BoxStates.isThrown;
            }

        }
    }

    void ThrowBox(Vector3 startPos, Vector3 endPos, bool startThrow)
    {

        // Update is called once per frame
        //boxState = BoxStates.isThrown; //#TODO this state may be unnecessary, but for now let's keep it in.
        //Vector3 startPos = player.transform.position + carryHeight;
        //Vector3 endPos = throwLocation;

        if (startThrow)
        {
            incrementor += 0.03f;
            Vector3 currentPos = Vector3.Lerp(startPos, endPos, incrementor);
            currentPos.z += height * Mathf.Sin(Mathf.Clamp01(incrementor) * Mathf.PI);
            transform.position = currentPos;
        }
        if (transform.position == endPos)
        {
            startThrow = false;
            incrementor = 0;
            Vector3 tempPos = startPos;
            startPos = transform.position;
            endPos = tempPos;

            boxState = BoxStates.onGround;
            player.currentType = PlayerController.PlayerType.nm;
            sprite.sortingLayerName = "Interactive";
        }
        // if (boxState == BoxStates.isThrown)
        // {
        //     startThrow = true;
        // }
    }

}
