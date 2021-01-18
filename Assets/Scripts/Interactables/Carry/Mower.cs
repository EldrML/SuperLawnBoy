using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mower : InteractableCarry
{
    public Vector2 mowerDirection;
    public Animator mowerAnimator;

    protected override void Start()
    {
        base.Start();
        mowerDirection = Vector2.left;
        mowerAnimator.SetFloat("MowerHorDir", mowerDirection.x);
        mowerAnimator.SetFloat("MowerVerDir", mowerDirection.y);
    }

    protected override void PickUpCarryObj(GameObject parentObject, int id)
    {
        if (id == this.GetInstanceID())
        {
            carryState = CarryStates.isHeld;
            transform.SetParent(parentObject.transform);
            transform.position = parentObject.transform.position;

            spriteRenderer.enabled = false;
            boxCollider.enabled = false;
            //gameObject.SetActive(false);

        }
    }

    protected override void PutDownCarryObj(GameObject parentObject, Vector2 lookDirection, int id)
    {
        if (id == this.GetInstanceID())
        {
            carryState = CarryStates.onGround;
            transform.position = parentObject.transform.TransformPoint(lookDirection);
            mowerDirection = lookDirection;
            mowerAnimator.SetFloat("MowerHorDir", mowerDirection.x);
            mowerAnimator.SetFloat("MowerVerDir", mowerDirection.y);
            
            //gameObject.SetActive(true);
            spriteRenderer.enabled = true;
            boxCollider.enabled = true;

            //Reset the mower.
            transform.SetParent(initialParent);
            playerTransform = null;

        }
    }
}
