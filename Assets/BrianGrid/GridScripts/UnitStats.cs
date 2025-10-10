using UnityEngine;

public class UnitStats : MonoBehaviour
{
    public string unitName = "Unit";
    public int maxHealth = 10;
    public int currentHealth;
    public int attackDamage = 3;
    public int attackRange = 1;

    private Renderer rend;
    private Color baseColor;

    void Awake()
    {
        currentHealth = maxHealth;
        rend = GetComponent<Renderer>();
        if (rend != null)
            baseColor = rend.material.color;
    }

    public bool IsAlive => currentHealth > 0;

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"{unitName} took {amount} damage! ({currentHealth}/{maxHealth})");

        // Quick visual feedback
        if (rend != null)
            rend.material.color = Color.red;

        // Small delay then reset color
        Invoke(nameof(ResetColor), 0.3f);

        if (currentHealth <= 0)
            Die();
    }

    void ResetColor()
    {
        if (rend != null)
            rend.material.color = baseColor;
    }

    void Die()
    {
        Debug.Log($"{unitName} died!");
        gameObject.SetActive(false);
    }
}
