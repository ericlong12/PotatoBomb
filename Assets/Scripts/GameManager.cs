using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private List<GameObject> players = new List<GameObject>();
    // Start is called before the first frame update
    
    public void Awake() {
        if(Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }
    
    public void Start()
    {
        players.AddRange(GameObject.FindGameObjectsWithTag("Player"));
    }

    public void RemovePlayer(GameObject player)
    {
        if (players.Contains(player))
        {
            players.Remove(player);
        }
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
