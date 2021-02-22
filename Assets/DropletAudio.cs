using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropletAudio : MonoBehaviour
{
    string Wood = "event:/Impacts/Wood";
    string Rock = "event:/Impacts/Rock";
    string Water = "event:/Impacts/Water";
    string Grass = "event:/Impacts/Grass";

    string Material;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //PlaySound();
    }

    void PlaySound()
    {
        Vector3 newPos = new Vector3(7.78f, 11.68f, -5.5f);
        FMODUnity.RuntimeManager.PlayOneShot(Wood, newPos);
    }
    
    public void PlaySound(Vector3 pos, int materialID)
    {
        string mat;
        switch (materialID)
        {
            case 0:
                mat = Wood;
                break;
            case 1:
                mat = Rock;
                break;
            case 2:
                mat = Water;
                break;
            case 3:
                mat = Grass;
                break;
            default:
                mat = "none";
                break;
        }

        if (mat != "none")
        {
            FMODUnity.RuntimeManager.PlayOneShot(mat, pos);
            Debug.Log("Sound");
        }
    }
}
