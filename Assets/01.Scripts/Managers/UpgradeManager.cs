using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public List<UpgradeDataSO> allUpgradeData;

    public const string UpgradeKey = "Upgrade_";

    private Dictionary<string, int> upgradeLevels = new Dictionary<string, int>();

    public void Init()
    {
        upgradeLevels.Clear();

        foreach (var data in allUpgradeData)
        {
            int savedLevel = PlayerPrefs.GetInt(UpgradeKey + data.upgradeName, 0);
            upgradeLevels.Add(data.upgradeName, savedLevel);
        }
    }

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
        if (upgradeLevels.TryGetValue(data.upgradeName, out int level))
        {
            return level;
        }

        return 0; // 리스트에 없는 데이터가 들어왔을 때를 대비한 안전 장치
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

        if (GameManager.Instance.Resource.gold >= cost)
        {
            GameManager.Instance.Resource.TrySpendGold(cost);

            GameManager.Instance.SFX.PlaySFX("BuySellUpgrade", 1);

            currentLevel++;
            upgradeLevels[data.upgradeName] = currentLevel;

            PlayerPrefs.SetInt(UpgradeKey + data.upgradeName, currentLevel);
            PlayerPrefs.Save();

            int totalLevel = GetAllTotalLevel();

            GameManager.Instance.LootLocker.SubmitScore("rank_upgrade_total", totalLevel);

            return true;
        }

        GameManager.Instance.SFX.PlaySFX("OnClickBtnFail", 1);

        return false;
    }

    // 전체 강화 레벨 합계를 반환하는 함수
    public int GetAllTotalLevel()
    {
        int total = 0;
        foreach (var data in allUpgradeData)
        {
            total += GetLevel(data);
        }
        return total;
    }
}

