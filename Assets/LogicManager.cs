using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class LogicManager : MonoBehaviour
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

    [SerializeField] private GameObject uiManager;
    [SerializeField] private GameEndPanelManager gameEndPanel;

    public static LogicManager SharedInstance;

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

        uiManager.GetComponent<UIManager>().SetDay(day);

        timeElapsed = Time.time;

        PauseControl.PauseGame(false);
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

            uiManager.GetComponent<UIManager>().SetDay(day);

            SpawnCarnivores();
            StartUpgrade();
        
        }
    }
    public void ToggleShowLines()
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
        // Carnivore waves and size get progressively larger as the days go by
        if((day - firstWave) % carnivoreSpawnInterval == 0)
        {
            int numCarnivoreWaves = (day - firstWave) / carnivoreSpawnInterval + 1;
            int numCarnivoresToSpawn = 6 + numCarnivoreWaves / 2;
            int sizeOfCarnivore = 6 + numCarnivoreWaves / 2;
            int energytoSpawnWith = 80 + 15*day;

            CritterManager.SharedInstance.SpawnCarnivores(sizeOfCarnivore, energytoSpawnWith,numCarnivoresToSpawn);
        }

    }

    public void EndGame()
    {
        gameEndPanel.OpenPanel();
    }
}
