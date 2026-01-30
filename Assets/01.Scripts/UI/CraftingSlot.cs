using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSlot : MonoBehaviour
{
    [Header("데이터")]
    public ItemDataSO itemData;
    public SmithingUI smithingUI;

    [Header("UI")]
    public Image Icon;
    public Button button;

    private readonly Color lockedColor = new Color(0, 0, 0, 0.5f); // Color은 구조체라 GC 관여없음
    private readonly Color unlockedColor = Color.white;

    [Header("잠금 설정")]
    public bool isUnlocked;

    private void Start()
    {
        if(Icon == null) Icon = GetComponent<Image>();
        if(button == null) button = GetComponent<Button>();
      

        if (isUnlocked)
        {
            UnlockSlot(); // 체크되어 있으면 해금 상태로 시작
        }
        else
        {
            LockSlot();   // 아니면 잠금
        }
    }

    public void LockSlot()
    {
        Icon.color = lockedColor;
        button.interactable = false; // 클릭 불가
        isUnlocked = false;
    }

    public void UnlockSlot()
    {
        Icon.color = unlockedColor;
        button.interactable = true;
        isUnlocked = true;
    }

    public void OnClickSelectThis()
    {
        smithingUI.SetTargetItem(itemData);

        Debug.Log($"{itemData.itemName}선택됨");
    }
}
