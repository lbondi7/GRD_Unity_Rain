using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class CheckMaterial : MonoBehaviour
{

    ParticleSystem ps;

    private List<MaterialData> materialsData = new List<MaterialData>();
    
    
    // Start is called before the first frame update
    void Start()
    {
        ps  = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        materialsData.Clear();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (!other.GetComponent<MaterialData>() || !ps)
            return;

        var droplets = GetComponent<DropletAudio>();
        List<ParticleCollisionEvent> collision = new List<ParticleCollisionEvent>();
        int amount = ps.GetCollisionEvents(other, collision);
        var mat = other.GetComponent<MaterialData>();
        mat.TimeSinceLastWetness = 60.0f;
        int i = 0;
        while (i < amount)
        {
            if(mat.SimulateWetness) 
                mat.Wetness = Mathf.Clamp(mat.Wetness += 0.01f, 0.0f, 1.0f);
            
            droplets.PlaySound(collision[i].intersection, mat.ID);
            i++;
        }
        
        
    }
}
