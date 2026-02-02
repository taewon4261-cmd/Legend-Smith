using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="WeaponDB", menuName = "Game/WeaponDataBaseSO")]
public class WeaponDBSO : ScriptableObject
{
    [Header("무기 리스트")]
    public List<ItemDataSO> allWeapon;

    [Header("밸런스 설정")]
    public int startCost = 100;
    public float costMultiplier = 1.2f;

    public int startPrice = 120;
    public float priceMultiplier = 1.2f;

    public int unlockedPrice = 500;
    public float unlockedPriceMultiplier = 2f;

    [ContextMenu("Auto Calculate Balance")]
    public void AutoCalculate()
    {
        for (int i = 0; i < allWeapon.Count; i++)
        {
            if (allWeapon[i] == null) continue;

            float newCost = startCost*Mathf.Pow(costMultiplier, i);
            float newPrice = startPrice * Mathf.Pow(priceMultiplier, i);
            float newunlockedPrice = unlockedPrice * Mathf.Pow(unlockedPriceMultiplier, i);

            allWeapon[i].cost = Mathf.RoundToInt(newCost);
            allWeapon[i].price = Mathf.RoundToInt(newPrice);
            allWeapon[i].unlockedPrice = Mathf.RoundToInt(newunlockedPrice);

        }

        Debug.Log("모든 무기 가격 밸런스 조정 완료");
    }
}
