using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSpeedController : MonoBehaviour
{
    // Start is called before the first frame update
    [Tooltip("The UI element for controlling time")]
    public Slider speedSlider;
    [Tooltip("The text on the slider")]
    public TextMeshProUGUI currentSpeedText;


    public bool isGamePaused = false;

    private GameManager manager;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //If the simulation has started, only then allow the slider to affect time
        if (manager.gameStarted)
        {
            //If the game is not paused, update the timeScale based on the slider
            if (!isGamePaused)
            {
                //Round the value of the slider to 2 decimal places
                float currentSpeed = Mathf.Round(speedSlider.value * 100) / 100f;
                //Set the timescale
                Time.timeScale = currentSpeed;
                //Set the label
                currentSpeedText.text = currentSpeed + "x";
            }
            else
            {
                //If it is paused, don't update the timeScale
            }
        }
    }


    /// <summary>
    /// Switches between being paused and not being paused
    /// </summary>
    /// <returns>The resulting state</returns>
    public bool ToggleGamePause()
    {
        //If the game is paused...
        if (isGamePaused)
        {
            //Unpause it
            UnPauseSim();
        }
        else
        {
            //Else, do pause it
            PauseSim();
        }

        return isGamePaused;
    }


    //The timescale before the game was paused
    private float timeBeforePause = 1;
    /// <summary>
    /// Pause the simulation
    /// </summary>
    private void PauseSim()
    {
        timeBeforePause = Time.timeScale;
        Time.timeScale = 0;
        //Set the flag
        isGamePaused = true;
    }



    /// <summary>
    /// Unpause the Simulation
    /// </summary>
    private void UnPauseSim()
    {
        Time.timeScale = timeBeforePause;
        //Set the flag
        isGamePaused = false;
    }
}
