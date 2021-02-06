using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

/*
Room Manager
    This script is attached to Room to facilitate moving between rooms, and counting the number of objects (Grass) in each room.
    May not grow too complicated, but just in case.
*/

public class RoomManager : MonoBehaviour
{
    [SerializeField] float dampingSpeed;
    [SerializeField] CinemachineConfiner cinConfiner;

    // Start is called before the first frame update
    private void Start()
    {
        cinConfiner = Camera.main.GetComponentInChildren<CinemachineConfiner>();
    }

    private void OnTriggerEnter2D(Collider2D otherObj)
    //When entering a new room.
    {
        if (otherObj.CompareTag("Player"))
        {
            //StartCoroutine(SmoothRoomTransition());
            cinConfiner.m_BoundingShape2D = GetComponentInParent<PolygonCollider2D>();

            //#TODO: Put in an event here for when entering a new room!
        }
    }

    private IEnumerator SmoothRoomTransition()
    {
        cinConfiner.m_Damping = dampingSpeed;
        yield return new WaitForSeconds(dampingSpeed);
        cinConfiner.m_Damping = 0;
    }
}