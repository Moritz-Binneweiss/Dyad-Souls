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

    private void InitializeEarthshaker()
    {
        if (earthshakerEffect == null)
        {
            earthshakerEffect = transform.Find("EarthshakerEffect")?.gameObject;
        }

        if (earthshakerEffect != null)
        {
            int childCount = earthshakerEffect.transform.childCount;
            earthshakerEffectParts = new GameObject[childCount];
            earthshakerEffectPartsOriginalPositions = new Vector3[childCount];

            for (int i = 0; i < childCount; i++)
            {
                earthshakerEffectParts[i] = earthshakerEffect.transform.GetChild(i).gameObject;
                earthshakerEffectPartsOriginalPositions[i] = earthshakerEffectParts[i]
                    .transform
                    .localPosition;
                SetParticleSystemToWorld(earthshakerEffectParts[i]);
            }
        }
    }

    private void InitializeEarthquake()
    {
        if (earthquakeEffect == null)
        {
            earthquakeEffect = transform.Find("EarthquakeEffect")?.gameObject;
        }

        if (earthquakeEffect != null)
        {
            int childCount = earthquakeEffect.transform.childCount;
            earthquakeEffectParts = new GameObject[childCount];
            earthquakeEffectPartsOriginalPositions = new Vector3[childCount];

            for (int i = 0; i < childCount; i++)
            {
                earthquakeEffectParts[i] = earthquakeEffect.transform.GetChild(i).gameObject;
                earthquakeEffectPartsOriginalPositions[i] = earthquakeEffectParts[i]
                    .transform
                    .localPosition;
                SetParticleSystemToWorld(earthquakeEffectParts[i]);
            }
        }
    }

    private void InitializeStomp1()
    {
        if (stomp1Effect == null)
        {
            stomp1Effect = transform.Find("Stomp1Effect")?.gameObject;
        }

        if (stomp1Effect != null)
        {
            int childCount = stomp1Effect.transform.childCount;
            stomp1Stones = new GameObject[childCount];
            stomp1StonesOriginalPositions = new Vector3[childCount];

            for (int i = 0; i < childCount; i++)
            {
                stomp1Stones[i] = stomp1Effect.transform.GetChild(i).gameObject;
                stomp1StonesOriginalPositions[i] = stomp1Stones[i].transform.localPosition;
                SetParticleSystemToWorld(stomp1Stones[i]);
            }
        }
    }

    private void InitializeStomp2()
    {
        if (stomp2Effect == null)
        {
            stomp2Effect = transform.Find("Stomp2Effect")?.gameObject;
        }

        if (stomp2Effect != null)
        {
            int childCount = stomp2Effect.transform.childCount;
            stomp2Stones = new GameObject[childCount];
            stomp2StonesOriginalPositions = new Vector3[childCount];

            for (int i = 0; i < childCount; i++)
            {
                stomp2Stones[i] = stomp2Effect.transform.GetChild(i).gameObject;
                stomp2StonesOriginalPositions[i] = stomp2Stones[i].transform.localPosition;
                SetParticleSystemToWorld(stomp2Stones[i]);
            }
        }
    }

    private void InitializeBestialRoar()
    {
        if (bestialRoarEffect == null)
        {
            bestialRoarEffect = transform.Find("BestialRoarEffect")?.gameObject;
        }

        if (bestialRoarEffect != null)
        {
            int childCount = bestialRoarEffect.transform.childCount;
            bestialRoarEffectParts = new GameObject[childCount];
            bestialRoarEffectPartsOriginalPositions = new Vector3[childCount];

            for (int i = 0; i < childCount; i++)
            {
                bestialRoarEffectParts[i] = bestialRoarEffect.transform.GetChild(i).gameObject;
                bestialRoarEffectPartsOriginalPositions[i] = bestialRoarEffectParts[i]
                    .transform
                    .localPosition;
                SetParticleSystemToWorld(bestialRoarEffectParts[i]);
            }
        }
    }

    public void EarthshakerEffectOn()
    {
        if (
            earthshakerEffect != null
            && earthshakerEffectParts != null
            && earthshakerEffectPartsOriginalPositions != null
        )
        {
            Vector3 bossPosition = transform.position + earthshakerPositionOffset;

            for (int i = 0; i < earthshakerEffectParts.Length; i++)
            {
                if (earthshakerEffectParts[i] != null)
                {
                    Vector3 relativePosition = earthshakerEffectPartsOriginalPositions[i];

                    earthshakerEffectParts[i].transform.SetParent(null);
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
            foreach (GameObject effect in earthshakerEffectParts)
            {
                if (effect != null)
                {
                    ParticleSystem[] particleSystems =
                        effect.GetComponentsInChildren<ParticleSystem>();
                    foreach (ParticleSystem ps in particleSystems)
                    {
                        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    }

                    effect.SetActive(false);
                    effect.transform.SetParent(earthshakerEffect.transform);
                }
            }
        }
    }

    public void EarthquakeEffectOn()
    {
        if (
            earthquakeEffect != null
            && earthquakeEffectParts != null
            && earthquakeEffectPartsOriginalPositions != null
        )
        {
            Vector3 bossPosition = transform.position + earthquakePositionOffset;

            for (int i = 0; i < earthquakeEffectParts.Length; i++)
            {
                if (earthquakeEffectParts[i] != null)
                {
                    Vector3 relativePosition = earthquakeEffectPartsOriginalPositions[i];

                    earthquakeEffectParts[i].transform.SetParent(null);
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
            foreach (GameObject effect in earthquakeEffectParts)
            {
                if (effect != null)
                {
                    ParticleSystem[] particleSystems =
                        effect.GetComponentsInChildren<ParticleSystem>();
                    foreach (ParticleSystem ps in particleSystems)
                    {
                        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    }

                    effect.SetActive(false);
                    effect.transform.SetParent(earthquakeEffect.transform);
                }
            }
        }
    }

    public void StompOneEffectOn()
    {
        if (stomp1Effect != null && stomp1Stones != null && stomp1StonesOriginalPositions != null)
        {
            Vector3 bossPosition = transform.position + stomp1PositionOffset;

            for (int i = 0; i < stomp1Stones.Length; i++)
            {
                if (stomp1Stones[i] != null)
                {
                    Vector3 relativePosition = stomp1StonesOriginalPositions[i];

                    stomp1Stones[i].transform.SetParent(null);
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
            foreach (GameObject stone in stomp1Stones)
            {
                if (stone != null)
                {
                    ParticleSystem[] particleSystems =
                        stone.GetComponentsInChildren<ParticleSystem>();
                    foreach (ParticleSystem ps in particleSystems)
                    {
                        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    }

                    stone.SetActive(false);
                    stone.transform.SetParent(stomp1Effect.transform);
                }
            }
        }
    }

    public void StompTwoEffectOn()
    {
        if (stomp2Effect != null && stomp2Stones != null && stomp2StonesOriginalPositions != null)
        {
            Vector3 bossPosition = transform.position + stomp2PositionOffset;

            for (int i = 0; i < stomp2Stones.Length; i++)
            {
                if (stomp2Stones[i] != null)
                {
                    Vector3 relativePosition = stomp2StonesOriginalPositions[i];

                    stomp2Stones[i].transform.SetParent(null);
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
            foreach (GameObject stone in stomp2Stones)
            {
                if (stone != null)
                {
                    ParticleSystem[] particleSystems =
                        stone.GetComponentsInChildren<ParticleSystem>();
                    foreach (ParticleSystem ps in particleSystems)
                    {
                        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    }
                    stone.SetActive(false);
                    stone.transform.SetParent(stomp2Effect.transform);
                }
            }
        }
    }

    public void BestialRoarOn()
    {
        if (
            bestialRoarEffect != null
            && bestialRoarEffectParts != null
            && bestialRoarEffectPartsOriginalPositions != null
        )
        {
            Vector3 bossPosition = transform.position + bestialRoarPositionOffset;

            for (int i = 0; i < bestialRoarEffectParts.Length; i++)
            {
                if (bestialRoarEffectParts[i] != null)
                {
                    Vector3 relativePosition = bestialRoarEffectPartsOriginalPositions[i];
                    bestialRoarEffectParts[i].transform.SetParent(null);
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
            foreach (GameObject effect in bestialRoarEffectParts)
            {
                if (effect != null)
                {
                    ParticleSystem[] particleSystems =
                        effect.GetComponentsInChildren<ParticleSystem>();
                    foreach (ParticleSystem ps in particleSystems)
                    {
                        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    }

                    effect.SetActive(false);

                    effect.transform.SetParent(bestialRoarEffect.transform);
                }
            }
        }
    }

    private void SetParticleSystemToWorld(GameObject obj)
    {
        if (obj == null)
            return;

        ParticleSystem[] particleSystems = obj.GetComponentsInChildren<ParticleSystem>(true);
        foreach (ParticleSystem ps in particleSystems)
        {
            var main = ps.main;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
        }
    }
}
