using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ItemData", menuName ="Game/Item Data")]
public class ItemDataSO : ScriptableObject
{
    [Header("기본 정보")]
    public string itemName;
    public Sprite icon;
    public int cost;
    public int price;
}
