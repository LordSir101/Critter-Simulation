using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnvironmentManager : MonoBehaviour
{
    public Vector3 mapSize;
    // Start is called before the first frame update
    void Start()
    {
        mapSize = GameObject.FindGameObjectWithTag("PlayableArea").GetComponent<Tilemap>().size;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
