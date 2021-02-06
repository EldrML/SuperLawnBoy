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
    [SerializeField] int grassInRoom;
    [SerializeField] GameObject grassList;
    [SerializeField] CinemachineConfiner cinConfiner;

    // Start is called before the first frame update
    private void Start()
    {
        GameEvents.current.onGrassCut -= GrassIsCut;
        GameEvents.current.onGrassCut += GrassIsCut; //Add this to your list of subscribed events.

        grassInRoom = grassList.transform.childCount;
        cinConfiner = Camera.main.GetComponentInChildren<CinemachineConfiner>();
    }

    private void OnRoomVisible()
    {
        GameEvents.current.onGrassCut -= GrassIsCut;
        GameEvents.current.onGrassCut += GrassIsCut; //Add this to your list of subscribed events.
    }

    private void OnRoomInvisible()
    {
        GameEvents.current.onGrassCut -= GrassIsCut; //Add this to your list of subscribed events.
    }

    private void OnTriggerEnter2D(Collider2D otherObj)
    //When entering a new room.
    {
        if (otherObj.CompareTag("Player"))
        {
            //StartCoroutine(SmoothRoomTransition());
            cinConfiner.m_BoundingShape2D = GetComponentInParent<PolygonCollider2D>();
            GameEvents.current.ChangeRoom(this);
            //gameController.currentRoom

            //#TODO: Put in an event here for when entering a new room!
        }
    }

    private void OnTriggerExit2D(Collider2D otherObj)
    //When leaving the current room.
    {
        if (otherObj.CompareTag("Player"))
        {
            if (grassInRoom > 0)
            {
                ResetRoom();
            }
            
        }
    }

    private IEnumerator SmoothRoomTransition()
    {
        cinConfiner.m_Damping = dampingSpeed;
        yield return new WaitForSeconds(dampingSpeed);
        cinConfiner.m_Damping = 0;
    }

    void ResetRoom()
    {
        //TODO create an event to reset the grass in a room if it isnt all cut
        for (int i = 0; i < grassList.transform.childCount; i++)
        {
            grassList.transform.GetChild(i).gameObject.SetActive(true);
            grassList.transform.GetChild(i).GetComponent<Grass1>().grassHasBeenCut = false;
        }
        grassInRoom = grassList.transform.childCount;
    }

    void GrassIsCut(int id)
    {
        if (id == this.transform.GetInstanceID())
        {
            if (grassInRoom > 0)
            {
                grassInRoom -= 1;

                if (grassInRoom == 0)
                {
                    GameEvents.current.allGrassIsCut(this.transform.GetInstanceID()); //ID not necessary here, just triggering the victory in the room.
                    Debug.Log("ALL GRASS in room CUT!");
                }
            }
        }



    }
}