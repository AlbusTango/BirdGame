using UnityEngine;

public class playerScript : MonoBehaviour
{
    public Rigidbody2D therigidbody;

    [Header("Fall Limits")]
    public float leftFallThreshold = -3f;
    public float rightFallThreshold = 3f;
    public float yThreshold = -6f;

    private LogicManager logic;

    void Start()
    {
        therigidbody = GetComponent<Rigidbody2D>();
        logic = FindFirstObjectByType<LogicManager>();
    }

    void Update()
    {
        Vector2 pos = transform.position;

        if (pos.x < leftFallThreshold ||
            pos.x > rightFallThreshold ||
            pos.y < yThreshold)
        {
            logic.GameOver();
        }
    }
}