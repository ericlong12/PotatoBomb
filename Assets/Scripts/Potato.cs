using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Potato : MonoBehaviour
{
    public float maxCountdown;
    public float countdown;
    private GameObject holder;
    public float passSpeed = 5f;
    public bool isMoving = false;

    private void Start()
    {
        GameObject firstHolder = GameManager.Instance.GetRandomPlayer();
        if (firstHolder != null)
        {
            firstHolder.GetComponent<PlayerController>().hasPotato = true;
            firstHolder.GetComponent<PlayerController>().canPass = true;
            transform.position = firstHolder.transform.position;
        }
        else
        {
            Debug.LogError("❌ No starting player found!");
        }

        SetRandomCountdown();
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
            holder.GetComponent<PlayerController>().hasPotato = false;
            holder.GetComponent<PlayerController>().canPass = false;
        }

        holder = newHolder;
        holder.GetComponent<PlayerController>().hasPotato = true;
        holder.GetComponent<PlayerController>().canPass = true;

        StartCoroutine(MoveToNewHolder(holder.transform.position));
    }

    private IEnumerator MoveToNewHolder(Vector3 targetPosition)
    {
        Debug.Log("🚀 Potato started moving.");
        isMoving = true;

        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;

        while (elapsedTime < 0.2f)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / 0.2f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        isMoving = false;
        Debug.Log("✅ Potato finished moving.");

        if (holder != null)
        {
            holder.GetComponent<PlayerController>().canPass = true;
        }
    }

    private void Explode()
    {
        Debug.Log($"{holder.name} has exploded!");

        GameManager.Instance.RemovePlayer(holder);

        string playerNum = holder.name.Substring(holder.name.Length - 1);
        Destroy(holder);

        GameManager.Instance.DestroyTag("P" + playerNum);

        if (GameManager.Instance.GetRemainingPlayers().Count == 1)
        {
            StopAllCoroutines();
            Destroy(gameObject);

            string winnerName = GameManager.Instance.GetRemainingPlayers()[0].name;
            Debug.Log($"{winnerName} is the winner!");

            PlayerPrefs.SetString("WinnerName", winnerName);
            PlayerPrefs.Save();

            SceneManager.LoadScene("WinScene");
            return;
        }

        // 🔥 Reset the Green Zone when starting next round
        TimingBarController timingBar = FindObjectOfType<TimingBarController>();
        if (timingBar != null)
        {
            timingBar.ResetGreenZone();
        }

        SetRandomCountdown();
        GameManager.Instance.RearrangePlayers();
        GameObject newHolder = GameManager.Instance.GetRandomPlayer();
        SetHolder(newHolder);
    }

    private void SetRandomCountdown()
    {
        if (Random.value < 0.5f)
        {
            countdown = Random.Range(5f, 12f);
        }
        else
        {
            countdown = Random.Range(12f, 28f);
        }

        maxCountdown = countdown;
    }
}
