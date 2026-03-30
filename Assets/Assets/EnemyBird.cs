using Unity.VisualScripting;
using UnityEngine;

public class EnemyBird : MonoBehaviour
{

    [Header("Move Settings")]
    public float speed = 2f;
    public bool moveLeft = true;

    [Header("Poo Settings")]
    public GameObject Poo;
    public float minPooSpawn = 0.1f;
    public float maxPooSpawn = 2.5f;

    private Vector3 originalScale;
    private bool hasPooped = false;
    private float pooTime;


    void Start()
    {
        originalScale = transform.localScale;

        float xScale = moveLeft ? Mathf.Abs(originalScale.x) : -Mathf.Abs(originalScale.x);
        transform.localScale = new Vector3(
        xScale,
        originalScale.y,
        originalScale.z
        );

        pooTime = Time.time + Random.Range(minPooSpawn, maxPooSpawn);

        Debug.Log("Bird will poop at: " + pooTime);
    }

    void Update()
    {
        move();
        PooTimer();

        if (transform.position.x < -20 || transform.position.x > 20)
        {
            Destroy(gameObject);
        }
    }

    void move()
    {
        UnityEngine.Vector3 dir = moveLeft ? UnityEngine.Vector3.left : UnityEngine.Vector3.right;
        transform.Translate(dir * speed * Time.deltaTime);
    }

    void DropPoo()
    {
        if (Poo != null)
        {
            Instantiate(Poo, transform.position + UnityEngine.Vector3.down * 0.5f, Quaternion.identity);
        }
    }

    void PooTimer()
    {
        if (!hasPooped && Time.time >= pooTime)
        {
            DropPoo();
            hasPooped = true;
        }
    }
}
