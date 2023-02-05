using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade
{
    public Sprite upgradeIcon;
    public string upgradeName;
    public int upgradeTimes;
    public int maxUpgradeTimes;
    public Action<Node> UpgradeMethod;

    public Upgrade(int maxUpgradeTimes, Action<Node> UpgradeMethod)
    {
        upgradeTimes = 0;
        this.maxUpgradeTimes = maxUpgradeTimes;
        this.UpgradeMethod = UpgradeMethod;
    }
}

public interface IUpgradable
{
    List<Upgrade> Upgrades { get; }
}
