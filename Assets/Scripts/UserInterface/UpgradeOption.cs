using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeOption : OptionButton
{
    [Header("Upgrade Extension")]
    [SerializeField] private Transform levelParent;
    [SerializeField] private GameObject levelPrefab;
    [SerializeField] private GameObject obtainedLevelPrefab;

    public void InitUpgrade(int currentLevel, int maxLevel)
    {
        for (int i = 0; i < currentLevel; i++)
        {
            Instantiate(obtainedLevelPrefab, levelParent);
        }

        for (int i = 0; i < maxLevel - currentLevel; i++)
        {
            Instantiate(levelPrefab, levelParent);
        }
    }
}
