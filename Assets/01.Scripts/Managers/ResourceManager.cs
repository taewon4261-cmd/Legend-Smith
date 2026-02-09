using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;


    [Header ("자원")]
    public int currentOre = 0;
    public int baseOrePerSecond = 1;
    public int gold;
    public int diamond;

    [Header("UI")]
    public TextMeshProUGUI oreText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI diaText;

    private const string OreKey = "Ore_";
    private const string GoldKey = "Gold_";
    private const string DiaKey = "Dia_";

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        LoadResource();
        // UI 초기화, 게임 시작시 재화수급 시작
        UpdateAllUI();
        ProduceOrePerSecond().Forget();
    }


    //초당 광석 재화 생산
    async UniTaskVoid ProduceOrePerSecond()
    {
        int autoSaveCounter = 0;

        while (true)
        {
            await UniTask.Delay(1000, cancellationToken: this.GetCancellationTokenOnDestroy());

            int bonus = (int)UpgradeManager.Instance.GetTotalBonusValue(UpgradeType.MiningAmount);
            int finalAmount = baseOrePerSecond + bonus;

            currentOre += finalAmount;

            //생산될때마다 업데이트
            UpdateOreUI();
            PlayerPrefs.SetInt(OreKey, currentOre);

            autoSaveCounter++;
            if (autoSaveCounter >= 30)
            {
                SaveAllData();
                autoSaveCounter = 0;
            }

        }
    }

    // 게임 꺼질때 저장
    private void OnApplicationQuit()
    {
        SaveAllData();
    }

    // 게임 멈췄을때 저장
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveAllData();
        }
    }

    /// <summary>
    /// 무기 제작 전 재화 확인 후 시도할때 사용하는 함수
    /// </summary>
    /// <param name="amount"> 광석 재료 비용</param>
    /// <returns></returns>
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
       currentOre = PlayerPrefs.GetInt(OreKey, 10000);
       gold = PlayerPrefs.GetInt(GoldKey,10000);
       diamond =  PlayerPrefs.GetInt(DiaKey, 10000);
    }

    void SaveAllData()
    {
        PlayerPrefs.SetInt(OreKey, currentOre);
        PlayerPrefs.SetInt(GoldKey, gold);
        PlayerPrefs.SetInt(DiaKey, diamond);

        PlayerPrefs.Save();
    }
}
