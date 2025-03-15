using System.Collections;
using UnityEngine;

public class Potato : MonoBehaviour
{
    public float countdown;
    private GameObject holder;
    public float passSpeed = 5f; // Adjust for smoother animation

    public bool isMoving = false;
    private void Start()
    {
        GameObject firstHolder = GameManager.Instance.GetRandomPlayer();
        transform.position = firstHolder.transform.position;
        
        // Set a random explosion time: either 3s, 7s, or a random value between 30s-60s
        int[] fixedTimes = { 3, 7 };
        if (Random.value < 0.5f)
        {
            countdown = fixedTimes[Random.Range(0, fixedTimes.Length)];
        }
        else
        {
            countdown = Random.Range(30f, 60f);
        }
    }

    private void Update()
    {
        if (holder != null)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0)
            {
                Explode();
            }
        }
    }

    public void SetHolder(GameObject newHolder)
    {
        
        //holder.GetComponent<PlayerController>().hasPotato = false; // Remove potato from previous holder

        // if (isMoving && holder == null) return;

        if(holder != null) {
            holder.GetComponent<PlayerController>().hasPotato = false; // Remove potato from previous holder
            holder.GetComponent<PlayerController>().canPass = false;
        }

        holder = newHolder;

        holder.GetComponent<PlayerController>().hasPotato = true;
        holder.GetComponent<PlayerController>().canPass = true;
        
        //holder.ReceivePotato();
        // Start moving animation
        StartCoroutine(MoveToNewHolder(holder.transform.position));
    }

    private IEnumerator MoveToNewHolder(Vector3 targetPosition)
    {
        isMoving = true;
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;

        // holder.GetComponent<PlayerController>().canPass = false;

        while (elapsedTime < 0.3f) // 0.3 seconds duration
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / 0.3f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition; // Snap to exact position at the end
        isMoving = false;

        if(holder != null) {
            holder.GetComponent<PlayerController>().canPass = true;
        }
    }

    private void Explode()
    {
        Debug.Log(holder.name + " has exploded!");

        GameManager.Instance.RemovePlayer(holder);
        Destroy(holder); // Remove the player

        // StopAllCoroutines();
        
        // Destroy(gameObject); // Remove the potato

        if (GameManager.Instance.GetRemainingPlayers().Count == 1) {
            StopAllCoroutines();
            Destroy(gameObject);
            Debug.Log(GameManager.Instance.GetRemainingPlayers()[0].name + " is the winner!");
        }
    
        // holder = null;

        GameObject newHolder = GameManager.Instance.GetRandomPlayer();

        int[] fixedTimes = { 3, 7 };
        if (Random.value < 0.5f)
        {
            countdown = fixedTimes[Random.Range(0, fixedTimes.Length)];
        }
        else
        {
            countdown = Random.Range(30f, 60f);
        }

        SetHolder(newHolder);
       
    }
}
