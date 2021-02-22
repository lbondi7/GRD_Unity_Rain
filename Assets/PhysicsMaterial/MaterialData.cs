using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MaterialData : MonoBehaviour
{
    [SerializeField] [Range(0, 5)] private int id = 0;
    [SerializeField] [Range(0.0f, 1.0f)] private float wetness = 0.9f;
    [SerializeField] [Range(0.0f, 1.0f)] private float roughness = 0.0f;
    [SerializeField] [Range(0.0f, 1.0f)] private float thiccness = 0.9f;
    [SerializeField] private float timeSinceLastWetness = 60.0f;
    [SerializeField] private float dryingTimeMultiplier = 3.0f;

    public int ID { get => id; }
    public float Wetness { 
        get => wetness;
        set => wetness = value;
    }
    public float Roughness { get => roughness; }
    public float Thiccness { get => thiccness; }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastWetness = Mathf.Clamp(timeSinceLastWetness - Time.deltaTime, 0.0f, 60.0f);
        if (timeSinceLastWetness <= 0.0f)
        {
            wetness = Mathf.Clamp(wetness - Time.deltaTime * dryingTimeMultiplier, 0.0f, 1.0f);
        }
    }
}
