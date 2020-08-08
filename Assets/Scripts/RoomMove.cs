using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomMove : MonoBehaviour
{
    public Vector2 cameraMinChange;
    public Vector2 cameraMaxChange;
    public Vector3 playerChange;
    private CameraMovement cam;
    //public GameObject player;
    public PlayerController playerController;
    public bool needText; //Boolean to determine if title card is needed
    public string placeName;
    public GameObject text;
    public Text placeText;
    
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.GetComponent<CameraMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D otherObj)
    {
        if(otherObj.CompareTag("Player"))
        {
            cam.minPosition += cameraMinChange;
            cam.maxPosition += cameraMaxChange;
            otherObj.transform.position += playerChange;
            otherObj.transform.position = SnapPosition(otherObj.transform.position, 0.5f);
            playerController.movePoint.position = otherObj.transform.position;

            if(needText)
            {
                StartCoroutine(placeNameCo());
            }
        }
    }

    private IEnumerator placeNameCo() //place name coroutine
    {
        text.SetActive(true);
        placeText.text = placeName;
        yield return new WaitForSeconds(4f);
        text.SetActive(false);
    }

    private Vector3 SnapPosition(Vector3 input, float factor = 1f)
    {
        if (factor <= 0f)
            throw new UnityException("factor argument must be above 0");

        float x = Mathf.Round(input.x / factor) * factor;
        float y = Mathf.Round(input.y / factor) * factor;
        float z = Mathf.Round(input.z / factor) * factor;

    return new Vector3(x,y,z);
    }
}
