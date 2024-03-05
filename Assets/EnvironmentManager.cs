using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class EnvironmentManager : MonoBehaviour
{
    public Vector3 mapSize;
    private Button visionToggle;
    public bool showLines = false;

    public int day = 1;
    float timeElapsed;

    private int carnivoreSpawnInterval = 3;

    private bool temp = true;

    [SerializeField] private GameObject critterManager;
    // Start is called before the first frame update
    void Start()
    {
        mapSize = GameObject.FindGameObjectWithTag("PlayableArea").GetComponent<Tilemap>().size;

        visionToggle = GameObject.FindGameObjectWithTag("VisionToggle").GetComponent<Button>();
        visionToggle.onClick.AddListener(ToggleShowLines);

        timeElapsed = Time.time;
        
    }

    // Update is called once per frame
    void Update()
    {
        // if(temp)
        // {
        //     SpawnCarnivores();
        //     temp = false;
        // }

        if(Time.time - timeElapsed >= 60)
        {
            day++;
            timeElapsed = Time.time;

            if(day % carnivoreSpawnInterval == 0)
            {
                SpawnCarnivores();
            }
        }
    }
    void ToggleShowLines()
    {
        showLines = !showLines;
    }

    void SpawnCarnivores()
    {
        int numCarnivoreWaves = day / carnivoreSpawnInterval;
        int numCarnivoresToSpawn = 6 + numCarnivoreWaves / 2;
        int sizeOfCarnivore = 6 + numCarnivoreWaves / 2;


        int speed = UnityEngine.Random.Range(1,sizeOfCarnivore -2);
        int sense = UnityEngine.Random.Range(1,sizeOfCarnivore - speed);
        int breed = UnityEngine.Random.Range(1,sizeOfCarnivore - speed - sense);
        int currSize = speed + sense + breed;

        // randomly add 1 stat until the size is at min size for the wave
        while(currSize < sizeOfCarnivore)
        {
            int[]stats = {speed, sense, breed};
            stats[UnityEngine.Random.Range(0,3)]++;
            speed = stats[0];
            sense = stats[1];
            breed = stats[2];
            currSize = speed + sense + breed;
        }

        critterManager.GetComponent<CritterManager>().SpawnCarnivores(speed, sense, breed, numCarnivoresToSpawn);

    }
}
