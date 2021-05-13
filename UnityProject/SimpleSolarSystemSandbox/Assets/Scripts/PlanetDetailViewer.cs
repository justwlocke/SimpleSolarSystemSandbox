using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Globalization;
using UnityEngine.EventSystems;

public class PlanetDetailViewer : MonoBehaviour
{

    //The currently selected celestial body
    private CelestialBody selectedBody;

    [Header("All UI references")]
    [Tooltip("The Overarching Panel that holds all object")]
    public GameObject detailPanel;

    public Image planetImage;

    public TextMeshProUGUI planetName;

    [Tooltip("The panel containing the facts about the planet")]
    public GameObject factSheet;

    // Start is called before the first frame update
    void Start()
    {
        HideDetails();
    }

    // Update is called once per frame
    void Update()
    {
        //On left click
        if (Input.GetMouseButtonDown(0))
        {
            //Get the mouse's position
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Convert it to 2D
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            //Raycast to that point
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if(hit.collider != null)
            {
                Debug.Log("Hit: " + hit.collider.name);
                //Get the Celestial Body...
                CelestialBody body = hit.collider.GetComponent<CelestialBody>();
                if(body != null)
                {
                    //In order to access the detailed data about the planet
                    PlanetData data = body.data;
                    if(data != null)
                    {
                        //Show the data
                        ShowDetails(data);
                    }
                    else
                    {
                        Debug.Log("This body doesn't have any data attached to it");

                        //This is here because the spawned bodies don't have any data.
                        //Eventually they will have data about how they were generated, so this shouldn't be hit then.
                        //However, not 
                        HideDetails();
                    }


                    //Select the body even if there's no planetary details
                    SelectBody(body);
                   
                }
                else
                {
                    Debug.Log("Didn't hit a Celestial Body");
                    HideDetails();
                    //Unselect the previous body
                    UnSelectBody();
                }
            }
            else
            {
                //If the mouse is over UI...
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    //And that UI is the speed slider
                    if (EventSystem.current.currentSelectedGameObject.name.Equals("GameSpeedSlider"))
                    {
                        //Don't deselect the currently selected celestial body.
                    }
                    else
                    {
                        //We hit nothing important/interactive
                        Debug.Log("Hit: Nothing important/interactive: " + EventSystem.current.currentSelectedGameObject);
                        HideDetails();
                        //Unselect the previous body
                        UnSelectBody();
                    }

                }
                else
                {
                    //We hit nothing
                    //Debug.Log("Hit: Nothing");
                    HideDetails();
                    //Unselect the previous body
                    UnSelectBody();
                }
            }
        }


        //Debugging

        //If there is a body selected, resize it's gravty radius constantly, so I can see visually when testing
        if(selectedBody != null)
        {
            selectedBody.SetGravityRadiusDisplaySize();
        }


    }



    /// <summary>
    /// Shows data in a popu-p window about the selected Planet
    /// </summary>
    /// <param name="dataToShow">a reference to the data to show</param>
    private void ShowDetails(PlanetData dataToShow)
    {
        var thousandsSplitter = new NumberFormatInfo { NumberGroupSeparator = "," };


        //Set the name of the planet in the detail view
        planetName.text = dataToShow.planetName;
        //Set the detailed Image
        planetImage.sprite = dataToShow.planetImage;

        //Set the mass of the planet in the facts sheet
        factSheet.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Mass: " + dataToShow.planetMass + "kg";
        //Set the Gravity of the planet in the facts sheet
        factSheet.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Gravity: " + dataToShow.planetGravity + "m/s^2";
        //Set the distance of the planet to the sun in the facts sheet
        factSheet.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Distance to Sun: " + dataToShow.meanDistanceFromSun.ToString("n0", thousandsSplitter) + "km";
        //Set the amount of Earth days in the planets year in the facts sheet
        factSheet.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Earth days in year: " + dataToShow.orbitalPeriod.ToString("n0", thousandsSplitter);
        //Set the population of the planet in the facts sheet
        factSheet.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Population: " + dataToShow.population.ToString("n0", thousandsSplitter);

        //If it's not already visible, make it appear
        if (!detailPanel.activeSelf)
        {
            detailPanel.SetActive(true);
        }
    }

    /// <summary>
    /// Hides the details panel
    /// </summary>
    private void HideDetails()
    {
        //If it is visible, just turn it off
        if (detailPanel.activeSelf)
        {
            detailPanel.SetActive(false);
        }

    }

    /// <summary>
    /// Select the generic celestial body
    /// </summary>
    private void SelectBody(CelestialBody body)
    {
        //Change the color of the trail renderer anyways to show tha the body is "Selected"
        body.HighlightBody();

        //Turn the previously selected body's trail back to normal, if one was selected
        if (selectedBody != null) selectedBody.UnHighlightBody();

        //Update which celestial body is currently selected
        selectedBody = body;
    }


    /// <summary>
    /// Unselect the generic celestial body
    /// </summary>
    private void UnSelectBody()
    {
        //Turn the previously selected body's trail back to normal, if one was selected
        if (selectedBody != null) selectedBody.UnHighlightBody();
        //Reset the selected object
        selectedBody = null;
    }
}
