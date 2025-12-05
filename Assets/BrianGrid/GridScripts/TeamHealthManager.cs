using System.Collections.Generic;
using UnityEngine;

public class TeamHealthManager : MonoBehaviour
{
    private List<PlayerHealth> teamMembers = new List<PlayerHealth>();

    private int deadPlayers = 0;

    public void RegisterPlayer(PlayerHealth player)
    {
        if (!teamMembers.Contains(player))
            teamMembers.Add(player);
    }

    public void NotifyPlayerDeath(PlayerHealth player)
    {
        deadPlayers++;

        if (deadPlayers >= teamMembers.Count)
        {
            AllPlayersDead();
        }
    }

    private void AllPlayersDead()
    {
        Debug.Log("TEAM DEAD — GAME OVER!");
        // You can trigger UI, reset level, show menu, etc.
    }
}
