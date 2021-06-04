using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/*-------------------------------
Game Controller
    This script/object keeps track of the game's important information, like how much grass needs to be cut in each room.
---------------------------------*/

public class GameController : MonoBehaviour
{
    //[SerializeField] int grassInRoom;
    [SerializeField] int roomsToClear;
    [SerializeField] RoomManager currentRoomManager;
    public static int RoomGrassCount;
    public static int LevelGrassCount;
    public static int InitialLevelCount;

    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("For when you eventually have multiple scenes but want to keep the game manager running");
        //Debug.Log("https://stackoverflow.com/questions/35890932/unity-game-manager-script-works-only-one-time/35891919#35891919");

        roomsToClear = GameObject.FindGameObjectsWithTag("Room").Length;
        LevelGrassCount = GameObject.FindGameObjectsWithTag("Grass").Length;

        //Subscribe Listeners.
        GameEvents.current.onGrassCut -= GrassIsCut;                                //Event to track total amount of grass in the area.
        GameEvents.current.onGrassCut += GrassIsCut;
        
        GameEvents.current.onAllGrassIsCut -= AllGrassIsCut;
        GameEvents.current.onAllGrassIsCut += AllGrassIsCut;

        GameEvents.current.onChangeRoom -= ChangeRoomManager;
        GameEvents.current.onChangeRoom += ChangeRoomManager;  
    }

    int CountGrassInRoom()
    {
        Transform grassList = currentRoomManager.transform.Find("GrassList");
        int grassCount = 0;
        foreach (Transform t in grassList)
        {
            if (t.gameObject.activeInHierarchy)
            {
                grassCount += 1;
            }
        }
        return grassCount;
    }

    void ChangeRoomManager(RoomManager newRoomManager)
    //Update the room being affected by the player.
    {
        if (RoomGrassCount > 0)
        {
            //Subtract remaining grass in the room.
            LevelGrassCount -= RoomGrassCount;

            //Reactivate all the grass in the room.
            currentRoomManager.ResetRoom();

            //Get the total amount of grass in the previous room.
            int LastRoomGrassCount = CountGrassInRoom();

            //Add the full amount of grass back.
            LevelGrassCount += LastRoomGrassCount;

            //Swap to the new room manager.
            currentRoomManager = newRoomManager;

            //Get the total amount of grass in the current room.
            RoomGrassCount = CountGrassInRoom();
        }
        else
        {
            currentRoomManager = newRoomManager;
            RoomGrassCount = CountGrassInRoom();
        }
    }

    void GrassIsCut(int id)
    {
        if (id == currentRoomManager.transform.GetInstanceID())
        {
            if (RoomGrassCount > 0)
            {
                //REDUCE GRASS COUNTER WITHIN THE ROOM
                    RoomGrassCount -= 1;
                //REDUCE TOTAL GRASS AMOUNT
                    LevelGrassCount -= 1;

                if (RoomGrassCount == 0)
                {
                    GameEvents.current.allGrassIsCut(currentRoomManager.transform.GetInstanceID()); //ID not necessary here, just triggering the victory in the room.
                    Debug.Log("ALL GRASS in room CUT!");
                    //Debug.Log("May need a coroutine to prevent the gate from opening too fast.");
                }
            }
        }
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

