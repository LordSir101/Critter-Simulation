using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanelManager : MonoBehaviour
{
    [SerializeField] private GameObject upgradeMenu;
    [SerializeField] private List<UpgradeButton> buttons;
    [SerializeField] private TextMeshProUGUI warning;

    public void Upgrade(int pressedButtonId)
    {
        if(LogicManager.SharedInstance.gameObject.GetComponent<UpgradeManager>().ApplyUpgrade(pressedButtonId))
        {
            CloseUpgradeMenu();
        }
        else
        {
            warning.enabled = true;
        }
        
    }

    public void NoUpgrade()
    {
        CloseUpgradeMenu();
    }

    public void ShowUpgradeMenu(List<Upgrade> possibleUpgrades)
    {
        PauseControl.PauseGame(true);
        Debug.Log(possibleUpgrades.Count);

        for(int i = 0; i < possibleUpgrades.Count; i++)
        {
            buttons[i].SetData(possibleUpgrades[i]);
        }

        upgradeMenu.SetActive(true);
    }

    public void CloseUpgradeMenu()
    {
        for(int i = 0; i < buttons.Count; i++)
        {
            buttons[i].gameObject.SetActive(false);
        }
        warning.gameObject.SetActive(false);
        upgradeMenu.SetActive(false);
        PauseControl.PauseGame(false);
    }

}
