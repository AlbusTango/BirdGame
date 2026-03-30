using UnityEngine;

public class hitScript : MonoBehaviour
{

    private AudioSource hitSound;

    void Awake()
    {
        hitSound = GetComponent<AudioSource>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        Invincibility inv = other.GetComponent<Invincibility>();
        if (inv != null && inv.IsInvincible()) return;

        FindFirstObjectByType<LogicManager>().LoseLife();

        if (hitSound != null)
            hitSound.Play();

        if (inv != null)
            inv.Trigger();
    }
}
