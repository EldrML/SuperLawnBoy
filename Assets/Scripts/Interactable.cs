using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 1f;
    public bool inRange = false;
    public bool isFocus = false;
    //int buttonNum;
    public Transform player;
    //public PlayerController player;
    // Start is called before the first frame update
    // void Start()
    // {

    // }

    public virtual void Interact(int buttonNum, int state)
    {
        //This method to be overwritten.
        //-----

        float distance = Vector2.Distance(player.position, transform.position);
        if (distance <= radius)
        {
            inRange = true;
        }

        if(inRange)
        {
            Debug.Log("Interacting with " + transform.name);
        }
        
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if(player != null && isFocus)
        {
            float distance = Vector2.Distance(player.position, transform.position);
            if(distance <= radius)
            {
                //inRange = true;
            }
            else
            {
                inRange = false;
                isFocus = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D otherObj)
    {
        if (otherObj.CompareTag("Player"))
        {
            inRange = true;
        }
        
    }
}
