using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{

    // [SerializeField] Transform playerTransform;
    // [SerializeField] CinemachineVirtualCamera _virtualCamera;
    public Animator animator;

    private void Start()
    {
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
        PlayerEvents.current.TeleportPlayer(id, outputPosition); //Signal to PlayerMovement.cs, TransferPlayer function.
        transform.position = outputPosition;

        
    }

    public void OnTransferComplete()
    {
        Debug.Log("COMPLETE");
        animator.SetTrigger("FadeOut");
    }


}
