using UnityEngine;

public class MilkmanDamageTurnBased : MonoBehaviour
{
    public int damageAmount = 1;
    public float damageRange = 1.2f;  // grid distance

    // Called ONLY during the Milkman's turn
    public void DoTurnDamage()
    {
        PlayerHealth[] players = FindObjectsOfType<PlayerHealth>();

        foreach (PlayerHealth p in players)
        {
            float dist = Vector3.Distance(transform.position, p.transform.position);

            if (dist <= damageRange)
            {
                p.TakeDamage(damageAmount);
            }
        }
    }
}
