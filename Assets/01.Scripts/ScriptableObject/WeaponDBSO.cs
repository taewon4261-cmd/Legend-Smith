using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="WeaponDB", menuName = "Game/WeaponDataBaseSO")]
public class WeaponDBSO : ScriptableObject
{
    [Header("무기 리스트")]
    public List<ItemDataSO> allWeapon;

    [Header("밸런스 설정")]
    public int startCost = 100;
    public float costMultiplier = 1.5f;

    public int startPrice = 120;
    public float priceMultiplier = 1.2f;

    public int unlockedPrice = 500;
    public float unlockedPriceMultiplier = 2f;


    /// <summary>
    /// 인스펙터 우클릭 메뉴를 통해 실행, 리스트에 등록된 모든 무기를 설정에 따라 갱신
    /// </summary>
    [ContextMenu("Auto Calculate Balance")]
    public void AutoCalculate()
    {
        for (int i = 0; i < allWeapon.Count; i++)
        {
            if (allWeapon[i] == null) continue;

            // 가격 = 초기가격 * 배율 * i의 제곱
            float newCost = startCost*Mathf.Pow(costMultiplier, i);
            float newPrice = startPrice * Mathf.Pow(priceMultiplier, i);
            float newunlockedPrice = unlockedPrice * Mathf.Pow(unlockedPriceMultiplier, i);

            // 결과를 반올림해서 데이터에 저장
            allWeapon[i].cost = Mathf.RoundToInt(newCost);
            allWeapon[i].price = Mathf.RoundToInt(newPrice);
            allWeapon[i].unlockedPrice = Mathf.RoundToInt(newunlockedPrice);

        }
        Debug.Log("모든 무기 가격 밸런스 조정 완료");
    }
}
