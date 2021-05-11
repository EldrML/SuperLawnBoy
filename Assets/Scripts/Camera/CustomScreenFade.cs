using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomScreenFade : MonoBehaviour
{

    // [SerializeField] Transform playerTransform;
    // [SerializeField] CinemachineVirtualCamera _virtualCamera;
    public Animator animator;
    [SerializeField] float fadeInTime, fadeOutTime;

    private void Start()
    {
        //fadeInTime
        
        GameEvents.current.onScreenFade -= WhiteFadeIn;
        GameEvents.current.onScreenFade += WhiteFadeIn;
    }

    void PlayerEntersDoor(int id, Vector3 outputPosition)
    {
        // TeleportCamera();
        // Debug.Log("teleporting");
        // // this.gameObject.SetActive(false);
        // // transform.position = outputPosition;
        // // this.gameObject.SetActive(true);
    }

    // private void TeleportCamera()
    // {
    //     _virtualCamera.Follow = null;
    //     _virtualCamera.LookAt = null;

    //     StartCoroutine(UpdateCameraFrameLater());
    // }

    // private IEnumerator UpdateCameraFrameLater()
    // {
    //     yield return null;

    //     _virtualCamera.Follow = playerTransform;
    //     _virtualCamera.LookAt = playerTransform;
    // }

    public void WhiteFadeIn(int id, Vector3 outputPosition)
    {
        animator.SetTrigger("FadeIn");
        Debug.Log("CustomScreenFade.cs: TRIGGERED");
        StartCoroutine(TeleportPlayerAndCamera(id, outputPosition));

        
    }

    public void OnTransferComplete()
    {
        Debug.Log("COMPLETE");
        animator.SetTrigger("FadeOut");
    }

    private IEnumerator TeleportPlayerAndCamera(int id, Vector3 outputPosition)
    {
        Debug.Log("Hitting the coroutine");
        yield return new WaitForSecondsRealtime(fadeInTime);

        PlayerEvents.current.TeleportPlayer(id, outputPosition); //Signal to PlayerMovement.cs, TransferPlayer function.
        transform.position = outputPosition;

        
    }

}

