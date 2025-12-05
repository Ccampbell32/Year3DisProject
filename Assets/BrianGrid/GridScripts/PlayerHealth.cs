using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public bool IsDead => currentHealth <= 0;

    private TeamHealthManager teamManager;

    void Start()
    {
        currentHealth = maxHealth;
        teamManager = FindObjectOfType<TeamHealthManager>();
        teamManager.RegisterPlayer(this);
    }

    public void TakeDamage(int amount)
    {
        if (IsDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(name + " has died!");
        teamManager.NotifyPlayerDeath(this);

        // Optional: disable mesh, animations, etc.
        gameObject.SetActive(false);
    }
}

