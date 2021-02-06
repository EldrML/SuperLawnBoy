using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*-------------------------------
Game Controller
    This script/object keeps track of the game's important information, like how much grass needs to be cut in each room.
---------------------------------*/

public class GameController : MonoBehaviour
{
    [SerializeField] int grassInRoom;
    [SerializeField] int grassInLevel;
    [SerializeField] PolygonCollider2D currentRoom;

    // Start is called before the first frame update
    void Start()
    {
        grassInLevel = GameObject.FindGameObjectsWithTag("Grass").Length;

        //subscribe Listeners.
        GameEvents.current.onGrassCut -= GrassIsCut;
        GameEvents.current.onGrassCut += GrassIsCut;

        
    }

    // Update is called once per frame
    void Update()
    {
        //grassInLevel = GameObject.FindGameObjectsWithTag("Grass").Length;
    }

    void GrassIsCut()
    {   
        if (grassInLevel > 0)
        {
            grassInLevel -= 1;

            if (grassInLevel == 0)
            {
                GameEvents.current.allGrassIsCut();
                Debug.Log("ALL GRASS CUT!");
            }
        }
        
    }
}
