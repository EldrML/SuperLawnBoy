using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject dBox;
    public Text dText;

    void Start()
    // Start is called before the first frame update
    {
        //dialogueActive = false;
        dText.text = "";   
    }

    public void ShowBox(string dialog)
    //Makes the dialog box visible and changes the text inside.
    {
        //dBox.SetActive(true);
        dBox.SetActive(!dBox.activeInHierarchy);
        dText.text = dialog;
    }
}
