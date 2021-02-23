using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MaterialData : MonoBehaviour
{
    private List<string> materialName = new List<string>();
    private List<Color> materialColours = new List<Color>();
    [SerializeField] [Range(0, 4)] private int id = 0;
    [SerializeField] private bool simulateWetness = true;
    [SerializeField] [Range(0.0f, 1.0f)] private float wetness = 0.9f;
    [SerializeField] [Range(0.0f, 1.0f)] private float roughness = 0.0f;
    [SerializeField] [Range(0.0f, 1.0f)] private float thiccness = 0.9f;
    [SerializeField] private float timeSinceLastWetness = 60.0f;
    [SerializeField] private float dryingTimeMultiplier = 3.0f;


    public int ID { get => id; }
    public float Wetness { 
        get => wetness;
        set => wetness = value;
    }
    public float Roughness { get => roughness; }
    public float Thiccness { get => thiccness; }
    public float TimeSinceLastWetness { 
        get => timeSinceLastWetness;
        set => timeSinceLastWetness = value;
    }

    public bool SimulateWetness
    {
        get => simulateWetness; 
    }

    private int prevID = 0;
    // Start is called before the first frame update
    void Start()
    {
        materialName.Add("Wood");
        materialName.Add("Rock");
        materialName.Add("Grass");
        materialName.Add("Water");
        materialName.Add("Ice");

        materialColours.Add(GetColourNorm(242, 113, 58));
        materialColours.Add(GetColourNorm(122, 58, 9));
        materialColours.Add(GetColourNorm(87, 179, 7));
        materialColours.Add(GetColourNorm(40, 90, 120));
        materialColours.Add(GetColourNorm(191, 224, 224));
        AddDependencies();
    }

    // Update is called once per frame
    void Update()
    {
        if(prevID != id)
            AddDependencies();

        if (simulateWetness)
        {
            timeSinceLastWetness = Mathf.Clamp(timeSinceLastWetness - Time.deltaTime, 0.0f, 60.0f);
            if (timeSinceLastWetness <= 0.0f)
            {
                wetness = Mathf.Clamp(wetness - Time.deltaTime * dryingTimeMultiplier, 0.0f, 1.0f);
            }
        }
    }


    private void AddDependencies()
    {
        if (!GetComponent<Collider>())
        {
            gameObject.AddComponent<BoxCollider>();
        }
        var col = gameObject.GetComponent<BoxCollider>();
        col.sharedMaterial = Resources.Load<PhysicMaterial>("PhysicsMaterials/" + materialName[Mathf.Clamp(id, 0, materialName.Count - 1)]);
        if (!GetComponent<MeshFilter>())
        {
            gameObject.AddComponent<MeshFilter>();
            GetComponent<MeshFilter>().mesh = Resources.Load<Mesh>("Library/unity default resources/Cube");
        }
        
        if (!GetComponent<MeshRenderer>())
        {
            gameObject.AddComponent<MeshRenderer>();
        }
        if(!GetComponent<Renderer>().material)
            GetComponent<Renderer>().material = new Material(Shader.Find("Standard"));
        
        var mat = GetComponent<MeshRenderer>().material;
        mat.color = materialColours[Mathf.Clamp(id, 0, materialColours.Count - 1)];
        GetComponent<MeshRenderer>().material = mat;
        prevID = id;
    }

    Color GetColourNorm(float r, float g, float b)
    {
        return new Color(r/ 255.0f, g/255.0f, b / 255.0f);
    }


}
