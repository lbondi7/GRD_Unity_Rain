using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WettableSurface : MonoBehaviour
{
    // TODO: Make new function that goes in reverse to make object "Drier" over time.
    public WettableSurface()
    {
        this.wetness = 0;
        this.wetnessThresholds = new List<int>();
        this.baseReflectiveness = 0;
        this.wetReflectiveness = new List<float>();
    }
    public WettableSurface(int _wetness,
                    List<int> _wetnessThresholds,
                    Material _material,
                    float _baseReflectiveness,
                    List<float> _wetReflectiness)
    {
        this.wetness = _wetness;
        this.wetnessThresholds = _wetnessThresholds;
        this.material = _material;
        this.baseReflectiveness = _baseReflectiveness;
        this.wetReflectiveness = _wetReflectiness;
    }

    /* Amount of frames the object has been under rain */
    [SerializeField] protected int wetness;

    /* How wet the object needs to be to trigger a change */
    [SerializeField] protected List<int> wetnessThresholds = new List<int>();

    [SerializeField] protected Material material;

    /* How reflective the object is at the start. */
    [SerializeField] protected float baseReflectiveness;
    /* How reflective the object will get at each stage of wetness. 1-1000. */
    [SerializeField] protected List<float> wetReflectiveness = new List<float>();

    [SerializeField] private int wetnessState = 0;
    private float rainLerp;


    protected void InitWetObject()
    {
        material = GetComponent<Renderer>().material;
    }

    private void OnParticleCollision(GameObject other)
    {
        if (wetnessState != wetnessThresholds.Count)
        {
            DetectRainConditions(other);
        }
    }
    
    private void DetectRainConditions(GameObject _pSys)
    {
        if (_pSys.CompareTag("Rain"))
        {
            ++wetness;
            if (wetness > wetnessThresholds[wetnessThresholds.Count-1])
                wetness = wetnessThresholds[wetnessThresholds.Count - 1];

            for (int i = 0; i < wetnessThresholds.Count; i++)
            {
                if (wetness == wetnessThresholds[i])
                {
                    //rainLerp = 0.1f;
                    if (wetnessState < wetnessThresholds.Count - 1)
                        wetnessState = i;
                }
            }
            ApplyRainEffect();
        }
    }

    protected virtual void ApplyRainEffect()
    {
        material.SetFloat("_Metallic", (wetReflectiveness[wetnessState] / 1000));
        material.SetFloat("_Glossiness", (wetReflectiveness[wetnessState] / 1000));
        //rainLerp++;
        //if (wetnessState == wetnessThresholds.Count - 1)
        //{
        //    material.SetFloat("_Metallic", wetReflectiveness[wetnessState] / 1000);
        //    return;
        //}
        //var p1 = wetReflectiveness[wetnessState];
        //var p2 = wetReflectiveness[wetnessState + 1];
        //var x = (Mathf.Lerp(p1,p2, Time.deltaTime / rainLerp) / 1000);
        //print(x);
        //material.SetFloat("_Metallic", x);
               
    }
}
