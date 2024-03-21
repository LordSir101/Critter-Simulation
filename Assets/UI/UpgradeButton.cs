using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI upgradeName;
    [SerializeField] private TextMeshProUGUI cost;

    public void SetData(Upgrade upgrade)
    {
        icon.sprite = upgrade.icon;
        description.text = upgrade.description;
        upgradeName.text = upgrade.upgradeName;
        cost.text = upgrade.cost.ToString();
        gameObject.SetActive(true);
    }
}
