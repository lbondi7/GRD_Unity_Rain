using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterVolume : MonoBehaviour
{
    [Header("Objects")]
    public GameObject WaterPlane;
    public GameObject HeightMarker;
    public GameObject Log;
    [Header("Behaviour")]
    [Range(0.1f, 1f)]
    public float heightIncrement = 0.25f;

    private void OnParticleCollision(GameObject other)
    {
        WaterPlane.transform.position = Vector3.MoveTowards(WaterPlane.transform.position, HeightMarker.transform.position, heightIncrement);
        Log.transform.position = Vector3.MoveTowards(Log.transform.position, HeightMarker.transform.position, heightIncrement);
    }

}
