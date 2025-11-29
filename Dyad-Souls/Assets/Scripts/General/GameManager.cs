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
    private float enemyDefeatedDisplayTime = 5f;

    [SerializeField]
    private float healthIncreasePerWave = 500f;

    private int currentWave = 1;
    private bool isInWaveCooldown = false;
    private float currentCooldownTime = 0f;
    private List<PlayerManager> deadPlayers = new List<PlayerManager>();

    void Start()
    {
        if (players.Count == 0)
        {
            PlayerManager[] foundPlayers = FindObjectsByType<PlayerManager>(
                FindObjectsSortMode.None
            );
            players.AddRange(foundPlayers);
        }

        if (bossEnemy == null)
            bossEnemy = FindFirstObjectByType<EnemyManager>();
    }

    void Update()
    {
        if (AreAllPlayersDead() && !isInWaveCooldown)
            gameUIManager.GameOver();

        if (bossEnemy != null && !bossEnemy.IsAlive() && !isInWaveCooldown)
            StartWaveCooldown();
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
                return false;
        }
        return players.Count > 0;
    }

    private void StartWaveCooldown()
    {
        isInWaveCooldown = true;
        StartCoroutine(WaveCooldownCoroutine());
    }

    private IEnumerator WaveCooldownCoroutine()
    {
        HealAllLivingPlayers();

        gameUIManager.ShowEnemyDefeated();
        yield return new WaitForSeconds(enemyDefeatedDisplayTime);
        gameUIManager.HideEnemyDefeated();

        currentCooldownTime = waveCooldownTime;
        while (currentCooldownTime > 0)
        {
            int secondsRemaining = Mathf.CeilToInt(currentCooldownTime);
            gameUIManager.ShowWaveCountdown(currentWave + 1, secondsRemaining);

            yield return new WaitForSeconds(0.1f);
            currentCooldownTime -= 0.1f;
        }

        gameUIManager.HideWaveCountdown();

        currentWave++;
        ReviveBoss();
        ReviveAllPlayers();

        isInWaveCooldown = false;
    }

    private void ReviveBoss()
    {
        if (bossEnemy != null)
        {
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

    private void HealAllLivingPlayers()
    {
        foreach (PlayerManager player in players)
        {
            if (player != null && !player.IsDead())
            {
                float missingHealth = player.GetMaxHealth() - player.GetCurrentHealth();
                if (missingHealth > 0)
                    player.Heal(missingHealth);
            }
        }
    }

    public int GetCurrentWave() => currentWave;
}
