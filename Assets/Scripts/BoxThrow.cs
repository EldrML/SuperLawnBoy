 using UnityEngine;
 using System.Collections;
 
 public class BoxThrow : Box
 {
    //  Vector3 startPos = new Vector3(0, 0, 0);
    //  Vector3 endPos = new Vector3(0, 0, 10);
    //  float height = 4f;
    //  bool startThrow = false;
    //  float incrementor = 0;
 
     // Update is called once per frame
    //  void Update()
    //  {
    //      startPos   = player.transform.position + carryHeight;
    //      endPos     = throwLocation;

    //      if (startThrow)
    //      {
    //          incrementor += 0.04f;
    //          Vector3 currentPos = Vector3.Lerp(startPos, endPos, incrementor);
    //          currentPos.z += height * Mathf.Sin(Mathf.Clamp01(incrementor) * Mathf.PI);
    //          transform.position = currentPos;
    //      }
    //      if (transform.position == endPos)
    //      {
    //          startThrow = false;
    //          incrementor = 0;
    //          Vector3 tempPos = startPos;
    //          startPos = transform.position;
    //          endPos = tempPos;

    //         boxState = BoxStates.onGround;
    //         player.currentType = PlayerController.PlayerType.nm;
    //         sprite.sortingLayerName = "Interactive";
    //      }
    //      if (boxState == BoxStates.isThrown)
    //      {
    //          startThrow = true;
    //      }
    //  }
 }