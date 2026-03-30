using System.Numerics;
using UnityEngine;

public class moveScript : MonoBehaviour
{
    public float lowRemoveZone = -30f;
    public float highRemoveZone = 30f;
    private float spikeSpeed = 2f;

    void Update()
    {
        float speed = SpeedManager.moveSpeed;
        transform.Translate(UnityEngine.Vector3.down * speed * Time.deltaTime);

        foreach (Transform child in transform)
        {
            if (child.CompareTag("Balloon"))
            {
                child.Translate(UnityEngine.Vector3.up * speed * 2 * Time.deltaTime);
            }

            if (child.CompareTag("FlyingSpike"))
            {
                child.Translate(UnityEngine.Vector3.down * speed * spikeSpeed * Time.deltaTime);
            }

            if (transform.position.y < lowRemoveZone || transform.position.y > highRemoveZone)
            {
                Destroy(gameObject);
            }

        }
    }
}
