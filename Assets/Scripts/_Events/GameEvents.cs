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
    

    public event Action onGrassCut;
    public void GrassIsCut()
    {
        onGrassCut?.Invoke(); 
        //Reference: https://www.youtube.com/watch?v=oc3sQamIh-Q
    }
    
    public event Action onAllGrassIsCut;
    public void allGrassIsCut()
    {
        onAllGrassIsCut?.Invoke(); 
    }

    // public event Action<Vector2, Vector2> onMoveRoom;
    // public void MoveCameraRoomLimits(Vector2 newCameraLimitsX, Vector2 newCameraLimitsY)
    // {
    //     onMoveRoom?.Invoke(newCameraLimitsX, newCameraLimitsY); 
    // }
}
