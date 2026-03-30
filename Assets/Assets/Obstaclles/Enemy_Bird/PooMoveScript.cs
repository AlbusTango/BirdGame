using UnityEngine;

public class PooMoveScript : MonoBehaviour
{
    public float fallSpeed = 6f;
    public float removeY = -6f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(UnityEngine.Vector3.down * fallSpeed * Time.deltaTime);

        if (transform.position.y < removeY)
        {
            Destroy(gameObject);
        }
    }
}
