using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public KeyCode passKey;
    public bool hasPotato = false;
    public bool canPass = true;
    public bool autoPlayTest = false;

    private bool bufferedPassInput = false;
    private float inputBufferTime = 2f; // Longer buffer
    private float bufferTimer = 0f;

    private TimingBarController timingBar;

    private void Start()
    {
        timingBar = FindObjectOfType<TimingBarController>();

        if (autoPlayTest && hasPotato)
        {
            StartCoroutine(AutoPassLoop());
        }
    }

    private void Update()
    {
        if (!autoPlayTest)
        {
            if (Input.GetKeyDown(passKey))
            {
                PlayThrowAnimation(); // 🎯 Always play animation immediately!

                if (hasPotato)
                {
                    if (timingBar != null && timingBar.IsInGreenZone())
                    {
                        BufferPassInput();
                    }
                    else
                    {
                        Debug.Log("❌ Pressed outside green zone!");
                    }
                }
            }

            if (bufferedPassInput)
            {
                bufferTimer -= Time.deltaTime;
                if (bufferTimer <= 0f)
                {
                    bufferedPassInput = false;
                }
                else if (hasPotato && canPass)
                {
                    StartCoroutine(BufferedPassPotato());
                    bufferedPassInput = false;
                }
            }
        }
    }

    private void BufferPassInput()
    {
        bufferedPassInput = true;
        bufferTimer = inputBufferTime;
        Debug.Log("📥 Pass input buffered.");
    }

    private void PlayThrowAnimation()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.SetTrigger("Throw");
        }
    }

    private IEnumerator AutoPassLoop()
    {
        while (autoPlayTest)
        {
            yield return new WaitForSeconds(Random.Range(1f, 3f));
            if (hasPotato && canPass)
            {
                StartCoroutine(BufferedPassPotato());
            }
        }
    }

    private IEnumerator BufferedPassPotato()
    {
        Potato potato = FindObjectOfType<Potato>();
        if (potato == null)
        {
            Debug.Log("❌ No potato found!");
            yield break;
        }

        Debug.Log("⏳ Waiting for potato to stop moving...");
        while (potato.isMoving)
        {
            yield return null;
        }
        Debug.Log("🏁 Potato stopped moving. Executing pass!");

        if (!hasPotato || !canPass)
        {
            Debug.Log($"❌ Can't pass anymore. hasPotato: {hasPotato}, canPass: {canPass}");
            yield break;
        }

        canPass = false;
        hasPotato = false;

        GameObject targetPlayer = GameManager.Instance.GetRandomPlayerExcluding(this.gameObject);
        if (targetPlayer != null)
        {
            potato.SetHolder(targetPlayer);

            PlayerController targetController = targetPlayer.GetComponent<PlayerController>();
            if (targetController != null)
            {
                targetController.ReceivePotato();
            }
        }
        else
        {
            Debug.Log("❌ No target player found!");
        }

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
