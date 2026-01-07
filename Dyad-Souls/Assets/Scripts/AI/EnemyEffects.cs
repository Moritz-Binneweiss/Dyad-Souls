using Unity.VisualScripting;
using UnityEngine;

public class EnemyEffects : MonoBehaviour
{
    // Earthshaker Animation Effect
    // Prefab = "Liminor, Eternal Warden", Tag = "Enemy"
    // Animation = "Earthshaker"

    [SerializeField]
    private GameObject earthshakerEffect;

    [SerializeField]
    private GameObject earthquakeEffect;

    [Header("Earthshaker Effect Parts")]
    private GameObject earthShatter;
    private GameObject spikes;
    private GameObject shockwave; 

    [Header("Earthquake Effect Parts")]
    private GameObject shock;
    private GameObject dust;
    private GameObject rocks;
    private GameObject fireExplosion;
    
    
    private void Start()
    {
        InitializeEarthshaker();
        InitializeEarthquake();
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
            earthShatter = earthshakerEffect.transform.Find("EarthShatter")?.gameObject;
            spikes = earthshakerEffect.transform.Find("Spikes")?.gameObject;
            shockwave = earthshakerEffect.transform.Find("Shockwave")?.gameObject;
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
            shock = earthquakeEffect.transform.Find("Shock")?.gameObject;
            dust = earthquakeEffect.transform.Find("Dust")?.gameObject;
            rocks = earthquakeEffect.transform.Find("Rocks")?.gameObject;
            fireExplosion = earthquakeEffect.transform.Find("FireExplosion")?.gameObject;
        }
    }


//Earthshaker Actionevents
    public void EarthshakerEffectOn()
    {
        if (earthshakerEffect != null)
        {
            earthshakerEffect.SetActive(true);
        }
    }
    public void EarthshakerEffectOff()
    {
        if (earthshakerEffect != null)
        {
            earthshakerEffect.SetActive(false);
        }
    }

    public void EarthShatterOn()
    {
        if (earthShatter != null)
        {
            earthShatter.SetActive(true);
        }
    }
    public void EarthShatterOff()
    {
        if (earthShatter != null)
        {
            earthShatter.SetActive(false);
        }
    }

    public void SpikesOn()
    {
        if (spikes != null)
        {
            spikes.SetActive(true);
        }
    }
    public void SpikesOff()
    {
        if (spikes != null)
        {
            spikes.SetActive(false);
        }
    }

    public void ShockwaveOn()
    {
        if (shockwave != null)
        {
            shockwave.SetActive(true);
        }
    }
    public void ShockwaveOff()
    {
        if (shockwave != null)
        {
            shockwave.SetActive(false);
        }
    }


//Earthquake Actionevents
    public void SmashOn()
    {
        if (earthquakeEffect != null)
        {
            earthquakeEffect.SetActive(true);
        }
    }
    public void SmashOff()
    {
        if (earthquakeEffect != null)
        {
            earthquakeEffect.SetActive(false);
        }
    }

    public void ShockOn()
    {
        if (shock != null)
        {
            shock.SetActive(true);
        }
    }
    public void ShockOff()
    {
        if (shock != null)
        {
            shock.SetActive(false);
        }
    }

    public void DustOn()
    {
        if (dust != null)
        {
            dust.SetActive(true);
        }
    }
    public void DustOff()
    {
        if (dust != null)
        {
            dust.SetActive(false);
        }
    }

    public void RocksOn()
    {
        if (rocks != null)
        {
            rocks.SetActive(true);
        }
    }
    public void RocksOff()
    {
        if (rocks != null)
        {
            rocks.SetActive(false);
        }
    }

    public void FireExplosionOn()
    {
        if (fireExplosion != null)
        {
            fireExplosion.SetActive(true);
        }
    }
    public void FireExplosionOff()
    {
        if (fireExplosion != null)
        {
            fireExplosion.SetActive(false);
        }
    }

}