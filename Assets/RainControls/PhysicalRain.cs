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
}
