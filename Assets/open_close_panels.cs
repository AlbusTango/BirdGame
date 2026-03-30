using UnityEngine;

public class SettingsUI : MonoBehaviour
{
    [Header("Panels")]
    public GameObject howToPlayPanel;
    public GameObject closePanel;

    void Start()
    {
        howToPlayPanel.SetActive(false);
        closePanel.SetActive(false);
    }

    // When HowToPlay button is clicked
    public void openHowToPlay()
    {
        howToPlayPanel.SetActive(true);
        closePanel.SetActive(false);
    }

    // When Close button is clicked
    public void openClosePanel()
    {
        howToPlayPanel.SetActive(false);
        closePanel.SetActive(true);
    }

    // Close Panels
    public void BackToMenu()
    {
        closePanel.SetActive(false);
        howToPlayPanel.SetActive(false);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
