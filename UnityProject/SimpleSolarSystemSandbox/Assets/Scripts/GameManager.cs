using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Tooltip("Has the player pressed enter to start the simulation yet?")]
    public bool gameStarted = false;

    [Tooltip("The Speed controller")]
    public GameSpeedController speedControl;

    [Header("UI Elements")]
    [Tooltip("The panel with the starking text")]
    public RectTransform startPanel;
    [Tooltip("The panel for when the game is paused")]
    public RectTransform pausePanel;

    void Awake()
    {
        startPanel.gameObject.SetActive(true);
        //Set time to 0 to start so that nothing happens.
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Submit") && !gameStarted)
        {
            //Give the planets their initial push to establish orbits
            BeginSimulation();
            //Start to increase time
            StartCoroutine("SlowlySpeedUpTime");
        }


        //If the player presses escape to pause the game...
        if (Input.GetButtonDown("GameMenu") && gameStarted)
        {
            TogglePauseGame();
        }


        if(Input.GetButtonDown("Restart") && gameStarted)
        {
            RestartSimulation();
        }
    }


    private IEnumerator SlowlySpeedUpTime()
    {
        //While we want to increase it...
        while(Time.timeScale < 1)
        {
            Time.timeScale += 0.02f;
            yield return new WaitForSecondsRealtime(0.1f);
        }

        //Make sure the timeScale is equal to 1.
        Time.timeScale = 1;
        gameStarted = true;
    }


    private void BeginSimulation()
    {
        //Get all celestial bodies
        CelestialBody[] allBodies = GameObject.FindObjectsOfType<CelestialBody>();
        //For each of them...
        foreach(CelestialBody body in allBodies)
        {
            //Apply their starting force
            body.StartSim();
        }

        //Fade out the panel and any text it might have
        FadeOutPanelWithText();

    }


    private void FadeOutPanelWithText()
    {
        //Fade out this panel
        startPanel.GetComponent<Image>().CrossFadeAlpha(0.0f, 2f, true);
        //... and all of it's children
        foreach (Image img in startPanel.GetComponentsInChildren<Image>())
        {
            img.CrossFadeAlpha(0.0f, 2f, true);
        }
        //...And any text as well
        foreach(TextMeshProUGUI txt in startPanel.GetComponentsInChildren<TextMeshProUGUI>())
        {
            txt.CrossFadeAlpha(0.0f, 2f, true);
        }
        //Turn off the panel after it has faded out
        Invoke("DisablePanel", 4f);
    }

    //Turn off the panel
    private void DisablePanel()
    {
        startPanel.gameObject.SetActive(false);
    }



    /// <summary>
    /// Tells the speed controller to pause the game and shows the Simulation paused UI
    /// </summary>
    public bool TogglePauseGame()
    {
        //Toggle the game's state
        bool result = speedControl.ToggleGamePause();
        //And make the pause panel appear/disappear appropriately
        pausePanel.gameObject.SetActive(result);
        //Continue propagting the result to whoever called this function in case they need it
        return result;
    }


    public void RestartSimulation()
    {
        //Reload the scene
        SceneManager.LoadSceneAsync(0);
    }
}
