using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTalk : Interactable
{
    /*
    An extension of the base Interactable class for objects you can talk to.
    */

    #region Variables

    public string dialog;

    private DialogueManager dMan;

    [SerializeField] Transform player;

    [SerializeField] Vector2 lookDirection;
    #endregion


    #region Setup Logic
    void Start()
    {
        SLBEvents.current.onPlayerReadInteractable -= ReadInteractable;    //Subscribe this listener.
        SLBEvents.current.onPlayerReadInteractable += ReadInteractable;    //Subscribe this listener.
        dMan = FindObjectOfType<DialogueManager>();
    }

    void OnEnable()
    //Set up events.
    {
        if (SLBEvents.current != null)
        {
            SLBEvents.current.onPlayerReadInteractable -= ReadInteractable;    //Subscribe this listener.
            SLBEvents.current.onPlayerReadInteractable += ReadInteractable;    //Subscribe this listener.
        }
    }

    private void OnDisable()
    //Remove the listener when the object is disabled.
    {
        SLBEvents.current.onPlayerReadInteractable -= ReadInteractable;
    }
    #endregion


    #region Event Logic
    private void ReadInteractable(int id)
    //Reference the dialogue manager for this part.
    {
        if (id == this.gameObject.GetInstanceID())
        {
            dMan.ShowBox(dialog);
        }
    }
    #endregion
}