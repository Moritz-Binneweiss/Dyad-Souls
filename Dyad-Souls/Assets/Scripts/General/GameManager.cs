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

    [Header("Boss Phase Settings")]
    [SerializeField]
    private Material phase2SkyboxMaterial;

    [SerializeField]
    private GameObject phase1BossModel;

    [SerializeField]
    private GameObject phase2BossModel;

    [SerializeField]
    private GameObject phase1Ground;

    [SerializeField]
    private GameObject phase2Ground;

    [SerializeField]
    private GameObject phase1Arena;

    [SerializeField]
    private GameObject phase2Arena;

    [SerializeField]
    private float phase2TransitionDelay = 2f;

    [Header("Game Over Settings")]
    [SerializeField]
    private float gameOverDelay = 3f;

    private int currentPhase = 1;
    private bool isInPhaseTransition = false;
    private Material originalSkyboxMaterial;
    private List<PlayerManager> deadPlayers = new List<PlayerManager>();
    private bool gameOverTriggered = false;

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

        // Store original skybox
        originalSkyboxMaterial = RenderSettings.skybox;

        // Setup Phase 1 - activate phase 1 models
        if (phase1BossModel != null)
            phase1BossModel.SetActive(true);
        if (phase2BossModel != null)
            phase2BossModel.SetActive(false);
        if (phase1Ground != null)
            phase1Ground.SetActive(true);
        if (phase2Ground != null)
            phase2Ground.SetActive(false);
        if (phase1Arena != null)
            phase1Arena.SetActive(true);
        if (phase2Arena != null)
            phase2Arena.SetActive(false);
    }

    void Update()
    {
        if (AreAllPlayersDead() && !isInPhaseTransition && !gameOverTriggered)
        {
            gameOverTriggered = true;
            StartCoroutine(GameOverDelayCoroutine());
        }

        // Check for Phase 2 transition (when Phase 1 boss is defeated)
        if (currentPhase == 1 && !isInPhaseTransition && bossEnemy != null && !bossEnemy.IsAlive())
        {
            StartPhase2Transition();
        }

        // Check for game win (Phase 2 boss defeated)
        if (currentPhase == 2 && !isInPhaseTransition && bossEnemy != null && !bossEnemy.IsAlive())
        {
            isInPhaseTransition = true;
            StartCoroutine(VictoryCoroutine());
        }
    }

    private IEnumerator GameOverDelayCoroutine()
    {
        yield return new WaitForSeconds(gameOverDelay);
        gameUIManager.GameOver();
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

    private void StartPhase2Transition()
    {
        isInPhaseTransition = true;
        StartCoroutine(Phase2TransitionCoroutine());
    }

    private IEnumerator Phase2TransitionCoroutine()
    {
        yield return new WaitForSeconds(phase2TransitionDelay);

        // Change skybox
        if (phase2SkyboxMaterial != null)
        {
            RenderSettings.skybox = phase2SkyboxMaterial;
            DynamicGI.UpdateEnvironment();
        }

        // Switch boss models
        if (phase1BossModel != null)
            phase1BossModel.SetActive(false);
        if (phase2BossModel != null)
            phase2BossModel.SetActive(true);

        // Switch ground and arena (Ground -> Ground2, BasicArena -> BasicArena2)
        if (phase1Ground != null)
            phase1Ground.SetActive(false);
        if (phase2Ground != null)
            phase2Ground.SetActive(true);
        if (phase1Arena != null)
            phase1Arena.SetActive(false);
        if (phase2Arena != null)
            phase2Arena.SetActive(true);

        // Update boss reference to Phase 2 boss (Liminor)
        if (phase2BossModel != null)
        {
            EnemyManager phase2Boss = phase2BossModel.GetComponent<EnemyManager>();
            if (phase2Boss != null)
            {
                bossEnemy = phase2Boss;

                // Update all player lock-on targets to new boss
                LockOnTarget[] lockOnScripts = FindObjectsByType<LockOnTarget>(
                    FindObjectsSortMode.None
                );
                foreach (LockOnTarget lockOn in lockOnScripts)
                {
                    lockOn.SetTargetEnemy(phase2Boss);
                }
            }
        }

        // Reset boss health to full and set name
        if (bossEnemy != null)
        {
            bossEnemy.ResetToFullHealth();
            bossEnemy.SetBossName("Liminor, Eternal Warden");
        }

        currentPhase = 2;
        isInPhaseTransition = false;
    }

    private IEnumerator VictoryCoroutine()
    {
        yield return new WaitForSeconds(1f); // Short delay before showing victory
        gameUIManager.ShowVictory();
        yield return new WaitForSeconds(7f); // Wait for fade animation to complete
        // You can add victory screen transition here
    }

    public int GetCurrentPhase() => currentPhase;
}
