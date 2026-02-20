using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [Header ("자원")]
    public int currentOre = 0;
    public int baseOrePerSecond = 1;
    public int gold;
    public int diamond;

    public int totalGoldEarned = 0;

    [Header("UI")]
    public TextMeshProUGUI oreText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI diaText;

    // 상수로 키 값을 관리하여 오타로 인한 세이브 데이터 유실 방지
    private const string OreKey = "Ore_";
    private const string GoldKey = "Gold_";
    private const string DiaKey = "Dia_";
    public void Init()
    {
        LoadResource();
        UpdateAllUI(); // UI 초기화, 게임 시작시 재화수급 시작
        ProduceOrePerSecond().Forget(); // UniTask 비동기 실행 (Fire and Forget)
    }


    /// <summary>
    /// 백그라운드에서 초당 광석 재화를 생산하는 비동기 루틴
    /// </summary>
    async UniTaskVoid ProduceOrePerSecond()
    {
        int autoSaveCounter = 0;

        while (true)
        {
            // 취소 토큰을 넘겨주어 오브젝트 파괴 시 메모리 누수를 방지
            await UniTask.Delay(1000, cancellationToken: this.GetCancellationTokenOnDestroy());

            int bonus = (int)GameManager.Instance.Upgrade.GetTotalBonusValue(UpgradeType.MiningAmount);
            int finalAmount = baseOrePerSecond + bonus;

            currentOre += finalAmount;

            //생산될때마다 업데이트
            UpdateOreUI();

            autoSaveCounter++;
            if (autoSaveCounter >= 30) // 30초마다 자동 저장
            {
                GameManager.Instance.SaveAllGameData();
                autoSaveCounter = 0;
            }

        }
    }

    /// <summary>
    /// 무기 제작 전 재화 확인 후 시도할때 사용하는 함수
    /// </summary>
    /// <param name="amount"> 광석 재료 비용</param>
    /// <returns>차감 성공 여부</returns>
    public bool TrySpendOre(int amount)
    {
        if (currentOre >= amount)
        {
            currentOre -= amount;

            UpdateOreUI();

            return true;
        }
        return false;
    }

    /// <summary>
    /// 업그레이드 전 골드 확인 후 시도할때 사용하는 함수
    /// </summary>
    /// <param name="amount"> 업그레이드 가격 </param>
    /// <returns></returns>
    public bool TrySpendGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;

            UpdateGoldUI();

            return true;
        }
        return false;
    }

    /// <summary>
    /// 업그레이드 전 다이아 확인 후 시도할때 사용하는 함수
    /// </summary>
    /// <param name="amount"> 다이아상점 구매 가격 </param>
    /// <returns></returns>
    public bool TrySpendDia(int amount)
    {
        if (diamond >= amount)
        {
            diamond -= amount;

            UpdateDiaUI();

            return true;
        }
        return false;
    }

    /// <summary>
    ///  아이템 판매시 골드 얻는 함수
    /// </summary>
    /// <param name="amount"></param>
    public void AddGold(int amount)
    {
        gold += amount;
        totalGoldEarned += amount;
        UpdateGoldUI();
    }

    public void AddDia(int amount)
    {
        diamond += amount;
        UpdateDiaUI();
    }
    public void AddOre(int amount)
    {
        currentOre += amount;
        UpdateOreUI();
    }

    // 초기화 함수들
    void UpdateAllUI()
    {
        UpdateOreUI();
        UpdateGoldUI();
        UpdateDiaUI();
    }
    void UpdateOreUI()
    {
        oreText.text = $"{currentOre}";
    }
    void UpdateGoldUI()
    {
        goldText.text = $"{gold}";
    }
    
    void UpdateDiaUI()
    {
        diaText.text = $"{diamond}";
    }
     
    void LoadResource()
    {
       currentOre = PlayerPrefs.GetInt(OreKey, 500);
       gold = PlayerPrefs.GetInt(GoldKey, 500);
       diamond =  PlayerPrefs.GetInt(DiaKey, 0);
    }

    public void SaveAllData()
    {
        PlayerPrefs.SetInt(OreKey, currentOre);
        PlayerPrefs.SetInt(GoldKey, gold);
        PlayerPrefs.SetInt(DiaKey, diamond);

        GameManager.Instance.LootLocker.SubmitScore("rank_gold", gold);

        PlayerPrefs.Save();
    }
}
