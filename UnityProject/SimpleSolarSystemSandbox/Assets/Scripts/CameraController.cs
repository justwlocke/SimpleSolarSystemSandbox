using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [Header("Camera Speed")]
    public float camSpeed = 10f;

    [Header("Camera movement bounds")]
    [Tooltip("The max x the camera can move to in both +/-")]
    public float xBounds = 200;
    [Tooltip("The max y the camera can move to in both +/-")]
    public float yBounds = 200;

    [Header("Camera Zoom")]
    public float minZoom = 5;
    public float maxZoom = 50;

    [Header("Script Refs")]
    public DashedLineResizer measurementLine;

    //Holder variables
    private float vertExtent;
    private float horzExtent;

    private float minXBounds;
    private float maxXBounds;
    private float minYBounds;
    private float maxYBounds;

    // Start is called before the first frame update
    void Start()
    {
        UpdateExtents();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 cameraMovement = Vector2.zero;

        cameraMovement.x = Input.GetAxis("Horizontal");
        cameraMovement.y = Input.GetAxis("Vertical");

        //Adjust the speed of the movement
        cameraMovement = (cameraMovement * camSpeed * Time.deltaTime);
        //Apply it to the camera
        //this.gameObject.GetComponent<Rigidbody2D>().AddForce(cameraMovement);
        this.transform.Translate(cameraMovement);


        Vector3 clampPosition = transform.position;
        clampPosition.x = Mathf.Clamp(clampPosition.x, minXBounds, maxXBounds);
        clampPosition.y = Mathf.Clamp(clampPosition.y, minYBounds, maxYBounds);
        transform.position = clampPosition;

        //For zooming in....
        if (Input.mouseScrollDelta.y > 0)
        {
            if (Camera.main.orthographicSize - Input.mouseScrollDelta.y < minZoom)
            {
                //Don't do anything
            }
            else
            {
                Camera.main.orthographicSize += -Input.mouseScrollDelta.y;
                UpdateExtents();
            }
        }
        //...and for zooming out
        else if (Input.mouseScrollDelta.y < 0)
        {
            if (Camera.main.orthographicSize - Input.mouseScrollDelta.y > maxZoom)
            {
                //Als don't follow through
            }
            else
            {
                Camera.main.orthographicSize += -Input.mouseScrollDelta.y;

                //And also when zooming out...
                UpdateExtents();

                clampPosition = transform.position;
                clampPosition.x = Mathf.Clamp(clampPosition.x, minXBounds, maxXBounds);
                clampPosition.y = Mathf.Clamp(clampPosition.y, minYBounds, maxYBounds);
                transform.position = clampPosition;
            }
        }
    }


    //Update how far the camera can move based on the size of the camera and screen
    private void UpdateExtents()
    {
        //Resize the dashed measurement line
        measurementLine.Resize(Camera.main.orthographicSize);

        vertExtent = Camera.main.orthographicSize;
        horzExtent = vertExtent * Screen.width / Screen.height;

        // Calculations assume map is position at the origin
        minXBounds = horzExtent - 1000 / 2.0f;
        maxXBounds = 1000 / 2.0f - horzExtent;
        minYBounds = vertExtent - 1000 / 2.0f;
        maxYBounds = 1000 / 2.0f - vertExtent;
    }
}
