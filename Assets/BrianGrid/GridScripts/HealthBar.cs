using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image fillImage;
    private Transform cam;
    private UnitStats targetStats;

    void Start()
    {
        cam = Camera.main.transform;
    }

    public void Initialize(UnitStats target)
    {
        targetStats = target;
        UpdateHealthBar();
    }

    void Update()
    {
        if (targetStats == null) return;
        if (fillImage != null)
        {
            float fillAmount = (float)targetStats.currentHealth / targetStats.maxHealth;
            fillImage.fillAmount = fillAmount;
        }

        // Always face camera
        transform.LookAt(transform.position + cam.forward);
    }

    public void UpdateHealthBar()
    {
        if (targetStats == null) return;
        if (fillImage == null) return;

        float fillAmount = (float)targetStats.currentHealth / targetStats.maxHealth;
        fillImage.fillAmount = fillAmount;
    }
}
