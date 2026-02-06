using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewUpgrade",menuName ="Game/UpgradeData")]
public class UpgradeDataSO : ScriptableObject
{
    [Header("기본 정보")]
    public string upgradeName;
    public Sprite Icon;

    [Header("설정")]
    public UpgradeType type; // 업그레이드 종류

    [Header("효과 수치")]
    public float valuePerLevel; // 레벨당 효과

    [Header("가격")]
    public int baseUpgradeCost;
    public float costMultiplier = 1.2f;
}
