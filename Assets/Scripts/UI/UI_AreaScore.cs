using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_AreaScore : MonoBehaviour
{
    //public static int AreaGrassCount_value;
    Text AreaGrassCount_text;

    // Start is called before the first frame update
    void Start()
    {
        AreaGrassCount_text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        AreaGrassCount_text.text = GameController.LevelGrassCount.ToString();
    }
}
