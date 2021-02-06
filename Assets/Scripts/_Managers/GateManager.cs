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

    void GateSwitch()
    {
        gameObject.SetActive(false);
    }

}
