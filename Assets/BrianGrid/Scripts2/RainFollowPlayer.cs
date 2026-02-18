using UnityEngine;

public class RainFollowPlayer : MonoBehaviour
{
    public Transform player;
    public float height = 10f;

    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = new Vector3(
                player.position.x,
                player.position.y + height,
                player.position.z
            );
        }
    }
}
