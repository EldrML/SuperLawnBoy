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
    [SerializeField] GameObject grassList;
    [SerializeField] GameObject virtualCamera;

    private void Start() {}

    private void OnTriggerEnter2D(Collider2D otherObj)
    //When entering a new room. ALSO TRIGGERS AT BEGINNING OF GAME
    {
        if (otherObj.CompareTag("Player"))
        {
                GameEvents.current.ChangeRoom(this);
                virtualCamera.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D otherObj)
    //When entering a new room. ALSO TRIGGERS AT BEGINNING OF GAME
    {
        if (otherObj.CompareTag("Player"))
        {
                virtualCamera.SetActive(false);
        }
    }


    public void ResetRoom()
    {
        for (int i = 0; i < grassList.transform.childCount; i++)
        {
            grassList.transform.GetChild(i).gameObject.SetActive(true);
        }
    }
}