using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class RoomMove : MonoBehaviour
{
    [SerializeField] Tilemap tilemap;
    [SerializeField] Vector3 bottomLeft, topRight;
    public Vector2 cameraMinChange;
    public Vector2 cameraMaxChange;
    public Vector3 playerChange;
    private CameraMovement cam;
    //public GameObject player;
    //public PlayerController playerController;
    // public bool needText; //Boolean to determine if title card is needed
    // public string placeName;
    // public GameObject text;
    // public Text placeText;

    // Start is called before the first frame update
    private void Start()
    {
        tilemap = transform.parent.GetComponent<Tilemap>();
        bottomLeft = transform.parent.parent.position;
        topRight = bottomLeft + tilemap.size;
        //#TODO: send an event here to the camera edge controller. Set the initial camera limits to be the size of the room. Camera Tile Limits are: 12.5 X, 11.25 Y

    }

    void OnBecameVisible()
    {
        cam = Camera.main.GetComponent<CameraMovement>();
    }

    void OnBecameInvisible()
    {
        cam = null;
    }

    private void OnTriggerEnter2D(Collider2D otherObj)
    {
        if (otherObj.CompareTag("Player"))
        {
            //#TODO: Finish updating this: send an event to the camera controller to update camera limits
            //#TODO: Send event to the player to snap them over in the direction of the look.
            
            // cam.minPosition += cameraMinChange;
            // cam.maxPosition += cameraMaxChange;
            // otherObj.transform.position += playerChange;
            otherObj.transform.position = SnapPosition(otherObj.transform.position, 0.5f);
            // playerController.movePoint.position = otherObj.transform.position;

            // if(needText)
            // {
            //     StartCoroutine(placeNameCo());
            // }
        }
    }

    // private IEnumerator placeNameCo() //place name coroutine
    // {
    //     text.SetActive(true);
    //     placeText.text = placeName;
    //     yield return new WaitForSeconds(4f);
    //     text.SetActive(false);
    // }

    private Vector3 SnapPosition(Vector3 input, float factor = 1f)
    {
        if (factor <= 0f)
            throw new UnityException("factor argument must be above 0");

        float x = Mathf.Round(input.x / factor) * factor;
        float y = Mathf.Round(input.y / factor) * factor;
        float z = Mathf.Round(input.z / factor) * factor;

        return new Vector3(x, y, z);
    }
}