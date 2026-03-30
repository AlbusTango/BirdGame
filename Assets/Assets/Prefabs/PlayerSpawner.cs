using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    public Transform spawnPoint;

    void Start()
    {
        int index = PlayerPrefs.GetInt("SelectedCharacter", 0);
        index = Mathf.Clamp(index, 0, characterPrefabs.Length - 1);

        GameObject prefab = characterPrefabs[index];

        GameObject player = Instantiate(characterPrefabs[index], spawnPoint.position, prefab.transform.rotation);

        player.transform.localScale = prefab.transform.localScale;
    }
}
