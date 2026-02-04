using UnityEngine;

public class UnlockManager : MonoBehaviour
{
    public static UnlockManager Instance;

    // 저장시 사용할 키
    private const string UnlockKey = "Unlock_";

    private void Awake()
    {
        if(Instance == null)Instance = this;
        else Destroy(gameObject);
    }
    
    /// <summary>
    /// 아이템의 해금 여부 확인
    /// </summary>
    /// <param name="data"> 아이템 정보 데이터 </param>
    /// <returns>해금되어 있으면 true, 잠겨있으면 false 반환 </returns>
    public bool CheckUnlock(ItemDataSO data)
    {
        //기본적으로 해금된 아이템인 경우 즉시 true
        if(data.isDefaultUnlocked == true) return true;

        // 키값 확인, 1이면 해금 0이면 잠금 판정
        return PlayerPrefs.GetInt(UnlockKey + data.itemName, 0) == 1;
    }
    /// <summary>
    /// 아이템 구매시 데이터를 PlayerPrefs로 저장하는 함수
    /// </summary>
    /// <param name="data">구매한 아이템 정보</param>
    public void PurchaseWeapon(ItemDataSO data)
    {
        PlayerPrefs.SetInt( UnlockKey + data.itemName, 1);
        PlayerPrefs.Save(); // 저장후 디스크에 기록해주는 함수
    }
}
