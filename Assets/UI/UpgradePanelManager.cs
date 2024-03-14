using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanelManager : MonoBehaviour
{
    [SerializeField] private GameObject upgradeMenu;
    [SerializeField] private List<UpgradeButton> buttons;
    
    // Start is called before the first frame update

    public void Upgrade(int pressedButtonId)
    {
        EnvironmentManager.SharedInstance.gameObject.GetComponent<UpgradeManager>().ApplyUpgrade(pressedButtonId);
        CloseUpgradeMenu();
    }

    public void ShowUpgradeMenu(List<Upgrade> possibleUpgrades)
    {
        PauseControl.PauseGame(true);

        for(int i = 0; i < buttons.Count; i++)
        {
            buttons[i].SetData(possibleUpgrades[i]);
        }

        upgradeMenu.SetActive(true);
    }

    public void CloseUpgradeMenu()
    {
        upgradeMenu.SetActive(false);
        PauseControl.PauseGame(false);
    }

}
