using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    public static float moveSpeed;

    [Header("SpeedSettings")]

    public float startSpeed = 4f;
    public float endSpeed = 10f;
    public float increaseRate = 0.05f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveSpeed = startSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        moveSpeed += increaseRate * Time.deltaTime;
        moveSpeed = Mathf.Min(moveSpeed, endSpeed);
    }
}
