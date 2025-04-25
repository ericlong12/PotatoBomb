using UnityEngine;
using System.Collections;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    public KeyCode passKey; // Key assigned to this player
    public bool hasPotato = false; // Does this player currently hold the potato?
    public bool canPass; // Is this player allowed to pass right now?
    public bool autoPlayTest = false; // Used for automatic testing

    private void Start()
    {
        canPass = true;

        // Get player index based on GameObject name, like "Player0"
        int playerIndex = GetPlayerIndexFromName();

        // Try to load the saved key
        string keyName = PlayerPrefs.GetString("Player" + playerIndex + "_Key", "None");

        // If no key has been saved yet, assign default
        if (keyName == "None" || keyName == "Not Set")
        {
            KeyCode defaultKey = GetDefaultKeyForPlayer(playerIndex);
            keyName = defaultKey.ToString();
            PlayerPrefs.SetString("Player" + playerIndex + "_Key", keyName);
        }

        // Parse and assign the key
        if (System.Enum.TryParse(keyName, out KeyCode result))
        {
            passKey = result;
        }

        if (autoPlayTest && hasPotato)
        {
            StartCoroutine(AutoPassLoop());
        }
    }

    private void Update()
    {
        if (!hasPotato) canPass = false;

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
        if (potato == null || potato.isMoving) yield break;

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

    private int GetPlayerIndexFromName()
    {
        string name = gameObject.name; // e.g., "Player0"
        string digits = new string(name.Where(char.IsDigit).ToArray());
        int index;
        if (int.TryParse(digits, out index))
        {
            return index;
        }

        Debug.LogWarning("Could not determine player index from GameObject name: " + name);
        return 0;
    }

    private KeyCode GetDefaultKeyForPlayer(int index)
    {
        switch (index)
        {
            case 0: return KeyCode.BackQuote;     // `
            case 1: return KeyCode.A;             // A
            case 2: return KeyCode.C;             // C
            case 3: return KeyCode.Equals;        // =
            case 4: return KeyCode.Backslash;     // \
            case 5: return KeyCode.M;             // M
            default: return KeyCode.Space;
        }
    }
}
