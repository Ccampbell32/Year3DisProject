using UnityEngine;

public class UnitStats : MonoBehaviour
{
    public string unitName = "Unit";
    public int maxHealth = 10;
    public int currentHealth;
    public int attackDamage = 3;
    public int attackRange = 1;

    [Header("Health Bar")]
    public GameObject healthBarPrefab;
    private HealthBar healthBarInstance;

    private Renderer rend;
    private Color baseColor;

    void Awake()
    {
        currentHealth = maxHealth;
        rend = GetComponent<Renderer>();
        if (rend != null)
            baseColor = rend.material.color;
    }

    void Start()
    {
        // Spawn health bar
        if (healthBarPrefab != null)
        {
            GameObject bar = Instantiate(healthBarPrefab, transform.position + Vector3.up * 2f, Quaternion.identity);
            bar.transform.SetParent(transform); // follow the unit
            healthBarInstance = bar.GetComponent<HealthBar>();
            healthBarInstance.Initialize(this);
        }
    }

    public bool IsAlive => currentHealth > 0;

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth < 0) currentHealth = 0;

        Debug.Log($"{unitName} took {amount} damage! ({currentHealth}/{maxHealth})");

        if (healthBarInstance != null)
            healthBarInstance.UpdateHealthBar();

        // Quick flash
        if (rend != null)
            rend.material.color = Color.red;

        Invoke(nameof(ResetColor), 0.2f);

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
        if (healthBarInstance != null)
            Destroy(healthBarInstance.gameObject);
        gameObject.SetActive(false);
    }
}
