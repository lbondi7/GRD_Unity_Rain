using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEditor;

// Wrapper class for any object that can get wet
public class WettableObject : MonoBehaviour
{

    /// <summary>
    ///  The amount of time in seconds this object has been receiving collisions from rain.
    /// </summary>
    [HideInInspector] public float wetness;
    [HideInInspector] public WetnessState CurrentWetState;
    [HideInInspector] public int currentWetnessLevel = 0;
    /// <summary>
    /// Thresholds passed by <see cref="wetness"/> as it increases. 
    /// <para>Use to differentiate "levels of wetness" of an object. </para>
    /// </summary>
    [HideInInspector] public List<int> wetnessTimeThresholds = new List<int>();

    protected float dryTimer = 0.5f;

    /// <summary>
    /// A base material for typical use. Is swapped out with the "Fade" material if fading is enabled.
    /// </summary>
    [HideInInspector] protected Material baseMaterial;

    ///<summary>
    /// The material currently being used by the object. Is swapped in and out when fading is used.
    /// </summary>
    [HideInInspector] private Material currentMaterial;
    /// <summary>
    /// An alternate material that is only used if fading is enabled.
    /// </summary>
    [HideInInspector] public Material fMaterial;

    private Material usableFMaterial;
    

    [HideInInspector] protected bool faded;
    public enum WetnessState
    {
        OutOfRain = 0,
        InRain = 1,
        FullyWet = 2
    }

    private void Start()
    {
        baseMaterial = GetComponent<Renderer>().material;
        currentMaterial = baseMaterial;
        usableFMaterial = new Material(fMaterial);
    }

    private void Update()
    {
        if (!currentMaterial) currentMaterial = GetComponent<Renderer>().material;
        if (!usableFMaterial) usableFMaterial = fMaterial;
        if (dryTimer > 0f) 
            dryTimer -= Time.deltaTime;
        else
        {
            dryTimer = 0f;
            // TODO: Make new function that goes in reverse to make object "Drier" over time.
            if (wetness > 0f) wetness -= Time.deltaTime;
            else wetness = 0f;

            for (int i = wetnessTimeThresholds.Count-1; i > 1; i--)
            {
                if (currentWetnessLevel == i &&
                    wetness <= wetnessTimeThresholds[i])
                    --currentWetnessLevel;
            }

            if (currentWetnessLevel == 1 && wetness == 0)
                currentWetnessLevel = 0;

            ApplyRainEffect();
        }

        UpdateWetState();
    }

    private void UpdateWetState()
    {
        if (currentWetnessLevel == 0)
            CurrentWetState = WetnessState.OutOfRain;
        else if (currentWetnessLevel > 0 && currentWetnessLevel < wetnessTimeThresholds.Count - 1)
            CurrentWetState = WetnessState.InRain;
        else if (currentWetnessLevel >= wetnessTimeThresholds.Count - 1)
            CurrentWetState = WetnessState.FullyWet;
    }


    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Rain"))
            UpdateInRain();
    }

    /// <summary>
    /// Only runs every frame if the object has collided with rain.
    /// </summary>
    private void UpdateInRain()
    {
        if (CurrentWetState == WetnessState.OutOfRain)
        {
            currentWetnessLevel++;
            dryTimer = 5f;
            return;
        }
        wetness += 0.1f;

            for (int i = 0; i < wetnessTimeThresholds.Count - 1; i++)
            {
                if (currentWetnessLevel == i &&
                     wetness >= wetnessTimeThresholds[i])
                    ++currentWetnessLevel;
            }
            ApplyRainEffect();
            
        dryTimer = 5f;
    }

    /// <summary>
    /// Executes when the object is in contact with rain.
    /// <para>Use <see cref="currentWetnessLevel"/> and <see cref="wetnessTimeThresholds"/> to differentiate levels of wetness.</para>
    /// </summary>
    virtual protected void ApplyRainEffect()
    { }

    protected IEnumerator FadeOut(float fadeSpeed)
    {
        if (usableFMaterial.color.a == 0) yield break;
        while(true)
        {
            yield return Fade(0, fadeSpeed);
            if (usableFMaterial.color.a == 0)
            {
                yield break;
            }
        }
    }

    protected IEnumerator FadeIn(float fadeSpeed)
    {
        if (usableFMaterial.color.a == 100) yield break;

        while(true)
        {
            yield return Fade(100, fadeSpeed);
            if (usableFMaterial.color.a == 100)
            {
                SwapActiveMaterial(baseMaterial);
                yield break;
            }
        }
        
    }
    

    public IEnumerator Fade(float targetAlpha, float speed)
    {
        targetAlpha /= 100;
        if (targetAlpha == 0)faded = true;
        if (targetAlpha == usableFMaterial.color.a) 
        {
            if (targetAlpha == 1)
                SwapActiveMaterial(baseMaterial);
            yield break;
            
        }
        
        if (currentMaterial != usableFMaterial) SwapActiveMaterial(usableFMaterial);
        while (usableFMaterial.color.a != targetAlpha)
        {
            var newAlpha = Mathf.MoveTowards(usableFMaterial.color.a, targetAlpha, (speed * Time.deltaTime));
            usableFMaterial.color = new Color(usableFMaterial.color.r, usableFMaterial.color.g, usableFMaterial.color.b, newAlpha);
            yield return null;
        }

        
    }

    private void SwapActiveMaterial(Material newMat)
    {
        newMat.SetFloat("_Glossiness", GetComponent<Renderer>().material.GetFloat("_Glossiness"));
        newMat.SetFloat("_Metallic", GetComponent<Renderer>().material.GetFloat("_Metallic"));
        currentMaterial = newMat;
        GetComponent<Renderer>().material = currentMaterial;
    }
}
