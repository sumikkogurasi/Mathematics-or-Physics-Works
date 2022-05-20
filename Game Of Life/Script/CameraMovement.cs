using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private Vector3 dragOrigin;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PanCamera();
    }

    private void PanCamera()
    {
        // save position of mouse in worlt space when drag starts (first time clicked)

        if (Input.GetMouseButtonDown(0))
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);

        // calculate distance between drag origin and new position if it is still held down
        if (Input.GetMouseButton(0))
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);

            print("origin" + dragOrigin + "newPosition" + cam.ScreenToWorldPoint(Input.mousePosition) + "=difference" + difference);

            // move the camera by that distance
            cam.transform.position += difference;
        }
        
    }
}
