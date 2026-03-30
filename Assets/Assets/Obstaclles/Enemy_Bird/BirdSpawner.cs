using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    public float spawnArea = 7.5f;
    public GameObject BirdPrefab;

    [Header("Spawn Sides")]

    public float left = -10f;
    public float right = 10f;

    public float minspawnDelay = 10f;
    public float maxspawnDelay = 30f;

    public float delay = 30f;

    private float nextSpawn;

    void Start()
    {
        nextSpawn = Time.time + delay;
    }

    void Update()
    {
        if (Time.time >= nextSpawn)
        {
            SpawnBird();
            nextSpawnTime();
        }
    }

    void SpawnBird()
    {
        bool spawnLeft = Random.value < 0.5f;
        float x = spawnLeft ? left : right;

        GameObject bird = Instantiate(BirdPrefab, new Vector3(x, spawnArea, 0f), Quaternion.identity);

        EnemyBird birdScript = bird.GetComponent<EnemyBird>();
        birdScript.moveLeft = !spawnLeft;
    }

    void nextSpawnTime()
    {
        nextSpawn = Time.time + Random.Range(minspawnDelay, maxspawnDelay);
    }
}
