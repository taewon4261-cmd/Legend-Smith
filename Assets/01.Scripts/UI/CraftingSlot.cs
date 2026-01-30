using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSlot : MonoBehaviour
{
    [Header("데이터 연결")]
    public ItemDataSO itemData;
    public SmithingUI smithingUI;

    public void OnClickSelectThis()
    {
        smithingUI.SetTargetItem(itemData);

        Debug.Log($"{itemData.itemName}선택됨");
    }
}
