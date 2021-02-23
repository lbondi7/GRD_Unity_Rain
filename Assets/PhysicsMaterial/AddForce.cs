using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForce : MonoBehaviour
{

    [SerializeField] [Range(-20.0f, 20.0f)]private float scalar = 1.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(new Vector3(0.0f, 0.0f, 1.0f * scalar), ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
