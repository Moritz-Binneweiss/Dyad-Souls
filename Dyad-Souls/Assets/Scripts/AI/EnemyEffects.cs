using Unity.VisualScripting;
using UnityEngine;

public class EnemyEffects : MonoBehaviour
{
    [Header("Earthshaker Effect Parts")]
    [SerializeField]
    private GameObject earthshakerEffect;

    private GameObject[] earthshakerEffectParts;
    private Vector3[] earthshakerEffectPartsOriginalPositions;

    [SerializeField]
    private Vector3 earthshakerPositionOffset = Vector3.zero;

    [Header("Earthquake Effect Parts")]
    [SerializeField]
    private GameObject earthquakeEffect;
    private GameObject[] earthquakeEffectParts;
    private Vector3[] earthquakeEffectPartsOriginalPositions;

    [SerializeField]
    private Vector3 earthquakePositionOffset = Vector3.zero;

    [Header("Stomp1 Effect Parts")]
    [SerializeField]
    private GameObject stomp1Effect;
    private GameObject[] stomp1Stones;
    private Vector3[] stomp1StonesOriginalPositions;

    [SerializeField]
    private Vector3 stomp1PositionOffset = Vector3.zero;

    [Header("Stomp2 Effect Parts")]
    [SerializeField]
    private GameObject stomp2Effect;
    private GameObject[] stomp2Stones;
    private Vector3[] stomp2StonesOriginalPositions;

    [SerializeField]
    private Vector3 stomp2PositionOffset = Vector3.zero;

    [Header("Bestial Roar Effect Parts")]
    [SerializeField]
    private GameObject bestialRoarEffect;
    private GameObject[] bestialRoarEffectParts;
    private Vector3[] bestialRoarEffectPartsOriginalPositions;

    [SerializeField]
    private Vector3 bestialRoarPositionOffset = Vector3.zero;

    private void Start()
    {
        InitializeEarthshaker();
        InitializeEarthquake();
        InitializeStomp1();
        InitializeStomp2();
        InitializeBestialRoar();
    }

    // ===== EARTHSHAKER INITIALIZATION =====
    private void InitializeEarthshaker()
    {
        if (earthshakerEffect == null)
        {
            earthshakerEffect = transform.Find("EarthshakerEffect")?.gameObject;
        }

        if (earthshakerEffect != null)
        {
            // Hole alle Child-GameObjects (die Effekte)
            int childCount = earthshakerEffect.transform.childCount;
            earthshakerEffectParts = new GameObject[childCount];
            earthshakerEffectPartsOriginalPositions = new Vector3[childCount];

            for (int i = 0; i < childCount; i++)
            {
                earthshakerEffectParts[i] = earthshakerEffect.transform.GetChild(i).gameObject;
                // Speichere die ursprüngliche lokale Position
                earthshakerEffectPartsOriginalPositions[i] = earthshakerEffectParts[i]
                    .transform
                    .localPosition;
                // Setze Particle Systems auf World Space
                SetParticleSystemToWorld(earthshakerEffectParts[i]);
            }
        }
    }

    // ===== EARTHQUAKE INITIALIZATION =====
    private void InitializeEarthquake()
    {
        if (earthquakeEffect == null)
        {
            earthquakeEffect = transform.Find("EarthquakeEffect")?.gameObject;
        }

        if (earthquakeEffect != null)
        {
            // Hole alle Child-GameObjects (die Effekte)
            int childCount = earthquakeEffect.transform.childCount;
            earthquakeEffectParts = new GameObject[childCount];
            earthquakeEffectPartsOriginalPositions = new Vector3[childCount];

            for (int i = 0; i < childCount; i++)
            {
                earthquakeEffectParts[i] = earthquakeEffect.transform.GetChild(i).gameObject;
                // Speichere die ursprüngliche lokale Position
                earthquakeEffectPartsOriginalPositions[i] = earthquakeEffectParts[i]
                    .transform
                    .localPosition;
                // Setze Particle Systems auf World Space
                SetParticleSystemToWorld(earthquakeEffectParts[i]);
            }
        }
    }

    // ===== STOMP1 INITIALIZATION =====
    private void InitializeStomp1()
    {
        if (stomp1Effect == null)
        {
            stomp1Effect = transform.Find("Stomp1Effect")?.gameObject;
        }

        if (stomp1Effect != null)
        {
            // Hole alle Child-GameObjects (die Steine)
            int childCount = stomp1Effect.transform.childCount;
            stomp1Stones = new GameObject[childCount];
            stomp1StonesOriginalPositions = new Vector3[childCount];

            for (int i = 0; i < childCount; i++)
            {
                stomp1Stones[i] = stomp1Effect.transform.GetChild(i).gameObject;
                // Speichere die ursprüngliche lokale Position
                stomp1StonesOriginalPositions[i] = stomp1Stones[i].transform.localPosition;
                // Setze Particle Systems auf World Space
                SetParticleSystemToWorld(stomp1Stones[i]);
            }
        }
    }

    // ===== STOMP2 INITIALIZATION =====
    private void InitializeStomp2()
    {
        if (stomp2Effect == null)
        {
            stomp2Effect = transform.Find("Stomp2Effect")?.gameObject;
        }

        if (stomp2Effect != null)
        {
            // Hole alle Child-GameObjects (die Steine)
            int childCount = stomp2Effect.transform.childCount;
            stomp2Stones = new GameObject[childCount];
            stomp2StonesOriginalPositions = new Vector3[childCount];

            for (int i = 0; i < childCount; i++)
            {
                stomp2Stones[i] = stomp2Effect.transform.GetChild(i).gameObject;
                // Speichere die ursprüngliche lokale Position
                stomp2StonesOriginalPositions[i] = stomp2Stones[i].transform.localPosition;
                // Setze Particle Systems auf World Space
                SetParticleSystemToWorld(stomp2Stones[i]);
            }
        }
    }

    // ===== BESTIAL ROAR INITIALIZATION =====
    private void InitializeBestialRoar()
    {
        if (bestialRoarEffect == null)
        {
            bestialRoarEffect = transform.Find("BestialRoarEffect")?.gameObject;
        }

        if (bestialRoarEffect != null)
        {
            // Hole alle Child-GameObjects (die Effekte)
            int childCount = bestialRoarEffect.transform.childCount;
            bestialRoarEffectParts = new GameObject[childCount];
            bestialRoarEffectPartsOriginalPositions = new Vector3[childCount];

            for (int i = 0; i < childCount; i++)
            {
                bestialRoarEffectParts[i] = bestialRoarEffect.transform.GetChild(i).gameObject;
                // Speichere die ursprüngliche lokale Position
                bestialRoarEffectPartsOriginalPositions[i] = bestialRoarEffectParts[i]
                    .transform
                    .localPosition;
                // Setze Particle Systems auf World Space
                SetParticleSystemToWorld(bestialRoarEffectParts[i]);
            }
        }
    }

    //Earthshaker Actionevents
    public void EarthshakerEffectOn()
    {
        if (
            earthshakerEffect != null
            && earthshakerEffectParts != null
            && earthshakerEffectPartsOriginalPositions != null
        )
        {
            // Speichere die aktuelle Boss-Position + Offset
            Vector3 bossPosition = transform.position + earthshakerPositionOffset;

            // Aktiviere jeden Effekt und löse ihn vom Boss
            for (int i = 0; i < earthshakerEffectParts.Length; i++)
            {
                if (earthshakerEffectParts[i] != null)
                {
                    // Verwende die gespeicherte ursprüngliche relative Position
                    Vector3 relativePosition = earthshakerEffectPartsOriginalPositions[i];

                    // Entferne den Parent (dadurch bleibt der Effekt an der World-Position stehen)
                    earthshakerEffectParts[i].transform.SetParent(null);

                    // Setze die Position relativ zur Boss-Position (mit Offset)
                    earthshakerEffectParts[i].transform.position = bossPosition + relativePosition;

                    earthshakerEffectParts[i].SetActive(true);
                }
            }
        }
    }

    public void EarthshakerEffectOff()
    {
        if (earthshakerEffect != null && earthshakerEffectParts != null)
        {
            // Deaktiviere die Effekte und setze sie zurück als Children
            foreach (GameObject effect in earthshakerEffectParts)
            {
                if (effect != null)
                {
                    // Stoppe alle Particle Systems
                    ParticleSystem[] particleSystems = effect.GetComponentsInChildren<ParticleSystem>();
                    foreach (ParticleSystem ps in particleSystems)
                    {
                        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    }

                    effect.SetActive(false);

                    // Setze die Effekte zurück als Children vom EarthshakerEffect
                    effect.transform.SetParent(earthshakerEffect.transform);
                }
            }
        }
    }

    //Earthquake Actionevents
    public void EarthquakeEffectOn()
    {
        if (
            earthquakeEffect != null
            && earthquakeEffectParts != null
            && earthquakeEffectPartsOriginalPositions != null
        )
        {
            // Speichere die aktuelle Boss-Position + Offset
            Vector3 bossPosition = transform.position + earthquakePositionOffset;

            // Aktiviere jeden Effekt und löse ihn vom Boss
            for (int i = 0; i < earthquakeEffectParts.Length; i++)
            {
                if (earthquakeEffectParts[i] != null)
                {
                    // Verwende die gespeicherte ursprüngliche relative Position
                    Vector3 relativePosition = earthquakeEffectPartsOriginalPositions[i];

                    // Entferne den Parent (dadurch bleibt der Effekt an der World-Position stehen)
                    earthquakeEffectParts[i].transform.SetParent(null);

                    // Setze die Position relativ zur Boss-Position (mit Offset)
                    earthquakeEffectParts[i].transform.position = bossPosition + relativePosition;

                    earthquakeEffectParts[i].SetActive(true);
                }
            }
        }
    }

    public void EarthquakeEffectOff()
    {
        if (earthquakeEffect != null && earthquakeEffectParts != null)
        {
            // Deaktiviere die Effekte und setze sie zurück als Children
            foreach (GameObject effect in earthquakeEffectParts)
            {
                if (effect != null)
                {
                    // Stoppe alle Particle Systems
                    ParticleSystem[] particleSystems = effect.GetComponentsInChildren<ParticleSystem>();
                    foreach (ParticleSystem ps in particleSystems)
                    {
                        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    }

                    effect.SetActive(false);

                    // Setze die Effekte zurück als Children vom EarthquakeEffect
                    effect.transform.SetParent(earthquakeEffect.transform);
                }
            }
        }
    }

    // Stomp1 Actionevents
    public void StompOneEffectOn()
    {
        if (stomp1Effect != null && stomp1Stones != null && stomp1StonesOriginalPositions != null)
        {
            // Speichere die aktuelle Boss-Position + Offset
            Vector3 bossPosition = transform.position + stomp1PositionOffset;

            // Aktiviere jeden Stein und löse ihn vom Boss
            for (int i = 0; i < stomp1Stones.Length; i++)
            {
                if (stomp1Stones[i] != null)
                {
                    // Verwende die gespeicherte ursprüngliche relative Position
                    Vector3 relativePosition = stomp1StonesOriginalPositions[i];

                    // Entferne den Parent (dadurch bleibt der Stein an der World-Position stehen)
                    stomp1Stones[i].transform.SetParent(null);

                    // Setze die Position relativ zur Boss-Position (mit Offset)
                    stomp1Stones[i].transform.position = bossPosition + relativePosition;

                    stomp1Stones[i].SetActive(true);
                }
            }
        }
    }

    public void StompOneEffectOff()
    {
        if (stomp1Effect != null && stomp1Stones != null)
        {
            // Deaktiviere die Steine und setze sie zurück als Children
            foreach (GameObject stone in stomp1Stones)
            {
                if (stone != null)
                {
                    // Stoppe alle Particle Systems
                    ParticleSystem[] particleSystems = stone.GetComponentsInChildren<ParticleSystem>();
                    foreach (ParticleSystem ps in particleSystems)
                    {
                        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    }

                    stone.SetActive(false);

                    // Setze die Steine zurück als Children vom Stomp1Effect
                    stone.transform.SetParent(stomp1Effect.transform);
                }
            }
        }
    }

    // Stomp2 Actionevents
    public void StompTwoEffectOn()
    {
        if (stomp2Effect != null && stomp2Stones != null && stomp2StonesOriginalPositions != null)
        {
            // Speichere die aktuelle Boss-Position + Offset
            Vector3 bossPosition = transform.position + stomp2PositionOffset;

            // Aktiviere jeden Stein und löse ihn vom Boss
            for (int i = 0; i < stomp2Stones.Length; i++)
            {
                if (stomp2Stones[i] != null)
                {
                    // Verwende die gespeicherte ursprüngliche relative Position
                    Vector3 relativePosition = stomp2StonesOriginalPositions[i];

                    // Entferne den Parent (dadurch bleibt der Stein an der World-Position stehen)
                    stomp2Stones[i].transform.SetParent(null);

                    // Setze die Position relativ zur Boss-Position (mit Offset)
                    stomp2Stones[i].transform.position = bossPosition + relativePosition;

                    stomp2Stones[i].SetActive(true);
                }
            }
        }
    }

    public void StompTwoEffectOff()
    {
        if (stomp2Effect != null && stomp2Stones != null)
        {
            // Deaktiviere die Steine und setze sie zurück als Children
            foreach (GameObject stone in stomp2Stones)
            {
                if (stone != null)
                {
                    // Stoppe alle Particle Systems
                    ParticleSystem[] particleSystems = stone.GetComponentsInChildren<ParticleSystem>();
                    foreach (ParticleSystem ps in particleSystems)
                    {
                        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    }

                    stone.SetActive(false);

                    // Setze die Steine zurück als Children vom Stomp2Effect
                    stone.transform.SetParent(stomp2Effect.transform);
                }
            }
        }
    }

    // BestialRoar Actionevents
    public void BestialRoarOn()
    {
        if (
            bestialRoarEffect != null
            && bestialRoarEffectParts != null
            && bestialRoarEffectPartsOriginalPositions != null
        )
        {
            // Speichere die aktuelle Boss-Position + Offset
            Vector3 bossPosition = transform.position + bestialRoarPositionOffset;

            // Aktiviere jeden Effekt und löse ihn vom Boss
            for (int i = 0; i < bestialRoarEffectParts.Length; i++)
            {
                if (bestialRoarEffectParts[i] != null)
                {
                    // Verwende die gespeicherte ursprüngliche relative Position
                    Vector3 relativePosition = bestialRoarEffectPartsOriginalPositions[i];

                    // Entferne den Parent (dadurch bleibt der Effekt an der World-Position stehen)
                    bestialRoarEffectParts[i].transform.SetParent(null);

                    // Setze die Position relativ zur Boss-Position (mit Offset)
                    bestialRoarEffectParts[i].transform.position = bossPosition + relativePosition;

                    bestialRoarEffectParts[i].SetActive(true);
                }
            }
        }
    }

    public void BestialRoarOff()
    {
        if (bestialRoarEffect != null && bestialRoarEffectParts != null)
        {
            // Deaktiviere die Effekte und setze sie zurück als Children
            foreach (GameObject effect in bestialRoarEffectParts)
            {
                if (effect != null)
                {
                    // Stoppe alle Particle Systems
                    ParticleSystem[] particleSystems = effect.GetComponentsInChildren<ParticleSystem>();
                    foreach (ParticleSystem ps in particleSystems)
                    {
                        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    }

                    effect.SetActive(false);

                    // Setze die Effekte zurück als Children vom BestialRoarEffect
                    effect.transform.SetParent(bestialRoarEffect.transform);
                }
            }
        }
    }

    // ===== HELPER METHOD =====
    private void SetParticleSystemToWorld(GameObject obj)
    {
        if (obj == null)
            return;

        // Setze alle Particle Systems in diesem GameObject auf World Space
        ParticleSystem[] particleSystems = obj.GetComponentsInChildren<ParticleSystem>(true);
        foreach (ParticleSystem ps in particleSystems)
        {
            var main = ps.main;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
        }
    }
}
