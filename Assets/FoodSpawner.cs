using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    private float lastSpawnTime;
    private float spawnInterval = 5;

    private int numFoodToSpawn = 20;
    public GameObject food;
    // Start is called before the first frame update
    void Start()
    {
        SpawnFood();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastSpawnTime >= spawnInterval){
            SpawnFood();
        }
    }

    private void SpawnFood(){
        lastSpawnTime = Time.time;
        for(int i = 0; i < numFoodToSpawn; i++){
                int xpos = UnityEngine.Random.Range(-40,40);
                int ypos = UnityEngine.Random.Range(-40,40);
                Instantiate(food, new Vector3(xpos,ypos,0), transform.rotation);
            }
    }
}
