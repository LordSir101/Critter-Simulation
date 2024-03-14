using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] List<Upgrade> upgradePool;
    private List<Upgrade> selectedUpgrades;

    //TODO: unserialize field when done debugging
    [SerializeField] public List<Upgrade> aquiredUpgrades;

    public void Upgrade(UpgradePanelManager upgradeUI)
    {
        if(selectedUpgrades == null) {selectedUpgrades = new List<Upgrade>();}
        selectedUpgrades.Clear();
        selectedUpgrades.AddRange(GetSelectableUpgrades(3));
        upgradeUI.ShowUpgradeMenu(selectedUpgrades);
    }

    public void ApplyUpgrade(int selectedButtonID)
    {
        Upgrade selectedUpgrade = selectedUpgrades[selectedButtonID];

        if(aquiredUpgrades == null) {aquiredUpgrades = new List<Upgrade>();}
        aquiredUpgrades.Add(selectedUpgrade);

        //TODO: remove upgrade from upgrade pool?
        // upgradePool.Remove(selectedUpgrade)

        //TODO: 
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
}
