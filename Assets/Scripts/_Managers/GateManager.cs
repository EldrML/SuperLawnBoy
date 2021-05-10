using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.current.onAllGrassIsCut -= GateSwitch;
        GameEvents.current.onAllGrassIsCut += GateSwitch;
    }

    void GateSwitch(int id)
    {
        Debug.Log(id);
        Debug.Log(this.transform.parent.transform.GetInstanceID());
        if (id == this.transform.parent.transform.GetInstanceID())
        {
            gameObject.SetActive(false);
        }

    }

}
