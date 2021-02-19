using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalRain : BasicRain
{
    [Header("Intensity of the Rain")]
    [Range(50, 250)]
    public int Intensity = 100;
    [Header("Rain Lifetime")]
    [Range(5, 60)]
    public int Lifetime = 30;
    [Header("Active Range")]
    [Range(10, 50)]
    public int ActiveRange = 15;
    [Header("Entity Speed Reduction")]
    [Range(0.1f, 0.75f)]
    public float SpeedReduction = 0.5f;

    Collider triggerVolume;

    private void Awake()
    {
        float groundDistance = 4;
        RaycastHit hit;
        if (Physics.Raycast(transform.position,Vector3.down,out hit))
        {
            groundDistance = hit.distance;
        }
        if (!GetComponent<Collider>())
        {
            triggerVolume = gameObject.AddComponent<BoxCollider>();
            triggerVolume.bounds.center.Set(0, 0, 1);
            triggerVolume.bounds.size.Set(RainSize, RainSize, groundDistance + 1);
            triggerVolume.isTrigger = true;
        }
        else
        {
            triggerVolume = GetComponent<Collider>();
            if (!triggerVolume.isTrigger)
            {
                triggerVolume.isTrigger = true;
            }
        }
    }

    private void Update()
    {
        var rain = GetComponent<ParticleSystem>();
        if (Vector3.Distance(transform.position,player.transform.position) <= ActiveRange)
        {
            rain.Play();
        }
        else
        {
            rain.Stop();
        }
    }

    void OnValidate()
    {
        var rainPhysics = GetComponent<ParticleSystem>().collision;
        var rainTriggers = GetComponent<ParticleSystem>().trigger;
        var rain = GetComponent<ParticleSystem>().main;

        rainPhysics.enabled = true;
        rain.maxParticles = Intensity;
        rain.startLifetime = Lifetime;
        rainPhysics.maxKillSpeed = transform.position.y + 3;
        base.OnValidate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerControls>().amWet = true;
            other.gameObject.GetComponent<PlayerControls>().speedModifier = SpeedReduction;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerControls>().amWet = false;
            other.gameObject.GetComponent<PlayerControls>().speedModifier = 1;
        }
    }

}
