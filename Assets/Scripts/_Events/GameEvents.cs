using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Events;

public class GameEvents : MonoBehaviour
{
    /*
    This is the event handler for events that are not necessarily related to the player.
    For example, grass being cut/animations playing, enemies/npcs moving around the map, and more.
    */

    #region Setup Logic
    public static GameEvents current;

    private void Awake() 
    { current = this; }
    #endregion


    public event Action<int, int> onEnterGrassSquare;
    public void EnterGrassSquare(int playerType, int id)
    { 
        onEnterGrassSquare?.Invoke(playerType, id); 
    }
    

    public event Action<int> onGrassCut;
    public void GrassIsCut(int id)
    {
        onGrassCut?.Invoke(id); 
        //Reference: https://www.youtube.com/watch?v=oc3sQamIh-Q
    }
    
    public event Action<int> onAllGrassIsCut;
    public void allGrassIsCut(int id)
    {
        onAllGrassIsCut?.Invoke(id); 
    }

    public event Action<RoomManager> onChangeRoom;
    public void ChangeRoom(RoomManager newRoom)
    {
        onChangeRoom?.Invoke(newRoom); 
    }
}
