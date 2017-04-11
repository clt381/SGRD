using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftTrigger : MonoBehaviour {

    public CameraController cameraController;

    public Vector3 firstPos = new Vector3 (0,0,0);
    public Vector3 lastPos;

    void OnTriggerEnter2D(Collider2D coll)
    {
        cameraController.Shift(firstPos, lastPos);
    }
}
