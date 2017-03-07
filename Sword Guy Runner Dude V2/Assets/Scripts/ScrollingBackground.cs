using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour {

    public Transform backgroundImageBack;
    public Transform backgroundImageCenter;
    public Transform backgroundImageFront;

    public float backgroundSize;
    public float parallaxSpeed;     //speed of parallax background
    

    private Transform cameraTransform;
    private Transform[] layers;
    private float viewZone = 10f;             //make view zone value smaller if the background repositions too quickly for the camera
    private int leftIndex;
    private int rightIndex;
    private float lastCameraX;          //to track camera x position for parallax
    private const float backgroundZOffset = 2f;    //to keep background at the background

    private void Start()
    {
        backgroundSize = backgroundImageFront.position.x - backgroundImageCenter.position.x;    //setting consistent value of backgroundsize

        cameraTransform = Camera.main.transform;        //getting transform component of main camera
        lastCameraX = cameraTransform.position.x;
        layers = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            layers[i] = transform.GetChild(i);
        }

        leftIndex = 0;
        rightIndex = layers.Length - 1;
    }

    private void Update()
    {
        float deltaX = cameraTransform.position.x - lastCameraX;        //updating background to have a position relative to camera position - lastcamerax
        transform.position += Vector3.right * deltaX * parallaxSpeed;
        lastCameraX = cameraTransform.position.x;

        if (cameraTransform.position.x < (layers[leftIndex].transform.position.x + viewZone))       //automatic repositioning of layers based on camera position
        {
            ScrollLeft();
        }

        if (cameraTransform.position.x > (layers[rightIndex].transform.position.x - viewZone))
        {
            ScrollRight();
        }
    }

    private void ScrollLeft()   //to be called if camera scrolls too far to the left side
    {
        int lastRight = rightIndex;
        layers[rightIndex].position = Vector3.right * (layers[leftIndex].position.x - backgroundSize);
        leftIndex = rightIndex;
        rightIndex--;
        if (rightIndex < 0)
        {
            rightIndex = layers.Length - 1;
        }
       
    }

    private void ScrollRight()  //to be called if camera scrolls too far to the right side
    {
        int lastLeft = leftIndex;
        layers[leftIndex].position = Vector3.right * (layers[rightIndex].position.x + backgroundSize);
        rightIndex = leftIndex;
        leftIndex++;
        if (leftIndex == layers.Length)
        {
            leftIndex = 0;
        }
        
    }
}
