using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    public List<UpgradeDataSO> allUpgradeData;

    public const string UpgradeKey = "Upgrade_";

    public void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private Dictionary<string, int> upgradeLevels = new Dictionary<string, int>();

    public float GetTotalBonusValue(UpgradeType targetType)
    {
        float totalBonus = 0;

        foreach (var data in allUpgradeData)
        {
            if (data.type == targetType)
            {
                totalBonus += GetTotalEffect(data);
            }
        }
        return totalBonus;
    }

    public int GetLevel(UpgradeDataSO data)
    {
        if (!upgradeLevels.ContainsKey(data.upgradeName))
        {
            int savedLevel = PlayerPrefs.GetInt(UpgradeKey + data.upgradeName, 0);
            upgradeLevels.Add(data.upgradeName, savedLevel);
        }

        return upgradeLevels[data.upgradeName];
    }

    public int GetCurrentCost(UpgradeDataSO data)
    {
        int level = GetLevel(data);

        return Mathf.RoundToInt(data.baseUpgradeCost * Mathf.Pow(data.costMultiplier, level));
    }

    public float GetTotalEffect(UpgradeDataSO data)
    {
        return GetLevel(data) * data.valuePerLevel;
    }

    public bool TryUpgrade(UpgradeDataSO data)
    {
        int currentLevel = GetLevel(data);

        int cost = GetCurrentCost(data);

        if (ResourceManager.Instance.gold >= cost)
        {
            ResourceManager.Instance.TrySpendGold(cost);

            currentLevel++;
            upgradeLevels[data.upgradeName] = currentLevel;

            PlayerPrefs.SetInt(UpgradeKey + data.upgradeName, currentLevel);
            PlayerPrefs.Save();

            return true;
        }

        return false;
    }
}

