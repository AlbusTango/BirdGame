using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public GameObject settingsPanel;
    public Button pauseButton;

    private bool isPaused;

    void Start()
    {
        Resume();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (isPaused)
            Resume();
        else
            Pause();
    }

    public void Pause()
    {
        settingsPanel.SetActive(true);
        pauseButton.interactable = false; // button visible but unusable
        Time.timeScale = 0f;
        isPaused = true;

        FindFirstObjectByType<GravityController_Touch>()?.DisableControls();

    }

    public void Resume()
    {
        settingsPanel.SetActive(false);
        pauseButton.interactable = true; // usable again
        Time.timeScale = 1f;
        isPaused = false;

        FindFirstObjectByType<GravityController_Touch>()?.EnableControls();
    }
}