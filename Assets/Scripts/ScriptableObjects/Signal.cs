using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Signal : ScriptableObject
{
    public List<SignalListener> listeners = new List<SignalListener>();

    public void Raise()
    {
        //Moving backwards through loop to avoid index error.
        for (int ii  = listeners.Count - 1; ii>=0; ii--) 
        {
            listeners[ii].OnSignalRaised();
        }
    }

    public void RegisterListener(SignalListener listener)
    {
            listeners.Add(listener);

    }

    public void DeRegisterListener(SignalListener listener)
    {
        listeners.Remove(listener);
    }
    
}
