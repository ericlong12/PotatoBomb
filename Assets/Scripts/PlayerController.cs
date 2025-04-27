using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public KeyCode passKey;
    public bool hasPotato = false;
    public bool canPass;
    public bool autoPlayTest = false;

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
            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("Throw");
            }

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
            yield return new WaitForSeconds(Random.Range(1f, 3f));
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
        if (potato == null) yield break;

        // ✨ FIX: Wait if the potato is still moving
        while (potato.isMoving)
        {
            yield return null;
        }

        canPass = false;
        hasPotato = false;

        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Throw");
        }

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
            StartCoroutine(AutoPassLoop());
        }
    }
}
