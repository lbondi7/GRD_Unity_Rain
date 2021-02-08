using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RainShape
{
    Box,
    Cylinder,
    Plane
}

public class BasicRain : MonoBehaviour
{
    public RainShape Shape = RainShape.Box;
    [Header("Size of Rain Source")]
    [Range(0, 10)]
    public int RainSize = 2;
    [Header("Cloud")]
    public GameObject CloudObject;
    public bool SpawnCloud = true;

    GameObject cloud;
    int defaultHeight = 4;
    protected GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        if (CloudObject != null)
        {
            if (SpawnCloud)
            {
                cloud = Instantiate(CloudObject, transform);
            }
        }
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OnValidate()
    {
        var rainData = GetComponent<ParticleSystem>().shape;
        switch (Shape)
        {
            case RainShape.Box:
                rainData.shapeType = ParticleSystemShapeType.Box;
                break;
            case RainShape.Cylinder:
                rainData.shapeType = ParticleSystemShapeType.Cone;
                rainData.angle = 0;
                rainData.length = 1;
                break;
            case RainShape.Plane:
                break;
            default:
                break;
        }
        rainData.scale = new Vector3(RainSize, RainSize, 1);
        CloudConfig();
    }

    void CloudConfig() 
    {
        if (cloud != null)
        {
            if (!cloud.GetComponent<ParticleSystem>())
            {
                return;
            }
            var cloudData = cloud.GetComponent<ParticleSystem>().shape;
            cloudData.radius = RainSize;
        }


    }

    void RainConfig() 
    {
        if (transform.position.y > 0)
        {
            transform.position.Set(transform.position.x, defaultHeight, transform.position.z);
        }
    }
}
