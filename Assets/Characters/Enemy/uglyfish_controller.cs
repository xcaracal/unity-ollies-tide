using System.Collections;
using UnityEngine;
using CodeMonkey.Utils;
using UnityEngine.UI;

public class UglyFishController : MonoBehaviour
{
    public Animator uglyFishAnimator;
    public float moveSpeed = 3f;
    public int framesInIdle = 60;
    public float maxHealth = 100f; // Maximum health of the ugly fish
    public SpriteRenderer healthBarSprite; // Reference to the health bar sprite renderer

    private float currentHealth; // Current health of the ugly fish

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();

        // Explicitly set the initial state to idle
        uglyFishAnimator.Play("uglyfish_idle");
        StartCoroutine(AnimateUglyFish());
    }

    IEnumerator AnimateUglyFish()
    {
        while (true)
        {
            // Wait for the idle animation to finish
            yield return new WaitForSeconds(framesInIdle * Time.deltaTime);

            // Running animation and move to the left (opposite direction)
            uglyFishAnimator.SetTrigger("uglyfish_walk");
            float moveDuration = 2f;
            float startTime = Time.time;
            while (Time.time < startTime + moveDuration)
            {
                transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
                yield return null;
            }

            // Stop moving
            uglyFishAnimator.SetTrigger("uglyfish_idle");

            // Simulate taking damage
            TakeDamage(10);

            // Attack animation
            uglyFishAnimator.SetTrigger("uglyfish_attack");
            float attackDuration = 1f;
            yield return new WaitForSeconds(attackDuration);

            // Return to original position
            uglyFishAnimator.SetTrigger("uglyfish_walk");
            startTime = Time.time;
            while (Time.time < startTime + moveDuration)
            {
                transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
                yield return null;
            }

            // Back to idle and repeat
            // Set to idle state explicitly
            uglyFishAnimator.Play("uglyfish_idle");

            // Wait for the idle animation to finish
            yield return new WaitForSeconds(framesInIdle * Time.deltaTime);
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    void TakeDamage(float damageAmount)
    {
        int damage = Mathf.RoundToInt(damageAmount);
        DamagePopup.Create(GetPosition(), damage, false, 0);

        // Reduce health
        currentHealth -= damageAmount;

        // Update health bar
        UpdateHealthBar();

        // Check if ugly fish is defeated
        if (currentHealth <= 0)
        {
            // Optional: Handle ugly fish defeat logic (e.g., play a defeat animation, respawn, etc.)
            ResetHealth();
        }
    }

    void UpdateHealthBar()
    {
        // Update the health bar scale based on the current health percentage
        float healthPercentage = currentHealth / maxHealth;
        healthBarSprite.transform.localScale = new Vector3(healthPercentage, 1f, 1f);
    }

    void ResetHealth()
    {
        // Reset health to max when defeated
        currentHealth = maxHealth;
        UpdateHealthBar();
    }
}
