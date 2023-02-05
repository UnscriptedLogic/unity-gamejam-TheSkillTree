using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class BottomHUD : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI desc;
    [SerializeField] private Transform buttonParent;

    [Header("Prefabs")]
    [SerializeField] private GameObject optionButtonPrefab;
    [SerializeField] private GameObject upgradeOptionButtonPrefab;

    public void SetBasicDetails(string entityName, Sprite icon)
    {
        this.icon.enabled = true;
        this.icon.sprite = icon;
        desc.text = entityName;
    }

    public void ResetBasicDetails()
    {
        icon.enabled = false;
        icon.sprite = null;
        desc.text = "";
    }

    public void ClearOptions()
    {
        for (int i = buttonParent.childCount - 1; i >= 0; i--)
        {
            Destroy(buttonParent.GetChild(i).gameObject);
        }
    }

    public Button CreateOptionButton(NodeSO node, Action method)
    {
        GameObject button = Instantiate(optionButtonPrefab, buttonParent);
        button.GetComponent<Button>().onClick.AddListener(() => method());
        button.GetComponent<OptionButton>().Init(node.name, node.Icon);
        return button.GetComponent<Button>();
    }

    public Button CreateUpgradeButton(Upgrade upgrade, Action OnClick)
    {
        GameObject upgradeObject = Instantiate(upgradeOptionButtonPrefab, buttonParent);
        
        Button upgradeButton = upgradeObject.GetComponent<Button>();
        upgradeButton.onClick.AddListener(() => OnClick());
        
        UpgradeOption upgradeOption = upgradeObject.GetComponent<UpgradeOption>();
        upgradeOption.Init(upgrade.upgradeName, upgrade.upgradeIcon);
        upgradeOption.InitUpgrade(upgrade.upgradeTimes, upgrade.maxUpgradeTimes);

        return upgradeButton;
    }
}
