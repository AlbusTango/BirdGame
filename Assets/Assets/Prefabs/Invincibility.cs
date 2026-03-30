using System.Collections;
using UnityEngine;

public class Invincibility : MonoBehaviour
{
    [Header("Invincibility")]
    [SerializeField] private float invincibilityDuration = 1.5f;

    private bool isInvincible;
    private Animator animator;

    void Awake()
    {
        // Animator is on the Visuals child
        animator = GetComponentInChildren<Animator>();
    }

    public bool IsInvincible()
    {
        return isInvincible;
    }

    // Call this when the player is hit
    public void Trigger()
    {
        if (isInvincible) return;
        StartCoroutine(InvincibilityRoutine());
    }

    private IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;

        // Start blinking (Animator override layer)
        animator.SetBool("IsInvincible", true);

        yield return new WaitForSeconds(invincibilityDuration);

        // Stop blinking
        animator.SetBool("IsInvincible", false);
        isInvincible = false;
    }
}
