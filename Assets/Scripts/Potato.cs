using System.Collections;
using UnityEngine;

public class Potato : MonoBehaviour
{
    public float countdown;
    private GameObject holder;
    public float passSpeed = 5f; // Adjust for smoother animation

    private void Start()
    {
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
        if (holder != null)
        {
            holder.GetComponent<PlayerController>().hasPotato = false; // Remove potato from previous holder
        }

        holder = newHolder;
        holder.GetComponent<PlayerController>().hasPotato = true;

        // Start moving animation
        StartCoroutine(MoveToNewHolder(holder.transform.position));
    }

    private IEnumerator MoveToNewHolder(Vector3 targetPosition)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;

        while (elapsedTime < 0.3f) // 0.3 seconds duration
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / 0.3f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition; // Snap to exact position at the end
    }

    private void Explode()
    {
        Debug.Log(holder.name + " has exploded!");
        Destroy(holder); // Remove the player
        Destroy(gameObject); // Remove the potato
    }
}
