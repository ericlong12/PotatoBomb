using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private List<GameObject> players = new List<GameObject>();
    public static int initialPlayerCount;

    // Start is called before the first frame update

    public void Awake() {
        initialPlayerCount = PlayerPrefs.GetInt("PlayerCount");
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }
    
    public void Start()
    {
        
        players.AddRange(GameObject.FindGameObjectsWithTag("Player"));

        if (initialPlayerCount < 6)
        {
            for (int i = players.Count - 1; i >= initialPlayerCount; i--)
            {
                Destroy(players[i]);
                DestroyTag("P" + (i + 1));
                GameManager.Instance.RemovePlayer(players[i]);
            }
        }
    }

    public void RemovePlayer(GameObject player)
    {
        if (players.Contains(player))
        {
            players.Remove(player);
        }
    }

    public void DestroyTag(string tag)
    {
        Destroy(GameObject.Find(tag));
    }


    public List<GameObject> GetRemainingPlayers()
    {
        return players;
    }

    public GameObject GetRandomPlayer()
    {
        if (players.Count == 0) return null;
        
        GameObject randomPlayer = players[Random.Range(0, players.Count)];

        while(randomPlayer == null) {
            randomPlayer = players[Random.Range(0, players.Count)];
        }

        return randomPlayer;
    }

}
