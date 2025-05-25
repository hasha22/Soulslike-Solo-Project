using System.Collections.Generic;
using UnityEngine;

public class WorldGameSessionManager : MonoBehaviour
{
    public static WorldGameSessionManager instance { get; private set; }
    [Header("Active Players in Session")]
    public List<PlayerManager> players = new List<PlayerManager>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void AddPlayer(PlayerManager player)
    {
        if (!players.Contains(player))
        {
            players.Add(player);
        }

        for (int i = players.Count - 1; i > -1; i--)
        {
            if (players[i] == null)
                players.RemoveAt(i);
        }
    }
    public void RemovePlayer(PlayerManager player)
    {
        if (players.Contains(player))
        {
            players.Remove(player);
        }

        for (int i = players.Count - 1; i > -1; i--)
        {
            if (players[i] == null)
                players.RemoveAt(i);
        }
    }
}
