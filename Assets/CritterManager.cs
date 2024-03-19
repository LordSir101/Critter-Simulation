using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CritterManager : MonoBehaviour
{
    public CritterBuilder critterBuilder;
    [SerializeField] private GameObject critterTemplate;
    [SerializeField] private GameObject carnivoreTemplate;
    [SerializeField] private UIManager uIManager;
    [SerializeField] private SpeciesKnowledgePoints speciesKnowledgePoints;

    private int numInitialSpecies = 4;
    private int numSpeciesExisted = 4;
    private int numInitialCrittersToSpawn = 8;

    private int MAX_NUM_SPECIES = 10;

    // Tilemap
    private Vector3 mapSize;
    int buffer;
    int width;
    int height;

    //Critter pool
    private int amountToPool = 100;
    public static CritterManager SharedInstance;


    public Dictionary<int, int> speciesCount = new Dictionary<int, int>(){
            {0, 0}, 
            {1, 0}, 
            {2, 0}, 
            {3, 0} 
        };

    public Dictionary<int,Color> colors = new Dictionary<int, Color>(){
            {0, new Color32(207, 13, 13, 255 )}, //red
            {1, new Color32(26, 184, 217, 255 )}, //blue
            {2, new Color32(46, 154, 34, 255 )}, //green
            {3, new Color32(255, 230, 52, 255 )} //gold
        };

    public Dictionary<int, List<GameObject>> critterPoolPool = new Dictionary<int, List<GameObject>>(){
            {0, new List<GameObject>()}, 
            {1, new List<GameObject>()}, 
            {2, new List<GameObject>()}, 
            {3, new List<GameObject>()} 
        };
    void Awake()
    {
        SharedInstance = this;
    }
    void Start()
    {
        GameObject tmp;
        foreach(KeyValuePair<int, List<GameObject>> entry in critterPoolPool)
        {
            for(int i = 0; i < amountToPool; i++)
            {
                tmp = Instantiate(critterTemplate);
                tmp.SetActive(false);
                entry.Value.Add(tmp);
            }
        }

        // Create the player made critters
        mapSize = LogicManager.SharedInstance.mapSize;
        buffer = 5;
        width = (int)mapSize[0] / 2;
        height = (int)mapSize[1] / 2;

        for(int i = 0; i < numInitialCrittersToSpawn; i++){
            CritterBirth(MenuInput.speed, MenuInput.sense, MenuInput.breed, 0, colors[0], critterTemplate);
        }

        // Generate random critters to populate the world
        for(int i = 1; i < numInitialSpecies; i++){

            (int speed, int sense, int breed) = GenerateRandomCritter(3,9);

            for(int j = 0; j < numInitialCrittersToSpawn; j++)
            {
                CritterBirth(speed, sense, breed, i, colors[i], critterTemplate);
            }
        }
        
    }

    public GameObject GetPooledCritter(int speciesNum)
    {
        List<GameObject> pooledCritters = critterPoolPool[speciesNum];
        for(int i = 0; i < amountToPool; i++)
        {
            if(!pooledCritters[i].activeInHierarchy)
            {
                return pooledCritters[i];
            }
        }
        return null;
    }

    public (int,int,int) GetPooledCritterStats(int speciesNum)
    {
        Critter critter = critterPoolPool[speciesNum][0].GetComponent<Critter>();
        return (critter.speed, critter.sense, critter.breed);
    }

    public List<GameObject> GetAllPooledCritters(int speciesNum)
    {
        return critterPoolPool[speciesNum];
    }

    (int,int,int) GenerateRandomCritter(int minSize, int maxSize)
    {
        //random starting stats.  Maximum of 9 stats total
        //each stat is guaranteed a chance at having a value of 2 or more
        //each stat will be at least 1;
        // int speed = UnityEngine.Random.Range(0,6);
        // int sense = UnityEngine.Random.Range(0,8 - speed);
        // int breed = UnityEngine.Random.Range(0,10 - speed - sense);

        int speed = UnityEngine.Random.Range(0, maxSize - (int) Math.Floor(maxSize / 3d));
        int sense = UnityEngine.Random.Range(0, maxSize - speed);
        int breed = UnityEngine.Random.Range(0, maxSize - speed - sense);
        int size = speed + sense + breed;

        // randomly add 1 stat until the size is at least 3
        while(size < minSize)
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
    public GameObject CritterBirth(int speed, int sense, int breed, int speciesNum, Color color, GameObject prefab, int energyToSpawnWith=80)
    {

        int xoffset = UnityEngine.Random.Range(-width + buffer, width - buffer);
        int yoffset = UnityEngine.Random.Range(-height + buffer, height - buffer);

        //GameObject critter = Instantiate(prefab, new Vector3(transform.position.x + xoffset, transform.position.y + yoffset, 0), transform.rotation);
        GameObject critter = SharedInstance.GetPooledCritter(speciesNum); 
        if (critter  != null) {
            critter.transform.position = new Vector3(transform.position.x + xoffset, transform.position.y + yoffset, 0);
            critter.transform.rotation = transform.rotation;
            critter.SetActive(true);
            critter.GetComponent<Critter>().speed = speed;
            critter.GetComponent<Critter>().sense = sense;
            critter.GetComponent<Critter>().breed = breed;
            critter.GetComponent<Critter>().speciesNum = speciesNum;
            critter.GetComponent<CritterInformationDisplay>().color = color;
            critter.GetComponent<Critter>().energy = energyToSpawnWith;
            critterBuilder.CreateCritterSprite(speed, sense, breed, critter);

            speciesCount[speciesNum]++;
        }
        return critter;
    }

    public void CritterDeath(GameObject critter)
    {
        if(critter.activeInHierarchy)
        {
            speciesCount[critter.GetComponent<Critter>().speciesNum]--;

            // If the species is extinct, remove it from tables
            if(speciesCount[critter.GetComponent<Critter>().speciesNum] == 0)
            {
                if(critter.GetComponent<Critter>().speciesNum == PlayerGameInfo.currSpeciesNum)
                {
                    LogicManager.SharedInstance.EndGame();
                }
                speciesCount.Remove(critter.GetComponent<Critter>().speciesNum);
                critterPoolPool.Remove(critter.GetComponent<Critter>().speciesNum);
                speciesKnowledgePoints.RemoveSpecies(critter.GetComponent<Critter>().speciesNum);
                // Update the species count UI to reflect that a species is dead
                uIManager.BuildCritterCountUI();
            }

            critter.SetActive(false);

        }       
    }

    public void SpawnCarnivores(int size, int energyToSpawnWith, int numToSpawn) //int speed, int sense, int breed
    {
        numSpeciesExisted++;
        int newSpeciesNum = numSpeciesExisted;

        (int speed, int sense, int breed) = GenerateRandomCritter(size, size);

        SpawnNewSpecies(newSpeciesNum, numToSpawn, speed, sense, breed, energyToSpawnWith, Color.white, carnivoreTemplate);

        // speciesCount.Add(newSpeciesNum, 0);
        // colors.Add(newSpeciesNum, Color.white);
        // critterPoolPool.Add(newSpeciesNum, new List<GameObject>());

        // GameObject tmp;
        // for(int i = 0; i < amountToPool; i++)
        // {
        //     tmp = Instantiate(carnivoreTemplate);
        //     tmp.SetActive(false);
        //     critterPoolPool[newSpeciesNum].Add(tmp);
        // }

        // for(int i = 0; i < numToSpawn; i++){
        //     CritterBirth(speed, sense, breed, newSpeciesNum, Color.white, carnivoreTemplate, energyToSpawnWith);
        // }

        // uIManager.BuildCritterCountUI();
    }

    public void EvolveFromCritter(GameObject critterToEvolveFrom)
    {
        if(speciesCount.Count >= MAX_NUM_SPECIES)
        {
            return;
        }

        numSpeciesExisted++;
        int newSpeciesNum = numSpeciesExisted;
        // modify a stat of the critter by +/- 1
        Critter critter = critterToEvolveFrom.GetComponent<Critter>();
        int[]stats = {critter.speed, critter.sense, critter.breed};
        int[] amountToChangeOptions = {-1, 1};

        int statToChangeIndex = UnityEngine.Random.Range(0,3);
        int amountToChange = amountToChangeOptions[UnityEngine.Random.Range(0,2)];

        int size = critter.speed+critter.sense + critter.breed;
        if(size == 3)
        {
            amountToChange = 1;
        }
        else if(size == 9)
        {
            amountToChange = -1;
        }
        stats[statToChangeIndex] = stats[statToChangeIndex] + amountToChange;
        if(stats[statToChangeIndex] < 0)
        {
            stats[statToChangeIndex] = 0;
        }

        int newSpeed = stats[0];
        int newSense = stats[1];
        int newBreed = stats[2];    

        Color32 newColor = ModifyColor(critterToEvolveFrom.GetComponent<CritterInformationDisplay>().color);

        // speciesCount.Add(newSpeciesNum, 0);
        // colors.Add(newSpeciesNum, newColor);
        // critterPoolPool.Add(newSpeciesNum, new List<GameObject>());
        SpawnNewSpecies(newSpeciesNum, numInitialCrittersToSpawn, newSpeed, newSense, newBreed, critter.energy /2, newColor, critterTemplate);

        // GameObject tmp;
        // for(int i = 0; i < amountToPool; i++)
        // {
        //     tmp = Instantiate(critterTemplate);
        //     tmp.SetActive(false);
        //     critterPoolPool[newSpeciesNum].Add(tmp);
        // }

        //Debug.Log(newColor);

        // for(int i = 0; i < numInitialCrittersToSpawn; i++)
        // {
        //     CritterBirth(newSpeed, newSense, newBreed, newSpeciesNum, newColor, critterTemplate, critter.energy /2);
        // }

        // // update ui to include new species
        // uIManager.BuildCritterCountUI();
        
    }

    Color32 ModifyColor(Color32 color)
    {
        Color32 oldColor = color;
        int[] oldColorComponents = {oldColor.r, oldColor.g, oldColor.b};
        int mainColor = oldColorComponents.Max(); // saturation
        int mainColorIdx = Array.IndexOf(oldColorComponents, mainColor);
        int rightColorIdx = (mainColorIdx + 1) % 3;
        //int leftColorIdx = (mainColorIdx - 1) - 3 * (int)Math.Floor((double)(mainColorIdx - 1) / 3); // needed since % is remainder not modulo

        int[] direction = {-1,1};

        for(int i = 0; i < 3; i++)
        {
            int dir = direction[UnityEngine.Random.Range(0,2)];
            int amountToModify = i == mainColorIdx ? 20 : 30;

            // The color will slowly trend towards the right on color wheel.  Red->orange->yellow etc
            if(i == rightColorIdx)
            {
                dir = 1;
            }

            // Prevent over/under flow
            if(oldColorComponents[i] + amountToModify * dir < 0)
            {
                dir = 1;
            }
            else if(oldColorComponents[i] + amountToModify * dir > 255)
            {
                dir = -1;
            }
            oldColorComponents[i] = oldColorComponents[i] + amountToModify * dir;
        }

        return new Color32((byte)oldColorComponents[0], (byte)oldColorComponents[1], (byte)oldColorComponents[2], 255);
    }

    void SpawnNewSpecies(int speciesNum, int numToSpawn, int speed, int sense, int breed, int startingEnergy, Color color, GameObject prefab)
    {
        speciesCount.Add(speciesNum, 0);
        colors.Add(speciesNum, color);
        critterPoolPool.Add(speciesNum, new List<GameObject>());

        GameObject tmp;
        for(int i = 0; i < amountToPool; i++)
        {
            tmp = Instantiate(prefab);
            tmp.SetActive(false);
            critterPoolPool[speciesNum].Add(tmp);
        }

        for(int i = 0; i < numToSpawn; i++)
        {
            CritterBirth(speed, sense, breed, speciesNum, color, prefab, startingEnergy);
        }

        // update ui to include new species
        uIManager.BuildCritterCountUI();
    }
}
