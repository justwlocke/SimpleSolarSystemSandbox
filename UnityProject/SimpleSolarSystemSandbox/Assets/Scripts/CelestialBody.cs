using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(TrailRenderer))]
public class CelestialBody : MonoBehaviour
{

    [Header("Simulation Data")]
    [Tooltip("The amount of force this body will attract others towards itself with...")]
    public float gravityPower = 9.8f;
    [Tooltip("Over this distance, decreasing as it gets further away.")]
    public float gravityRadius = 10;
    [Tooltip("Will not be affected by other bodies' gravity. Typically only on the sun.")]
    public bool staticBody = false;
    [Tooltip("How often to update the length of the bodies trail based on its speed.")]
    public float trailUpdateTimer = 10;

    [Tooltip("The Vector that is given to the body when it starts moving at the beginning of the simulation")]
    public Vector2 StartingForce = Vector2.zero;

    [Header("Detailed Data")]
    [Tooltip("The scriptable object that holds the detailed data for this body.")]
    public PlanetData data;



    //Has the sim started?
    private bool simActive = false;

    // Start is called before the first frame update
    void Start()
    {
        //Ensure this object is on the CelestialBody layer
        this.gameObject.layer = LayerMask.NameToLayer("CelestialBody");

        //Change the radius of this body's gravitational influence based on the public variable
        SetGravityRadiusDisplaySize();

        
    }

    private void FixedUpdate()
    {
        //Only exert gravity once the sim has started, or while not paused
        if (simActive)
        {
            //Only interplolate if the timescale is very slow
            if (Time.timeScale < 0.4)
            {
                this.GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.Interpolate;
            }
            else
            {
                this.GetComponent<Rigidbody2D>().interpolation = RigidbodyInterpolation2D.None;
            }
            ExertGravity();
        }
    }


    /// <summary>
    /// Exert gravity on nearby game objects
    /// </summary>
    private void ExertGravity()
    {
        //Get all celestial bodies within the range of this body
        Collider2D[] nearbyBodies = Physics2D.OverlapCircleAll(this.transform.position, gravityRadius, 1<<8);
        //for each of them...
        foreach(Collider2D body in nearbyBodies)
        {
            //If this body is ourself...
            if (body.gameObject == this.gameObject)
            {
                //Don't try to gravitate to yourself!
            }
            //If the body is not static, then add a force to it
            else if (!body.GetComponent<CelestialBody>().staticBody)
            {
                /*
                if (!this.gameObject.name.Equals("Sun"))
                {
                    Debug.Log(body.gameObject.name + " is near " + this.gameObject.name);
                }
                */
                //Get the directional vector
                Vector2 exudedForce = (this.transform.position - body.transform.position);
                //Normalize it.
                exudedForce = exudedForce.normalized;
                //Debug.Log(this.name + " is exuding " + exudedForce + " normalized force on " + body.name);
                //Calculate the real force by multiplying in the power, divided by the distance
                exudedForce = exudedForce * (gravityPower / Vector2.Distance(this.transform.position, body.transform.position));

                //We can assume this because every GameObject with this script, is set to the correct layer and requires a rigidbody2D
                body.GetComponent<Rigidbody2D>().AddForce(exudedForce);

            }
            else
            {
                //It is static, and so should not move
                //Really, only the sun uses this.
            }
        }
    }

    /// <summary>
    /// Begin the simulation of this object
    /// </summary>
    public void StartSim()
    {
        this.GetComponent<Rigidbody2D>().AddForce(StartingForce);
        simActive = true;

        InvokeRepeating("CalculateAndSetTrailLength", 0, trailUpdateTimer);
    }



    /// <summary>
    /// Changes the color of the trail renderer in order to highlight it.
    /// Also makes the planet's influence circle be visible
    /// </summary>
    public void HighlightBody()
    {
        //Debug.Log("Highlighting trail");
        TrailRenderer tr = this.gameObject.GetComponent<TrailRenderer>();
        tr.startColor = Color.green;
        tr.endColor = Color.green;

        //Make the bodys gravity circle visible
        this.transform.GetChild(0).gameObject.SetActive(true);
    }


    /// <summary>
    /// Changes the color of the trail renderer in order to unhighlight it.
    /// Also makes the planet's influence circle be invisible
    /// </summary>
    public void UnHighlightBody()
    {
        //Debug.Log("UnHighlighting trail");
        TrailRenderer tr = this.gameObject.GetComponent<TrailRenderer>();
        tr.startColor = Color.white;
        tr.endColor = Color.white;

        //Make the bodies gravity circle invisible
        this.transform.GetChild(0).gameObject.SetActive(false);

    }


    /// <summary>
    /// Calculate and set the length of the trail renderer based on how fast the body is moving, and the distance from the sun
    /// Faster objects should have a smaller value, objects further away should have a larger one
    /// </summary>
    private void CalculateAndSetTrailLength()
    {
        TrailRenderer tr = this.gameObject.GetComponent<TrailRenderer>();
        //Invert the speed 
        float calculatedTime = 100 * (1 / this.GetComponent<Rigidbody2D>().velocity.magnitude);
        //The distance to the sun, but then divided by the proportion of how much the objects positions were multiplied in the scene.
        //AKA mercury and venus should be less than 1, Earth should be exactly 1, and everything else more.
        float relativeDistance = (Vector2.Distance(Vector2.zero, this.gameObject.transform.position) / 10);

        //Set the length
        tr.time = calculatedTime * relativeDistance;
    }



    public void SetGravityRadiusDisplaySize()
    {
        //Change the radius of this body's gravitational influence based on the public variable
        Vector3 newDesiredScale = new Vector3(gravityRadius, gravityRadius, gravityRadius) * 2;
        //Debug.Log(newDesiredScale);
        SetGlobalScale(this.transform.GetChild(0), newDesiredScale);
    }


    /// <summary>
    /// Changes loval scale to global scale and sets it on the object
    /// </summary>
    /// <param name="transform">The object to resize</param>
    /// <param name="globalScale">The desired global scale</param>
    private void SetGlobalScale(Transform transform, Vector3 globalScale)
    {
        transform.localScale = Vector3.one;
        transform.localScale = new Vector3(globalScale.x / transform.lossyScale.x, globalScale.y / transform.lossyScale.y, globalScale.z / transform.lossyScale.z);
    }
}
