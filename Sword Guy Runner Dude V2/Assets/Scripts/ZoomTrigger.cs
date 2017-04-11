using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomTrigger : MonoBehaviour {

    public CameraController cameraController;
	
    public float firstZoom = 6.396629f;
    public float lastZoom = 10f;

    void OnTriggerEnter2D(Collider2D coll)
    {
        cameraController.Zoom(firstZoom, lastZoom);

    }
}
