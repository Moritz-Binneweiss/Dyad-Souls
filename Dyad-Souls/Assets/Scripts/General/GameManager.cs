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

    [SerializeField]
    private PhaseTransition phaseTransitionCutscene;

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

    [Header("Phase 2 Effects")]
    [SerializeField]
    private GameObject meteorRainEffect;

    [SerializeField]
    private GameObject bloodRainEffect;

    [SerializeField]
    private float phase2TransitionDelay = 2f;

    [SerializeField]
    private float phase2BossIntroDuration = 3f;

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

        // Deactivate Phase 2 effects
        if (meteorRainEffect != null)
            meteorRainEffect.SetActive(false);
        if (bloodRainEffect != null)
            bloodRainEffect.SetActive(false);
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
        // Disable player combat and movement to prevent new actions
        List<PlayerMovement> playerMovements = new List<PlayerMovement>();
        List<PlayerCombatSystem> playerCombats = new List<PlayerCombatSystem>();

        foreach (PlayerManager player in players)
        {
            if (player != null && !player.IsDead())
            {
                // Disable combat and movement components
                PlayerMovement movement = player.GetComponent<PlayerMovement>();
                if (movement != null)
                {
                    movement.enabled = false;
                    playerMovements.Add(movement);
                }

                PlayerCombatSystem combat = player.GetComponent<PlayerCombatSystem>();
                if (combat != null)
                {
                    combat.enabled = false;
                    playerCombats.Add(combat);
                }
            }
        }

        // Play the cutscene
        if (phaseTransitionCutscene != null)
        {
            bool cutsceneComplete = false;
            phaseTransitionCutscene.PlayPhaseTransition(() => cutsceneComplete = true);

            // Wait for fade to black to complete (use realtime since timeScale is 0)
            // 0.3s pause + 1s fade = 1.3s, add small buffer
            yield return new WaitForSecondsRealtime(1.5f);

            // Screen is now black - briefly resume time to let animations finish
            Time.timeScale = 1f;
            yield return new WaitForSeconds(1f);
            Time.timeScale = 0f;

            // Reset all player animations to idle
            foreach (PlayerManager player in players)
            {
                if (player != null && !player.IsDead())
                {
                    Animator animator = player.GetComponent<Animator>();
                    if (animator != null)
                    {
                        animator.Rebind();
                        animator.Update(0f);
                    }
                }
            }

            // Now that screen is black, switch everything in the background
            // (Time is frozen again, player won't see the switch)

            // Change skybox
            if (phase2SkyboxMaterial != null)
            {
                RenderSettings.skybox = phase2SkyboxMaterial;
                DynamicGI.UpdateEnvironment();
            }

            // Switch boss models (screen is black, player won't see this)
            if (phase1BossModel != null)
                phase1BossModel.SetActive(false);
            if (phase2BossModel != null)
                phase2BossModel.SetActive(true);

            // Switch ground and arena
            if (phase1Ground != null)
                phase1Ground.SetActive(false);
            if (phase2Ground != null)
                phase2Ground.SetActive(true);
            if (phase1Arena != null)
                phase1Arena.SetActive(false);
            if (phase2Arena != null)
                phase2Arena.SetActive(true);

            // Update boss reference to Phase 2 boss
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

                // Disable behavior tree initially so boss doesn't attack immediately
                BehaviorDesigner.Runtime.BehaviorTree bossBT =
                    bossEnemy.GetComponent<BehaviorDesigner.Runtime.BehaviorTree>();
                if (bossBT != null)
                {
                    bossBT.enabled = false;
                }
            }

            // Now wait for cutscene to finish
            while (!cutsceneComplete)
            {
                yield return null;
            }

            // Re-enable player movement and combat
            foreach (PlayerMovement movement in playerMovements)
            {
                if (movement != null)
                {
                    movement.enabled = true;
                }
            }

            foreach (PlayerCombatSystem combat in playerCombats)
            {
                if (combat != null)
                {
                    combat.enabled = true;
                }
            }

            // Activate Phase 2 effects
            if (meteorRainEffect != null)
                meteorRainEffect.SetActive(true);
            if (bloodRainEffect != null)
                bloodRainEffect.SetActive(true);

            // Give player a brief moment to see Phase 2 boss before combat starts
            yield return new WaitForSeconds(phase2BossIntroDuration);

            // Now enable boss AI to start Phase 2 combat
            if (bossEnemy != null)
            {
                BehaviorDesigner.Runtime.BehaviorTree bossBT =
                    bossEnemy.GetComponent<BehaviorDesigner.Runtime.BehaviorTree>();
                if (bossBT != null)
                {
                    bossBT.EnableBehavior();
                }
            }
        }
        else
        {
            // Fallback if no cutscene is assigned
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

            // Switch ground and arena
            if (phase1Ground != null)
                phase1Ground.SetActive(false);
            if (phase2Ground != null)
                phase2Ground.SetActive(true);
            if (phase1Arena != null)
                phase1Arena.SetActive(false);
            if (phase2Arena != null)
                phase2Arena.SetActive(true);

            // Update boss reference to Phase 2 boss
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

                // Disable behavior tree initially so boss doesn't attack immediately
                BehaviorDesigner.Runtime.BehaviorTree bossBT =
                    bossEnemy.GetComponent<BehaviorDesigner.Runtime.BehaviorTree>();
                if (bossBT != null)
                {
                    bossBT.enabled = false;
                }
            }

            // Activate Phase 2 effects
            if (meteorRainEffect != null)
                meteorRainEffect.SetActive(true);
            if (bloodRainEffect != null)
                bloodRainEffect.SetActive(true);

            // Give player a brief moment to see Phase 2 boss before combat starts
            yield return new WaitForSeconds(phase2BossIntroDuration);

            // Now enable boss AI to start Phase 2 combat
            if (bossEnemy != null)
            {
                BehaviorDesigner.Runtime.BehaviorTree bossBT =
                    bossEnemy.GetComponent<BehaviorDesigner.Runtime.BehaviorTree>();
                if (bossBT != null)
                {
                    bossBT.EnableBehavior();
                }
            }
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
