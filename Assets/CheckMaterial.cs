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
        var droplets = GetComponent<DropletAudio>();
        List<ParticleCollisionEvent> collision = new List<ParticleCollisionEvent>();
        int amount = ps.GetCollisionEvents(other, collision);
        var mat = other.GetComponent<MaterialData>();
        // if (mat)
        // {
        //     materialsData.Add(mat);
        // }
        int i = 0;
        while (i < amount)
        {
            droplets.PlaySound(collision[i].intersection, mat.ID);
            i++;
        }
    }
}
