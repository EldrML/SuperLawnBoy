using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mower : MonoBehaviour
{
    //public Vector2 mowerPosition;
    public Vector2 mowerDirection;

    public Animator mowerAnimator;
    // Start is called before the first frame update

    void Start()
    {
        mowerDirection = Vector2.left;
        //mowerPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(this.isActiveAndEnabled)
        {
                mowerAnimator.SetFloat("MowerHorDir", mowerDirection.x);
                mowerAnimator.SetFloat("MowerVerDir", mowerDirection.y);
        }
    }
}
