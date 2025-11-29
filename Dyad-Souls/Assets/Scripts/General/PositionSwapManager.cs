using UnityEngine;

public class PositionSwapManager : MonoBehaviour
{
    public static PositionSwapManager Instance { get; private set; }

    [Header("Player References")]
    [SerializeField]
    private PlayerManager player1;

    [SerializeField]
    private PlayerManager player2;

    [Header("Swap Settings")]
    [SerializeField]
    private float requiredHoldTime = 0.3f;

    [SerializeField]
    private float swapCooldown = 1f;

    [Header("Debug Info")]
    [SerializeField]
    private bool player1IsHolding = false;

    [SerializeField]
    private bool player2IsHolding = false;

    [SerializeField]
    private float combinedHoldTimer = 0f;

    [SerializeField]
    private float cooldownTimer = 0f;

    private bool isSwapping = false;

    private void Awake()
    {
        // Singleton Pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (player1 == null || player2 == null)
        {
            PlayerManager[] players = FindObjectsByType<PlayerManager>(FindObjectsSortMode.None);
            if (players.Length >= 2)
            {
                player1 = players[0];
                player2 = players[1];
            }
        }
    }

    private void Update()
    {
        if (player1 == null || player2 == null || isSwapping)
            return;

        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            return;
        }

        if (player1IsHolding && player2IsHolding)
        {
            combinedHoldTimer += Time.deltaTime;

            if (combinedHoldTimer >= requiredHoldTime)
            {
                SwapPlayerPositions();
                ResetTimer();
            }
        }
        else
        {
            if (combinedHoldTimer > 0f)
                combinedHoldTimer = 0f;
        }
    }

    public void SetPlayer1Holding(bool holding)
    {
        player1IsHolding = holding;

        if (!holding)
            combinedHoldTimer = 0f;
    }

    public void SetPlayer2Holding(bool holding)
    {
        player2IsHolding = holding;

        if (!holding)
            combinedHoldTimer = 0f;
    }

    private void SwapPlayerPositions()
    {
        if (player1 == null || player2 == null)
        {
            return;
        }

        isSwapping = true;

        // Speichere nur die aktuellen Positionen (NICHT die Rotationen)
        Vector3 player1Position = player1.transform.position;
        Vector3 player2Position = player2.transform.position;

        // CharacterController blockiert direkte Transform-Änderungen
        // Wir müssen ihn kurz deaktivieren
        CharacterController cc1 = player1.GetComponent<CharacterController>();
        CharacterController cc2 = player2.GetComponent<CharacterController>();

        if (cc1 != null)
            cc1.enabled = false;
        if (cc2 != null)
            cc2.enabled = false;

        // Tausche nur die Positionen (Rotationen bleiben unverändert)
        player1.transform.position = player2Position;
        player2.transform.position = player1Position;

        // CharacterController wieder aktivieren
        if (cc1 != null)
            cc1.enabled = true;
        if (cc2 != null)
            cc2.enabled = true;

        // Starte Cooldown
        cooldownTimer = swapCooldown;

        isSwapping = false;
    }

    private void ResetTimer()
    {
        combinedHoldTimer = 0f;
    }

    public float GetHoldProgress()
    {
        if (!player1IsHolding || !player2IsHolding)
            return 0f;

        return Mathf.Clamp01(combinedHoldTimer / requiredHoldTime);
    }
}
