using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainTrigger : MonoBehaviour
{
    public GameObject indicator;
    private void OnParticleCollision(GameObject other)
    {
        print("Collided!");
    }
}
