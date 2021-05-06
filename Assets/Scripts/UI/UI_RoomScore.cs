using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_RoomScore : MonoBehaviour
{
    //public static int RoomGrassCount_value;
    Text RoomGrassCount_text;
    
    // Start is called before the first frame update
    void Start()
    {
        RoomGrassCount_text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        RoomGrassCount_text.text = GameController.RoomGrassCount.ToString();
    }
}
