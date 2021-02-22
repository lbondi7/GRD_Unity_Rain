using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MaterialData))]

public class Friction : MonoBehaviour
{

    private Collider col;

    private MaterialData mat;
    
    
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider>();
        mat = GetComponent<MaterialData>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(mat.Wetness);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        float drag = ((1.0f - mat.Wetness) + mat.Roughness + (col.material.dynamicFriction * other.gameObject.GetComponent<Collider>().material.dynamicFriction)) / 4.0f;
        rb.angularDrag = drag;
        Debug.Log(drag);
    }

    private void OnCollisionExit(Collision other)
    {
        Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
        rb.angularDrag = 0.05f;
    }
}
