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
        RearrangePlayers();
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

    public void RearrangePlayers() 
    { 
        Vector3 center = Vector3.zero;
        float radius = 3f;

        if (players.Count == 0)
        {
            return;
        }
        else if (players.Count == 1)
        {
            StartCoroutine(MovePlayers(center, players[0]));
            players[0].transform.position = center;
        }
        else
        {
            for (int i = 0; i < players.Count; i++)
            {
                float angle = (i * 360f / players.Count) * Mathf.Deg2Rad;
                Vector3 newPosition = center + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
                StartCoroutine(MovePlayers(newPosition, players[i]));
                players[i].transform.position = newPosition;
            }
        }
    }

    private IEnumerator MovePlayers(Vector3 targetPosition, GameObject player)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = player.transform.position;

        while (elapsedTime < 0.3f) // 0.3 seconds duration
        {
            player.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / 0.3f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        player.transform.position = targetPosition;

    }
}
