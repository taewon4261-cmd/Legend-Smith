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

    private readonly Color lockedColor = new Color(0, 0, 0, 0.5f); 
    private readonly Color unlockedColor = Color.white;

    private void OnEnable()
    {
        // 패널이 켜질때 해금 상태를 실시간으로 체크해서 갱신
        if (itemData == null) return;
        bool result = UnlockManager.Instance.CheckUnlock(itemData);

        if (result == true) UnlockSlot();
        else LockSlot();
    }

    private void Awake()
    {
        //컴포넌트 자동 할당 및 초기화
        if(Icon == null) Icon = GetComponent<Image>();
        if(button == null) button = GetComponent<Button>();
        Init();
    }

    // 버튼 이벤트 초기 설정
    void Init()
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClickSelectThis);
    }

    // 아이템 해금 전 시각적 처리 및 버튼 클릭 제한
    void LockSlot()
    {
        Icon.color = lockedColor;
        button.interactable = false;
    }

    // 아이템 해금 후 시각적 처리 및 버튼 클릭 제한해제
    void UnlockSlot()
    {
        Icon.color = unlockedColor;
        button.interactable = true;
    }

    // 클릭 시 현재 아이템 정보 전달
    public void OnClickSelectThis()
    {
        smithingUI.SetTargetItem(itemData);
    }
}
