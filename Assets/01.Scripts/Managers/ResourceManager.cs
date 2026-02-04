using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;


    [Header ("자원")]
    public int currentOre = 0;
    public int orePerSecond = 1;
    public int gold;
    public int diamond;

    [Header("UI")]
    public TextMeshProUGUI oreText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI diaText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // UI 초기화, 게임 시작시 재화수급 시작
        UpdateAllUI();
        ProduceOrePerSecond().Forget();
    }


    //초당 광석 재화 생산
    async UniTaskVoid ProduceOrePerSecond()
    {
        while (true)
        {
            await UniTask.Delay(1000, cancellationToken: this.GetCancellationTokenOnDestroy());
            currentOre += orePerSecond;

            //생산될때마다 업데이트
            UpdateOreUI();
        }
    }

    /// <summary>
    /// 무기 제작 전 재화 확인 후 시도할때 사용하는 함수
    /// </summary>
    /// <param name="amount"></param>
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
    ///  아이템 판매시 골드 얻는 함수 (무기 해금, 상점 업그레이드에서 사용)
    /// </summary>
    /// <param name="amount"></param>
    public void AddGold(int amount)
    {
        gold += amount;
        UpdateGoldUI();
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
}
