using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 1f;
    public Vector2 lookDir;
    public PlayerMovement playerScript;
    public Transform player;
    public float playerViewRange;

    public void Interact(int buttonNum, int state, int type, bool frontIsClear)
    {
        Debug.Log("" + buttonNum + state + type + frontIsClear);

        if(buttonNum == 1)  //First button
        {
            Debug.Log("Interacting with " + transform.name);
            if(type == 0)       //No Mower
            {
                InteractEmpty(frontIsClear);
            }
            else if(type == 1)  //Mower State
            {

            }
            else if(type == 2)  //Box Carry State
            {
                InteractCarry(frontIsClear);
            }
            else
            {

            }
        }
        
    }

    public virtual void InteractEmpty(bool frontIsClear)
    {
        //This method to be overwritten/appended.
        //-----
        {
            Debug.Log("Interacting with " + transform.name + " while not carrying anything.");
        }
        
    }

    public virtual void InteractCarry(bool frontIsClear)
    {
        //This method to be overwritten/appended.
        //-----
        {
            Debug.Log("Interacting with " + transform.name + " while carrying something.");
        }
        
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    // // Update is called once per frame
    // public virtual void Update()
    // {
    //     if (player != null)
    //     {
    //         // if (this.transform.position - this.player.transform.position != new Vector3(lookDir.x, lookDir.y, 0f))
    //         // {
    //         //     Debug.Log("No Longer In Range");
    //         //     player = null;
    //         //     Debug.Log("Note, if this is triggering, make sure the object is exactly on the grid.");
    //         //     Debug.Log("There is currently an issue with the if statement above. Might be able to fix with rounding?");
    //         // }
    //     }
    // }
}
