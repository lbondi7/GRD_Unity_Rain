using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class WettableSurface : WettableObject
{
  

    /* How reflective the object is at the start. */
    protected float baseReflectiveness;
    /* How reflective the object will get at each stage of wetness. 1-1000. */
    public List<float> thresholdReflectiveness = new List<float>();
    public bool reflectionsUsed;
    public bool fadingUsed;
    [HideInInspector] public WetnessState fadingTrigger;
    [HideInInspector] public float fadingSpeedMultiplier;
    public List<float> thresholdVisibility = new List<float>();
    public float outOfRainFadeStrength;
    public float submergedFadeStrength;

    void Start()
    {
    }

    override protected void ApplyRainEffect()
    {
        if (!baseMaterial) baseMaterial =GetComponent<Renderer>().material;
        baseMaterial.SetFloat("_Metallic", (thresholdReflectiveness[currentWetnessLevel] / 1000));
        baseMaterial.SetFloat("_Glossiness", (thresholdReflectiveness[currentWetnessLevel] / 1000));

        if (fadingUsed)
        {
            FadeOnConditions();
        }
    }

    void FadeOnConditions()
    {
        switch (fadingTrigger)
        {
            case WetnessState.OutOfRain:
                if (CurrentWetState == WetnessState.OutOfRain)
                {
                    StartCoroutine(Fade(outOfRainFadeStrength, 1 * fadingSpeedMultiplier));
                    break;
                }
                else
                {
                    if (faded)
                        StartCoroutine(FadeIn(1 * fadingSpeedMultiplier));
                    break;
                }
            case WetnessState.InRain:
                if (CurrentWetState == WetnessState.InRain)
                {
                    StartCoroutine(Fade(thresholdVisibility[currentWetnessLevel], 1 * fadingSpeedMultiplier));

                    break;
                }
                else if (CurrentWetState == WetnessState.FullyWet)
                {
                    StartCoroutine(Fade(thresholdVisibility[thresholdVisibility.Count-1], 1 * fadingSpeedMultiplier));
                    break;
                }
                else
                {
                    if (faded)
                        StartCoroutine(FadeIn(1 * fadingSpeedMultiplier));
                    break;
                }
            case WetnessState.FullyWet:
                if (CurrentWetState == WetnessState.FullyWet)
                {
                    if (submergedFadeStrength == 100)
                        StartCoroutine(FadeOut(1 * fadingSpeedMultiplier));
                    else
                    {
                        StartCoroutine(Fade(submergedFadeStrength, 1 * fadingSpeedMultiplier));
                    }
                    break;
                }
                else
                {
                    if (faded)
                    StartCoroutine(FadeIn(1 * fadingSpeedMultiplier));
                    break;
                }
        }
    }
}

