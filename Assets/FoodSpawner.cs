using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FoodSpawner : MonoBehaviour
{
    private float lastSpawnTime;
    private float spawnInterval = 10;

    private int numFoodToSpawn = 80;
    public GameObject food;

    private Vector3 mapSize;
 
    void Start()
    {
        mapSize = GameObject.FindAnyObjectByType<EnvironmentManager>().GetComponent<EnvironmentManager>().mapSize;
        SpawnFood();
    }

    void Update()
    {
        if(Time.time - lastSpawnTime >= spawnInterval){
            SpawnFood();
        }
    }

    private void SpawnFood(){
        lastSpawnTime = Time.time;
        int buffer = 3;
        int width = (int)mapSize[0] / 2;
        int height = (int)mapSize[1] / 2;
        for(int i = 0; i < numFoodToSpawn; i++){
                int xpos = UnityEngine.Random.Range(-width + buffer, width - buffer);
                int ypos = UnityEngine.Random.Range(-height + buffer, height - buffer);
                Instantiate(food, new Vector3(xpos,ypos,0), transform.rotation);
            }
    }
}
