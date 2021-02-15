using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RainTrigger : MonoBehaviour
{
    public GameObject Rain;

    private void Update()
    {
        transform.Rotate(0, 0.1f, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        Rain.SetActive(!Rain.activeSelf);
        Destroy(gameObject);
    }


}
