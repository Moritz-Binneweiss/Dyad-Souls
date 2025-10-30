using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Player References")]
    [SerializeField]
    private List<PlayerManager> players = new List<PlayerManager>();

    [Header("Boss References")]
    [SerializeField]
    private EnemyManager bossEnemy;

    [Header("UI References")]
    [SerializeField]
    private GameUIManager gameUIManager;

    [Header("Wave Settings")]
    [SerializeField]
    private float waveCooldownTime = 5f;

    [SerializeField]
    private float healthIncreasePerWave = 500f;

    private int currentWave = 1;
    private bool isInWaveCooldown = false;
    private float currentCooldownTime = 0f;
    private List<PlayerManager> deadPlayers = new List<PlayerManager>();

    void Start()
    {
        // Finde alle Player automatisch, falls nicht zugewiesen
        if (players.Count == 0)
        {
            PlayerManager[] foundPlayers = FindObjectsByType<PlayerManager>(
                FindObjectsSortMode.None
            );
            players.AddRange(foundPlayers);
        }

        // Finde Boss automatisch, falls nicht zugewiesen
        if (bossEnemy == null)
        {
            bossEnemy = FindFirstObjectByType<EnemyManager>();
        }
    }

    void Update()
    {
        // Prüfe ob alle Spieler tot sind
        if (AreAllPlayersDead() && !isInWaveCooldown)
        {
            gameUIManager.GameOver();
        }

        // Prüfe ob Boss tot ist
        if (bossEnemy != null && !bossEnemy.IsAlive() && !isInWaveCooldown)
        {
            StartWaveCooldown();
        }
    }

    public void OnPlayerDeath(PlayerManager player)
    {
        if (!deadPlayers.Contains(player))
        {
            deadPlayers.Add(player);
        }
    }

    private bool AreAllPlayersDead()
    {
        foreach (PlayerManager player in players)
        {
            if (player != null && !player.IsDead())
            {
                return false;
            }
        }
        return players.Count > 0; // Nur true wenn es Spieler gibt und alle tot sind
    }

    private void StartWaveCooldown()
    {
        isInWaveCooldown = true;
        StartCoroutine(WaveCooldownCoroutine());
    }

    private IEnumerator WaveCooldownCoroutine()
    {
        // Cooldown Timer
        currentCooldownTime = waveCooldownTime;
        while (currentCooldownTime > 0)
        {
            yield return new WaitForSeconds(0.1f);
            currentCooldownTime -= 0.1f;
        }

        // Starte nächste Wave
        currentWave++;

        // Belebe Boss wieder
        ReviveBoss();

        // Belebe tote Spieler wieder
        ReviveAllPlayers();

        isInWaveCooldown = false;
    }

    private void ReviveBoss()
    {
        if (bossEnemy != null)
        {
            // Erhöhe Boss Health
            float newMaxHealth = bossEnemy.GetMaxHealth() + healthIncreasePerWave;
            bossEnemy.Revive(newMaxHealth);
        }
    }

    private void ReviveAllPlayers()
    {
        foreach (PlayerManager player in deadPlayers)
        {
            if (player != null)
            {
                player.Revive();
            }
        }
        deadPlayers.Clear();
    }

    public int GetCurrentWave()
    {
        return currentWave;
    }
}
