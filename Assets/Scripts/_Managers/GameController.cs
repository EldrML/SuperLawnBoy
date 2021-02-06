using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*-------------------------------
Game Controller
    This script/object keeps track of the game's important information, like how much grass needs to be cut in each room.
---------------------------------*/

public class GameController : MonoBehaviour
{
    //[SerializeField] int grassInRoom;
    [SerializeField] int grassInLevel, roomsToClear;
    [SerializeField] RoomManager currentRoomManager;
    //[SerializeField] PolygonCollider2D currentRoom;

    // Start is called before the first frame update
    void Start()
    {
        roomsToClear = GameObject.FindGameObjectsWithTag("Room").Length;
        grassInLevel = GameObject.FindGameObjectsWithTag("Grass").Length;
        //grassInRoom = 

        //subscribe Listeners.
        GameEvents.current.onAllGrassIsCut -= AllGrassIsCut;
        GameEvents.current.onAllGrassIsCut += AllGrassIsCut;

        GameEvents.current.onChangeRoom -= ChangeRoomManager;
        GameEvents.current.onChangeRoom += ChangeRoomManager;

        
    }

    void ChangeRoomManager(RoomManager newRoomManager)
    //Update the room being affected by the player. May not be needed?
    {
        currentRoomManager = newRoomManager;
    }

    void AllGrassIsCut(int id)
    //id shouldn't be needed here, there is only one Game Controller for the level.
    //#TODO: Keep track of the number of rooms which have been cleared?
    {   
        if (roomsToClear > 0)
        {
            roomsToClear -= 1;

            if (roomsToClear == 0)
            {
                //GameEvents.current.allGrassIsCut(currentRoomManager.transform.GetInstanceID());
                Debug.Log("ALL ROOMS CLEAR!");
            }
        }
        
    }
}
