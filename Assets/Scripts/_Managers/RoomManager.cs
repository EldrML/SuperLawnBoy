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
    public int grassInRoom = 0; //TODO Remove Public variable and replace with event?
    [SerializeField] GameObject grassList;
    [SerializeField] CinemachineConfiner cinConfiner;

    // Start is called before the first frame update
    private void Start()
    {
        PolygonCollider2D triggerPoly = this.GetComponent<PolygonCollider2D>();
        triggerPoly.isTrigger = true;

        grassInRoom = CountGrass(grassList);

        GameController.RoomGrassCount = grassInRoom;                               //Count list of grass objects to cut.
        cinConfiner = Camera.main.GetComponentInChildren<CinemachineConfiner>();
    }

    private void OnRoomVisible()
    {
        //GameEvents.current.onGrassCut -= GrassIsCut;
        //GameEvents.current.onGrassCut += GrassIsCut; //Add this to your list of subscribed events.
    }

    private void OnRoomInvisible()
    {
        //GameEvents.current.onGrassCut -= GrassIsCut; //Add this to your list of subscribed events.
    }

    private void OnTriggerEnter2D(Collider2D otherObj)
    //When entering a new room. ALSO TRIGGERS AT BEGINNING OF GAME
    {
        Debug.Log(grassList.transform.parent.GetInstanceID());
        if (otherObj.CompareTag("Player"))
        {
            //StartCoroutine(SmoothRoomTransition());
            //Debug.Log(GameController.InitialRoomCount);
            //GameController.InitialRoomCount = grassList.transform.childCount;
            //Debug.Log(GameController.InitialRoomCount);
            //PREVIOUS ROOM LOGIC
            // if (GameController.RoomGrassCount > 0)
            // {
            //     GameController.LevelGrassCount -= GameController.RoomGrassCount;
            //         Debug.Log(GameController.LevelGrassCount);
            //     GameController.LevelGrassCount += GameController.InitialRoomCount;


            //     for (int i = 0; i < grassList.transform.childCount; i++)
            //     {
            //         grassList.transform.GetChild(i).gameObject.SetActive(true);

            //     }
            //     GameController.InitialRoomCount = grassList.transform.childCount;
            // }

                GameController.RoomGrassCount = CountGrass(grassList);

                //NEW ROOM LOGIC
                cinConfiner.m_BoundingShape2D = GetComponentInParent<PolygonCollider2D>();
                GameEvents.current.ChangeRoom(this);
                //GameController.RoomGrassCount = grassList.FindGameObjectsWithTag("Grass").Length;;
        }
    }

    private void OnTriggerExit2D(Collider2D otherObj)
    //When leaving the current room.
    {
        //if (otherObj.CompareTag("Player"))
        //{
            grassInRoom = CountGrass(grassList);
            if (grassInRoom > 0)
            {
                Debug.Log(grassInRoom);
                for (int i = 0; i < grassList.transform.childCount; i++)
                {
                    grassList.transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        //}
    }

    private IEnumerator SmoothRoomTransition()
    {
        cinConfiner.m_Damping = dampingSpeed;
        yield return new WaitForSeconds(dampingSpeed);
        cinConfiner.m_Damping = 0;
    }

    int CountGrass(GameObject grassList)
    {
        int grassCount = 0;
        foreach (Transform t in grassList.transform)
        {
            if (t.gameObject.activeInHierarchy)
            {
                grassCount += 1;
            }
        }

        return grassCount;
    }

    // void ResetRoom()
    // {
    //     Debug.Log(GameController.LevelGrassCount);
    //     Debug.Log(GameController.RoomGrassCount);
    //     GameController.LevelGrassCount -= grassInRoom;
    //     Debug.Log(GameController.LevelGrassCount);
    //     //TODO create an event to reset the grass in a room if it isnt all cut?
    //     for (int i = 0; i < grassList.transform.childCount; i++)
    //     {
    //         grassList.transform.GetChild(i).gameObject.SetActive(true);
    //     }
    //     GameController.RoomGrassCount = grassList.transform.childCount;
    //     Debug.Log(GameController.RoomGrassCount);
    //     GameController.LevelGrassCount += GameController.RoomGrassCount;
    //     Debug.Log(GameController.LevelGrassCount);
    //     // Debug.Log(GameController.RoomGrassCount);

    // }

    void GrassIsCut(int id)
    {
        if (id == this.transform.GetInstanceID())
        {
            if (grassInRoom > 0)
            {
                //REDUCE GRASS COUNTER WITHIN THE ROOM
                grassInRoom -= 1; 
                GameController.RoomGrassCount -= 1;

                //REDUCE TOTAL GRASS AMOUNT;
                GameController.RoomGrassCount -= 1;

                if (grassInRoom == 0)
                {
                    GameEvents.current.allGrassIsCut(this.transform.GetInstanceID()); //ID not necessary here, just triggering the victory in the room.
                    Debug.Log("ALL GRASS in room CUT!");
                }
            }
        }



    }
}