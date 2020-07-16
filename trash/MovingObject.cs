using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour
{
    public float moveTime = 0.1f;
    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb2D;
    private float inverseMoveTime;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        inverseMoveTime = 1f / moveTime;
    }

    // Define this function for documentation purposes
    protected bool Move (int xDir, int yDir, out RaycastHit2D hit) //OUT used to return more than one value from the function.
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2 (xDir, yDir);

        boxCollider.enabled = false;
        hit = Physics2D.Linecast (start, end, blockingLayer);
        boxCollider.enabled = true;

        if (hit.transform == null) //IF the space the raycast check was empty...
        {
            StartCoroutine(SmoothMovement (end));
            return true;
        }

        return false;
    }

    // Explain this Coroutine for documentation purposes
    protected IEnumerator SmoothMovement (Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon) //FIXME: this may be overkill. See: https://stackoverflow.com/a/30217218
        {
            Vector3 newPosition = Vector3.MoveTowards (rb2D.position, end, inverseMoveTime * Time.deltaTime);
            //NOTE: inverseMoveTime is computationally the same as doing (Time.deltaTime / MoveTime), but more efficient.
            rb2D.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;                                                 //Wait for a new frame before re-evaluating the loop.
        }
    }

    // Explain this function for documentation purposes
    protected virtual void AttemptMove <T> (int xDir, int yDir)
        where T : Component //Component is the generic form of what Unity element the UNIT will interact with, if blocked.
        {
            /*TODO Refactor all this mess of a code once we figure out what we want to in our movement.
            This shit is needlessly complicated for what we're trying to do. Probably. */
            RaycastHit2D hit;
            bool canMove = Move (xDir,yDir, out hit);

            if (hit.transform == null)
                return;

            T hitComponent = hit.transform.GetComponent<T>();

            if (!canMove && hitComponent != null)   
                OnCantMove(hitComponent);
            /*Essentially, if the Object is unable to move AND the component it hit IS NOT null, 
            then the OnCantMove function will run, running differently based on what the type of object is.*/
        }

    /// <summary>
    /// TODO Explain this function for documentation purposes
    /// </summary>
    protected abstract void OnCantMove <T> (T component)
        where T : Component;


}
