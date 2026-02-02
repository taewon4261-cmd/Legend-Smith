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

    private void Awake()
    {
        if(Icon == null) Icon = GetComponent<Image>();
        if(button == null) button = GetComponent<Button>();
        Init();

        if (isUnlocked) UnlockSlot();
        else LockSlot();
    }

    void Init()
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClickSelectThis);
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
