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
    //[SerializeField] UI_RoomScore ui_roomScore;
    public static int RoomGrassCount;
    public static int LevelGrassCount;
    public static int InitialRoomCount;
    public static int InitialLevelCount;
    //[SerializeField] PolygonCollider2D currentRoom;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("For when you eventually have multiple scenes but want to keep the game manager running");
        Debug.Log("https://stackoverflow.com/questions/35890932/unity-game-manager-script-works-only-one-time/35891919#35891919");

        roomsToClear = GameObject.FindGameObjectsWithTag("Room").Length;
        LevelGrassCount = GameObject.FindGameObjectsWithTag("Grass").Length;
        RoomGrassCount = currentRoomManager.grassInRoom;
        InitialRoomCount = RoomGrassCount;
        InitialLevelCount = LevelGrassCount;
        //grassInLevel = GameObject.FindGameObjectsWithTag("Grass").Length;
        //UI_AreaScore.AreaGrassCount_value = grassInLevel;

        //subscribe Listeners.
        GameEvents.current.onGrassCut -= GrassIsCut;                                //Event to track total amount of grass in the area.
        GameEvents.current.onGrassCut += GrassIsCut;
        
        GameEvents.current.onAllGrassIsCut -= AllGrassIsCut;
        GameEvents.current.onAllGrassIsCut += AllGrassIsCut;

        GameEvents.current.onChangeRoom -= ChangeRoomManager;
        GameEvents.current.onChangeRoom += ChangeRoomManager;

        
    }

    void ChangeRoomManager(RoomManager newRoomManager)
    //Update the room being affected by the player. May not be needed?
    {
        //grassInLevel = UI_AreaScore.AreaGrassCount_value;
        if (RoomGrassCount > 0)
        {
            LevelGrassCount = InitialLevelCount;
            currentRoomManager = newRoomManager;
        }
        else
        {
            currentRoomManager = newRoomManager;
            //RoomGrassCount = currentRoomManager.grassInRoom;
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
                    GameEvents.current.allGrassIsCut(this.transform.GetInstanceID()); //ID not necessary here, just triggering the victory in the room.
                    Debug.Log("ALL GRASS in room CUT!");
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
