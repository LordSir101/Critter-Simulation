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

    public void SetData(Upgrade upgrade)
    {
        icon.sprite = upgrade.icon;
        description.text = upgrade.description;

    }
}
