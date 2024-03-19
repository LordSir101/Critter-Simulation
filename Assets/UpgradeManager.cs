using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] List<Upgrade> upgradePool;
    private List<Upgrade> selectedUpgrades;
    private List<GameObject> crittersToUpgrade;
    [SerializeField] private SpeciesKnowledgePoints speciesKnowledgePoints;

    Dictionary<int, List<Upgrade>> speciesAquiredUpgrades = new Dictionary<int, List<Upgrade>>(){
            {0, new List<Upgrade>()}, 
            {1, new List<Upgrade>()}, 
            {2, new List<Upgrade>()}, 
            {3, new List<Upgrade>()} 
        };

    // Start upgrade process
    public void Upgrade(UpgradePanelManager upgradeUI)
    {
        if(selectedUpgrades == null) {selectedUpgrades = new List<Upgrade>();}
        selectedUpgrades.Clear();
        selectedUpgrades.AddRange(GetSelectableUpgrades(3));
        upgradeUI.ShowUpgradeMenu(selectedUpgrades);
    }

    //TODO: make this function abel to apply upgrades to any species
    public bool ApplyUpgrade(int selectedButtonID)
    {
        Upgrade selectedUpgrade = selectedUpgrades[selectedButtonID];

        int knowledge = speciesKnowledgePoints.GetKnowledgeOfSpecies(PlayerGameInfo.currSpeciesNum);

        if(selectedUpgrade.cost > knowledge)
        {
            return false;
        }
        else
        {
            // Apply upgrade to all critters in the pool
            crittersToUpgrade = CritterManager.SharedInstance.GetAllPooledCritters(PlayerGameInfo.currSpeciesNum);
            for(int i = 0; i < crittersToUpgrade.Count; i++)
            {
                selectedUpgrade.Apply(crittersToUpgrade[i]);
            }
            
            speciesAquiredUpgrades[PlayerGameInfo.currSpeciesNum].Add(selectedUpgrade);

            speciesKnowledgePoints.UseKnowledgePoints(PlayerGameInfo.currSpeciesNum, selectedUpgrade.cost);

            return true;
        }
    }

    public List<Upgrade> GetSelectableUpgrades(int count)
    {
        List<Upgrade> upgrades = new List<Upgrade>();

        if(count > upgradePool.Count)
        {
            count = upgradePool.Count;
        }
        for(int i = 0; i < count; i++)
        {
            upgrades.Add(upgradePool[Random.Range(0, upgradePool.Count)]);
        }
        return upgrades;
    }

    private void UpdateAquiredUpgradesTable()
    {
        // track upgrades for any new species that have been created
        foreach(KeyValuePair<int, int> entry in CritterManager.SharedInstance.speciesCount)
        {
            if(!speciesAquiredUpgrades.ContainsKey(entry.Key))
            {
                speciesAquiredUpgrades[entry.Key] = new List<Upgrade>();
            }
        }

        // stop tracking upgrades for species that have died
        foreach(KeyValuePair<int, List<Upgrade>> entry in speciesAquiredUpgrades)
        {
            if(!CritterManager.SharedInstance.speciesCount.ContainsKey(entry.Key))
            {
                speciesAquiredUpgrades.Remove(entry.Key);
            }
        }
    }
}
