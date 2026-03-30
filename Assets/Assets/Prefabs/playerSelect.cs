using UnityEngine;
using UnityEngine.SceneManagement;

public class playerSelect : MonoBehaviour
{
    public void characterSelect(int characterIndex)
    {
        PlayerPrefs.SetInt("SelectedCharacter", characterIndex);
        PlayerPrefs.Save();

        SceneManager.LoadScene("mainMenu");
    }
}
