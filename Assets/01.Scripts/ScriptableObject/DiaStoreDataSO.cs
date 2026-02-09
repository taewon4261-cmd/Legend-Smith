using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "DiaStore", menuName = "Game/DiaStoreData")]
public class DiaStoreDataSO : ScriptableObject
{
    [Header("기본 정보")]
    public string rewardName;
    public Sprite icon;
    public int reward;

    [Header("설정")]
    public DiaStoreType type;

    [Header("가격")]
    public int price;


}
