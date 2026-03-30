using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] public GameObject settingsPanel;
    public PauseManager pauseManager;

    void Start()
    {
        settingsPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        if (SceneManager.GetActiveScene().name == "game")
        {
            Time.timeScale = 0f;
        }
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        if (SceneManager.GetActiveScene().name == "game")
        {
            Time.timeScale = 1f;
            pauseManager.Resume();
        }
    }
}

