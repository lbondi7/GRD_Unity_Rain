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
        List<ParticleCollisionEvent> collision;

        var mat = other.GetComponent<MaterialData>();
        if (mat)
        {
            materialsData.Add(mat);
        }
    }
}
