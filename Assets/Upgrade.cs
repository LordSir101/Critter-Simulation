using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum UpgradeType
{
    statsUpgrade,
    defense
}

public enum StatToUpgrade
{
    speedScale,
    senseScale,
    breedScale,
    energyUsageInterval,
    breedInterval,
    maxChildren,
    amountOfEnergyToSteal,
    none
}


[CreateAssetMenu]
public class Upgrade : ScriptableObject
{
    public Sprite icon;
    public string upgradeName;
    public UpgradeType upgradeType;
    public StatToUpgrade statToUpgrade;
    public float upgradeValue;
    public int cost = 0;

    public String description;

    public void Apply(GameObject obj)
    {
        if(upgradeType == UpgradeType.statsUpgrade)
        {
            UpgradeStats(obj);
        }
    }

    void UpgradeStats(GameObject obj)
    {
        Critter critter = obj.GetComponent<Critter>();
        critter[statToUpgrade.ToString()] += upgradeValue;
    }
}
