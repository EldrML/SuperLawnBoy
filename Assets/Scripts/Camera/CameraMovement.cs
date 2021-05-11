using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{
    public Animator animator;

    private void Start()
    {
        GameEvents.current.onScreenFade -= WhiteFadeIn;
        GameEvents.current.onScreenFade += WhiteFadeIn;
    }

    public void WhiteFadeIn(int id, Vector3 outputPosition)
    {
        animator.SetTrigger("FadeIn");
        PlayerEvents.current.PlayerTeleport(id, outputPosition); //Signal to PlayerMovement.cs, TransferPlayer function.
        transform.position = outputPosition;

        
    }

    public void OnTransferComplete()
    {
        animator.SetTrigger("FadeOut");
    }


}
