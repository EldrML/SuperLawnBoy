using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform movePoint;
    public LayerMask whatStopsMovement;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
    
        Vector3 horizontal = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
        Vector3 vertical = new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
        
        

        //If the distance between the player and the movepoint is less than 0.5 units...
        if(Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            if(Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
            {
                if(!Physics2D.OverlapCircle(movePoint.position + horizontal, .2f, whatStopsMovement))
                {   
                    animator.SetFloat("Horizontal", Input.GetAxisRaw("Horizontal"));
                    animator.SetFloat("Vertical", 0f);
                    movePoint.position += horizontal;
                    return;
                }
            }
            else if(Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                if(!Physics2D.OverlapCircle(movePoint.position + vertical, .2f, whatStopsMovement))
                {
                    animator.SetFloat("Vertical", Input.GetAxisRaw("Vertical"));
                    animator.SetFloat("Horizontal", 0f);
                    movePoint.position += vertical;
                    return;
                }
                
            }
            else
            {
                animator.SetFloat("Vertical", 0f);
                animator.SetFloat("Horizontal", 0f);
                return;
            }
        }
    }
}
