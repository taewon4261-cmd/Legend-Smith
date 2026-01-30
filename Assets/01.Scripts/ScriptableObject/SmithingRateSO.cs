using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="SmithingRateData",menuName = "Game/Smithing Rate Data")]
public class SmithingRateSO : ScriptableObject
{
    [Header("확률 설정")]
    [Range(0, 100)] public int normalRate = 60;
    [Range(0, 100)] public int rareRate = 30;
    [Range(0, 100)] public int epciRate = 9;
    [Range(0, 100)] public int legendRate = 1;


    // 등급을 체크해주는 함수
    public ItemRarity GetRandomRarity()
    {

        int randomVal = Random.Range(0, 100); // 0 ~ 99까지
        int currentRate = 0;

        currentRate += normalRate;
        if (randomVal < currentRate) return ItemRarity.Normal;

        currentRate += rareRate;
        if (randomVal < currentRate) return ItemRarity.Rare;

        currentRate += epciRate;
        if (randomVal < currentRate) return ItemRarity.Epic;

        return ItemRarity.Legend;
    }
}
