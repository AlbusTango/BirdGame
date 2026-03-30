using UnityEngine;

public class platformSpawnScript : MonoBehaviour
{
    public GameObject[] platforms;
    private float timer;

    [Header("Spawn Timing")]
    public float currentSpawnTime = 3.75f;
    public float minSpawnTime = 1.5f;
    public float decreaseAmount = 0.05f;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spawnPlatform();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= currentSpawnTime)
        {
            spawnPlatform();
            SetNextSpawnTime();
        }
    }

    void SetNextSpawnTime()
    {
        timer = 0f;
        currentSpawnTime -= decreaseAmount;
        currentSpawnTime = Mathf.Max(currentSpawnTime, minSpawnTime);
    }

    void spawnPlatform()
    {
        // pick a random prefab from the array
        int randomIndex = Random.Range(0, platforms.Length);
        GameObject chosenPlatform = platforms[randomIndex];

        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y, 0);

        Instantiate(chosenPlatform, spawnPos, Quaternion.identity);
    }
}
