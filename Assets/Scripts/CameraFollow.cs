using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform LawnBoy;

    void FixedUpdate ()
    {
        transform.position = new Vector3(LawnBoy.position.x, LawnBoy.position.y, transform.position.z);
    }
}
