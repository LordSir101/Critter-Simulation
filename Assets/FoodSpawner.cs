using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public class FoodSpawner : MonoBehaviour
{
    private float lastSpawnTime;
    private float spawnInterval = 10;

    private int numFoodToSpawn = 80;
    private int buffer = 3;
    public GameObject food;
    public GameObject movementTarget;

    private Vector3 mapSize;

    Dictionary<int,Color> colors = new Dictionary<int, Color>(){
            {0, new Color32(219, 59, 55, 255 )}, //red
            {1, new Color32(26, 184, 217, 255 )}, //blue
            {2, new Color32(46, 154, 34, 255 )}, //green
            {3, new Color32(255, 230, 52, 255 )} //gold
        };
 
    //Object Pooling
    public static FoodSpawner SharedInstance;
    public List<GameObject> pooledFood;
    public List<GameObject> pooledMovementTargets;
    public int amountToPool = 200;

    void Awake()
    {
        SharedInstance = this;
    }
    void Start()
    {
        pooledFood = new List<GameObject>();
        GameObject tmp;
        for(int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(food);
            tmp.SetActive(false);
            pooledFood.Add(tmp);
        }

        pooledMovementTargets = new List<GameObject>();
        for(int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(movementTarget);
            tmp.SetActive(false);
            pooledMovementTargets.Add(tmp);
        }

        mapSize = LogicManager.SharedInstance.mapSize;
        SpawnFood();
    }

    void Update()
    {
        if(Time.time - lastSpawnTime >= spawnInterval){
            SpawnFood();
        }
    }

    public GameObject GetPooledFood()
    {
        for(int i = 0; i < amountToPool; i++)
        {
            if(!pooledFood[i].activeInHierarchy)
            {
                return pooledFood[i];
            }
        }
        return null;
    }

    public GameObject GetPooledMovementTarget()
    {
        for(int i = 0; i < amountToPool; i++)
        {
            if(!pooledMovementTargets[i].activeInHierarchy)
            {
                return pooledMovementTargets[i];
            }
        }
        return null;
    }

    private void SpawnFood(){
        lastSpawnTime = Time.time;
        int width = (int)mapSize[0] / 2;
        int height = (int)mapSize[1] / 2;

        for(int i = 0; i < numFoodToSpawn; i++){
            int xpos = UnityEngine.Random.Range(-width + buffer, width - buffer);
            int ypos = UnityEngine.Random.Range(-height + buffer, height - buffer);

            GameObject newFood = SharedInstance.GetPooledFood(); 
            if (newFood != null) {
                newFood.transform.position = new Vector3(xpos,ypos,0);
                newFood.transform.rotation = transform.rotation;
                
                int num = UnityEngine.Random.Range(0,4);
                newFood.GetComponent<Food>().energyValue = (num +1) * 8;
                newFood.GetComponent<Light2D>().color = colors[num];
                newFood.GetComponent<Light2D>().pointLightOuterRadius = 1+(0.1f*num);
                newFood.transform.localScale = new Vector3(1+(0.2f*num),1+(0.2f*num),0);
                newFood.SetActive(true);
            }
        }
    }

    public GameObject getMovementTarget(GameObject critter, Vector3 critterPos)
    {
        int[] directions = {-1,1};
        float xCoord = UnityEngine.Random.Range(8,20) * directions[UnityEngine.Random.Range(0,2)];
        float yCoord = UnityEngine.Random.Range(8,20) * directions[UnityEngine.Random.Range(0,2)];

        // movement target spawns at a point 8-20 units from where the critter is
        Vector3 targetPos = critterPos + new Vector3(xCoord, yCoord,0);

        // prevent target from spawning outside map
        targetPos = new Vector3(
            Mathf.Clamp(targetPos.x, (-mapSize.x / 2) + buffer, (mapSize.x / 2) - buffer),
            Mathf.Clamp(targetPos.y, (-mapSize.y / 2) + buffer, (mapSize.y / 2) - buffer),
            0
        );

        GameObject movementTarget = SharedInstance.GetPooledMovementTarget(); 
        if (movementTarget != null) {
            movementTarget.transform.position = targetPos;
            movementTarget.transform.rotation = transform.rotation;
            movementTarget.GetComponent<MovementTarget>().critter = critter;
            movementTarget.SetActive(true);
            movementTarget.GetComponent<MovementTarget>().StartTimer();
        }
        return movementTarget;

    }
}
