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
    private int dayLength = 60;

    private int carnivoreSpawnInterval = 3;
    private int firstWave = 2;

    private int upgradeInterval;
    private int firstUpgrade;

    private bool firstWaveSpawned = false;

    //[SerializeField] private GameObject critterManager;
    [SerializeField] private GameObject uiManager;

    public static EnvironmentManager SharedInstance;

    void Awake()
    {
        SharedInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // upgrades happen after every carnivore wave
        upgradeInterval = carnivoreSpawnInterval;
        firstUpgrade = firstWave + 1;

        mapSize = GameObject.FindGameObjectWithTag("PlayableArea").GetComponent<Tilemap>().size;

        visionToggle = GameObject.FindGameObjectWithTag("VisionToggle").GetComponent<Button>();
        visionToggle.onClick.AddListener(ToggleShowLines);

        uiManager.GetComponent<UIManager>().setDay(day);

        timeElapsed = Time.time;
        
    }

    // Update is called once per frame
    void Update()
    {
        // if(day == 1 && !firstWaveSpawned )
        // {
        //     SpawnCarnivores();
        //     firstWaveSpawned = true;
        // }

        if(Time.time - timeElapsed >= dayLength)
        {
            day++;
            timeElapsed = Time.time;

            uiManager.GetComponent<UIManager>().setDay(day);

            SpawnCarnivores();
            StartUpgrade();
        
        }
    }
    void ToggleShowLines()
    {
        showLines = !showLines;
    }

    
    void StartUpgrade()
    {
        if((day - firstUpgrade) % upgradeInterval == 0)
        {
            gameObject.GetComponent<UpgradeManager>().Upgrade(uiManager.GetComponent<UpgradePanelManager>());
        }
        
        //gameObject.GetComponent<UpgradeManager>().Upgrade(uiManager.GetComponent<UpgradePanelManager>());
        
    }

    void SpawnCarnivores()
    {
        if((day - firstWave) % carnivoreSpawnInterval == 0)
        {
            int numCarnivoreWaves = (day - firstWave) / carnivoreSpawnInterval + 1;
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

            int energytoSpawnWith = 80 + 15*day;

            CritterManager.SharedInstance.SpawnCarnivores(speed, sense, breed, energytoSpawnWith,numCarnivoresToSpawn);
        }

    }
}
