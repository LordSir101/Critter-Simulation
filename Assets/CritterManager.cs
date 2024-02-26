using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CritterManager : MonoBehaviour
{
    public critterBuilder critterBuilder;
    public GameObject critterTemplate;

    private int numInitialSpecies = 4;
    private int numInitialCrittersToSpawn = 8;

    // Tilemap
    private Vector3 mapSize;
    int buffer;
    int width;
    int height;

    int crittersAlive = 0;

    float lastCheck;

    private Dictionary<int,Color> colors = new Dictionary<int, Color>(){
            {1, new Color32(207, 13, 13, 255 )}, //red
            {2, new Color32(26, 184, 217, 255 )}, //blue
            {3, new Color32(46, 154, 34, 255 )}, //green
            {4, new Color32(255, 230, 52, 255 )} //gold
        };
    void Start()
    {
        // Create the player made critters
        mapSize = GameObject.FindAnyObjectByType<EnvironmentManager>().GetComponent<EnvironmentManager>().mapSize;
        buffer = 5;
        width = (int)mapSize[0] / 2;
        height = (int)mapSize[1] / 2;

        for(int i = 0; i < numInitialCrittersToSpawn; i++){
            CritterBirth(MenuInput.speed, MenuInput.sense, MenuInput.breed, 1, colors[1]);
        }

        lastCheck = Time.time;

        // Generate random critters to populate the world
        for(int i = 2; i <=numInitialSpecies; i++){
            (int speed, int sense, int breed) = GenerateRandomCritter();
            for(int j = 0; j < numInitialCrittersToSpawn; j++)
            {
                CritterBirth(speed, sense, breed, i, colors[i]);
            }
        }
        
    }

    void Update()
    {
        // if(Time.time - lastCheck >= 1){
        //     Debug.Log(crittersAlive);
        // }
    }

    (int,int,int) GenerateRandomCritter()
    {
        //random starting stats.  Maximum of 9 stats total
        //each stat is guaranteed a chance at having a value of 2 or more
        //each stat will be at least 1;
        int speed = UnityEngine.Random.Range(0,5);
        int sense = UnityEngine.Random.Range(0,7 - speed);
        int breed = UnityEngine.Random.Range(0,9 - speed - sense);
        int size = speed + sense + breed;

        // randomly add 1 stat until the size is at least 3
        while(size < 3)
        {
            int[]stats = {speed, sense, breed};
            stats[UnityEngine.Random.Range(0,3)]++;
            speed = stats[0];
            sense = stats[1];
            breed = stats[2];
            size = speed + sense + breed;
        }
        return (speed, sense, breed);
    }
    public void CritterBirth(int speed, int sense, int breed, int speciesNum, Color color)
    {

        int xoffset = UnityEngine.Random.Range(-width + buffer, width - buffer);
        int yoffset = UnityEngine.Random.Range(-height + buffer, height - buffer);

        GameObject critter = Instantiate(critterTemplate, new Vector3(transform.position.x + xoffset, transform.position.y + yoffset, 0), transform.rotation);

        critter.GetComponent<Critter>().speed = speed;
        critter.GetComponent<Critter>().sense = sense;
        critter.GetComponent<Critter>().breed = breed;
        critter.GetComponent<Critter>().speciesNum = speciesNum;
        critter.GetComponent<Critter>().color = color;
        critterBuilder.CreateCritter(speed, sense, breed, critter);
        crittersAlive++;
    }

    public void CritterDeath(GameObject critter)
    {
        crittersAlive--;
        Destroy(critter);
    }

}
