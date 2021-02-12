using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FillType
{
    Timed, //After first rain hits, start to fill
    Progressive, //As rain hits, fill it up

}

public enum FillSpeed
{
    VerySlow,
    Slow,
    Normal,
    Fast,
    VeryFast
}

public class WaterVolume : MonoBehaviour
{
    [Header("Objects")]
    public GameObject WaterPlane;
    public GameObject HeightMarker;
    public List<GameObject> floatables;
    [Header("Behaviour")]
    public FillType fillType = FillType.Progressive;
    public FillSpeed fillSpeed = FillSpeed.VerySlow;
    public bool willDrain = false;

    bool isActive = false;
    Vector3 startingPos;

    private void Awake()
    {
        startingPos = WaterPlane.transform.position;
        if (willDrain)
        {
            InvokeRepeating("drain", 1f, 1f);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (isActive)
        {
            return;
        }
        switch (fillType)
        {
            case FillType.Timed:
                isActive = true;
                break;
            case FillType.Progressive:
                WaterPlane.transform.position = Vector3.MoveTowards(WaterPlane.transform.position, HeightMarker.transform.position, getFillSpeed());
                raiseFloatables();
                checkWaterState();
                break;
            default:
                break;
        }

    }

    void raiseFloatables() 
    {
        foreach (GameObject item in floatables)
        {
            item.transform.position = Vector3.MoveTowards(item.transform.position, HeightMarker.transform.position, getFillSpeed()); ;
        }
    }

    float getFillSpeed() 
    {
        switch (fillSpeed)
        {
            case FillSpeed.VerySlow:
                return 0.0001f;
            case FillSpeed.Slow:
                return 0.001f;
            case FillSpeed.Normal:
                return 0.005f;
            case FillSpeed.Fast:
                return 0.01f;
            case FillSpeed.VeryFast:
                return 0.05f;
            default:
                return 1;
        }
    }

    float getDrainSpeed() 
    {
        switch (fillSpeed)
        {
            case FillSpeed.VerySlow:
                return 0.0005f;
            case FillSpeed.Slow:
                return 0.005f;
            case FillSpeed.Normal:
                return 0.001f;
            case FillSpeed.Fast:
                return 0.01f;
            case FillSpeed.VeryFast:
                return 0.05f;
            default:
                return 1;
        }
    }
    void checkWaterState() 
    {
        if (Vector3.Distance(WaterPlane.transform.position,HeightMarker.transform.position) < 0.1)
        {
            WaterPlane.GetComponent<BoxCollider>().enabled = true;
            WaterPlane.layer = 4;
        }
        else
        {
            WaterPlane.GetComponent<BoxCollider>().enabled = false;
            WaterPlane.layer = 1;
        }
    }

    void drain() 
    {
        WaterPlane.transform.position = Vector3.MoveTowards(startingPos, WaterPlane.transform.position, getDrainSpeed());
    }

}
