using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetSpawner : MonoBehaviour
{
    // Start is called before the first frame update

    [Tooltip("The Sun")]
    public GameObject theSun;
    [Tooltip("All Sol Planet Prefabs in order")]
    public GameObject[] planets = new GameObject[10];


    [Tooltip("The Game Manager")]
    public GameManager gm;

    [Header("UI Elements")]

    [Tooltip("The inner panel of the planet picker UI")]
    public GameObject innerUIPanel;
    [Tooltip("The Button that opens the picker UI")]
    public GameObject openPickerButton;


    //Which body is currently selected in the planet picker, which we will spawn on right click
    private GameObject bodyToSpawn;
    //References to a planet we've recently spawned
    private GameObject spawnedPlanet = null;
    private CelestialBody body = null;

    private Vector2 spawnForce = new Vector2(0, 0);


    //The line renderer that draws a straight line from the location of the click to the planet
    private LineRenderer lineRenderer;

    [Tooltip("How much of the arrow is the actual pointing part")]
    public float percentArrowHead = 0.4f;

    void Start()
    {
        //Setup the line renderer
        lineRenderer = this.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 4;

        //Set the default body to spawn
        bodyToSpawn = planets[9];
    }

    // Update is called once per frame
    void Update()
    {
        //Might want to prevent spawning when paused, might not.s


        //On right click
        if (Input.GetMouseButtonDown(1))
        {
            //Compensate for the Z axis
            Vector3 spawnPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            spawnPos = new Vector3(spawnPos.x, spawnPos.y, 0);

            //Set one end of the line renderer
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(3, spawnPos);

            //Spawn the new body where the mouse is
            spawnedPlanet = GameObject.Instantiate(bodyToSpawn, spawnPos, Quaternion.identity);
            body = spawnedPlanet.GetComponent<CelestialBody>();
            //Make it static so that it doesn't move.
            body.staticBody = true;
        }
        //While holding down right click...
        else if (Input.GetMouseButton(1))
        {
            
            //Compensate for the Z axis
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Get the force to move them after letting go by calcing the distance between where it was spawned and where the mouse is.
            spawnForce = spawnedPlanet.transform.position - new Vector3(mousePos.x, mousePos.y, 0);

            //Extra spawning default force
            spawnForce *= 10;

            //Add extra force based on the distance from the sun
            spawnForce = spawnForce * Vector2.Distance(spawnedPlanet.transform.position, theSun.transform.position);


            //Set one end of the line renderer
            lineRenderer.widthCurve = new AnimationCurve(
                new Keyframe(0, 0.4f)
                , new Keyframe(0.999f - percentArrowHead, 0.4f)  // neck of arrow
                , new Keyframe(1 - percentArrowHead, 1f)  // max width of arrow head
                , new Keyframe(1, 0f));  // tip of arrow
            lineRenderer.SetPosition(1, Vector3.Lerp(mousePos, spawnedPlanet.transform.position, 0.999f - percentArrowHead));
            lineRenderer.SetPosition(2, Vector3.Lerp(mousePos, spawnedPlanet.transform.position, 1 - percentArrowHead));
            lineRenderer.SetPosition(0, mousePos);
        }
        //When they let go...
        else if (Input.GetMouseButtonUp(1))
        {
            body.staticBody = false;
            //Give them a push
            spawnedPlanet.GetComponent<CelestialBody>().StartingForce = spawnForce;
            spawnedPlanet.GetComponent<CelestialBody>().StartSim();

            //Remove the references
            body = null;
            spawnedPlanet = null;


            lineRenderer.enabled = false;
        }
    }

    //=============================================== UI Functions ==================================================

    /// <summary>
    /// Called by the UI buttons to choose what to spawn
    /// </summary>
    /// <param name="index">The index of the planet to spawn</param>
    public void PickPlanet(int index)
    {
        Debug.Log("Picked Planet at index: " + index + " which is " + planets[index].name);
        //Get the prefab of the planet to spawn
        bodyToSpawn = planets[index];
        //Change the UI to highlight which we picked
        innerUIPanel.transform.GetChild(index).GetComponent<Image>().color = new Color(0, 1, 0, .75f);
        //Reset the color of every UI except the one we just changed
        for(int x = 0; x < planets.Length; x++)
        {
            if(x == index)
            {
                //Do nothing
            }
            else
            {
                //Temp skip while we don't have all the UI in place
                if(innerUIPanel.transform.GetChild(x) == null)
                {
                    break;
                }
                //Reset the color of every other selection radius because I don't want to store another "previously picked" thing to go straight there
                innerUIPanel.transform.GetChild(x).GetComponent<Image>().color = new Color(1, 1, 1, .33f);
            }
        }

    }



    /// <summary>
    /// Open the planet Picker
    /// </summary>
    public void OpenPicker()
    {
        innerUIPanel.transform.parent.gameObject.SetActive(true);
        //Hide the open button
        openPickerButton.SetActive(false);
        //Pause the sim while the picker is open
        gm.TogglePauseGame();
    }

    /// <summary>
    /// Close the planet picker
    /// </summary>
    public void ClosePicker()
    {
        innerUIPanel.transform.parent.gameObject.SetActive(false);
        //Show the open button
        openPickerButton.SetActive(true);
        //Pause the sim while the picker is open
        gm.TogglePauseGame();
    }


    
}
