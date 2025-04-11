using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public KeyCode passKey; // Unique key for each player
    public bool hasPotato = false; // Whether the player is holding a potato
    // private float passCooldown = 5f; // Prevents instant spam passing
    public bool canPass; // Flag to check if passing is allowed
    public bool autoPlayTest = false; // Enable automatic passing (for testing)

    private void Start()
    {
        canPass = true;
        if (autoPlayTest && hasPotato)
        {
            StartCoroutine(AutoPassLoop());
        }
    }

    private void Update()
    {
        if (!hasPotato)
        {
            canPass = false;
        }

        if (!autoPlayTest && Input.GetKeyDown(passKey))
        {
            // 🎯 Always trigger the Throw animation when pressing the button
            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("Throw");
            }

            // ✅ Only actually pass the potato if you have it and can pass
            if (hasPotato && canPass)
            {
                StartCoroutine(PassPotato());
            }
        }
    }

    private IEnumerator AutoPassLoop()
    {
        while (autoPlayTest)
        {
            yield return new WaitForSeconds(Random.Range(1f, 3f)); // Auto-pass every 1-3 seconds
            if (hasPotato)
            {
                StartCoroutine(PassPotato());
            }
        }
    }

    private IEnumerator PassPotato()
    {
        if (!canPass) yield break;

        Potato potato = FindObjectOfType<Potato>();
        if (potato == null || potato.isMoving) yield break; // Prevent passing while moving

        canPass = false;
        hasPotato = false;

        // ✨ NEW: Trigger the Throw animation!
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Throw");
        }

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject targetPlayer = GameManager.Instance.GetRandomPlayer();

        potato.SetHolder(targetPlayer);

        targetPlayer.GetComponent<PlayerController>().ReceivePotato();

        yield return new WaitForSeconds(0.3f);

        canPass = true;
    }

    public void ReceivePotato()
    {
        hasPotato = true;
        canPass = true;

        if (autoPlayTest)
        {
            StartCoroutine(AutoPassLoop()); // Restart auto-passing if enabled
        }
    }
}
