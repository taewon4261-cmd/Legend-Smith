using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="SmithingRateData",menuName = "Game/Smithing Rate Data")]
public class SmithingRateSO : ScriptableObject
{
    [Header("확률 설정")]
     public int normalRate = 60;
     public int rareRate = 90;
     public int epicRate = 99;


    // 등급을 체크해주는 함수
    public ItemRarity GetRandomRarity(int luckBonus)
    {

        int randomVal = Random.Range(0, 100) + luckBonus;

        Debug.Log($"주사위 : {randomVal - luckBonus} + 보너스 {luckBonus} + 최종 {randomVal}");

        //낮은 숫자부터 검사

        if (randomVal <= normalRate) return ItemRarity.Normal;
        if (randomVal <= rareRate) return ItemRarity.Rare;
        if (randomVal <= epicRate) return ItemRarity.Epic;
        return ItemRarity.Legend;
    }
}
