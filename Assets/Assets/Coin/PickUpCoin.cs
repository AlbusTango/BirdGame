using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    private AudioSource coinSound;
    private bool collected = false;

    void Awake()
    {
        coinSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;
        if (!other.CompareTag("Player")) return;

        collected = true;

        CoinManager coinManager = FindFirstObjectByType<CoinManager>();
        if (coinManager != null)
        {
            coinManager.AddCoin(1);
        }

        coinSound.Play();

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        Destroy(gameObject, coinSound.clip.length);
    }
}
