using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum UpgradeType
{
    statsUpgrade,
    defense
}
[CreateAssetMenu]
public class Upgrade : ScriptableObject
{
    public UpgradeType upgradeType;
    public string upgradeName;
    public Sprite icon;

    public String description;
}
