using UnityEngine;
using UnityEngine.UI;

public class WeaponShopSlot : MonoBehaviour
{
    public ItemDataSO data;
    public Image icon;
    private WeaponStoreUI storeUI;

    /// <summary>
    /// 상점 생성 시 호출되어 무기 데이터와 UI 매니저를 연결
    /// </summary>
    /// <param name="newData"> 할당할 무기 데이터 </param>
    /// <param name="manager"> 상점 메인 UI 스크립트 </param>
    public void SetWeaponSlot(ItemDataSO newData, WeaponStoreUI manager)
    {
        data = newData;
        storeUI = manager;
        icon.sprite = newData.icon;

        SetTransparency(0.5f); // 초기 상태 (반투명 처리)

        bool isUnlocked = UnlockManager.Instance.CheckUnlock(data);
    }

    // 이미지의 투명도 조절
    void SetTransparency(float alpha)
    {
        Color color = icon.color;
        color.a = alpha;
        icon.color = color;
    }

    // 클릭시 선택된 무기 정보 전달
    public void OnClickSlot()
    {
        if (storeUI != null)
        {
            storeUI.SelectedSlot(data);
        }
    }
        
}
