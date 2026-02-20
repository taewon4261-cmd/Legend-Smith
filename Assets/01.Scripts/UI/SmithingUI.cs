using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SmithingUI : MonoBehaviour
{
    [Header("Weapon Prefab")]
    public Transform gridContent;
    public GameObject craftingSlot;

    [Header("SO")]
    public SmithingDifficultySO difficultyData;
    public SmithingRateSO rateData;
    public ItemDataSO currentTargetItem; // 선택된 아이템
    public WeaponDBSO weaponDBSO;

    [Header("UI")]
    public Image targetZoneImage;
    public Slider timingSlider;

    [Header("Target UI")]
    public Image anvilIcon;
    public TextMeshProUGUI WeaponName;
    public TextMeshProUGUI WeaponCost;

    private float targetPosition = 0.5f; // 중심 위치 ()
    private float tolerance = 0.1f; // 난이도 조절

    [Header("성급 표시")]
    public GameObject[] comboStars;
    private int currentCombo = 0;
    private int maxCombo = 5;

    [Header("성공 후 팝업창")]
    public GameObject choicePopup;
    private bool isPuased;

    [Header("이펙트")]
    public ParticleSystem hitEffect;
    public CameraShake cameraShake;

    [Header("제작 타겟")]
    public ItemDataSO currentItem;

    private float currentSpeed;
    private float currentTolerance;

    private bool isPlaying;

    void Start()
    {
        if (timingSlider != null)
        {
            timingSlider.value = 0;
        }

        UpdateComboUI(); // 별 갯수 0개로 초기화
        SetDifficulty(); // 난이도 초기화

        CreateWeaponSlots();


        ResetUIState();
    }

    void Update()
    {
        if (timingSlider == null || isPuased || !isPlaying) return;

        // Mathf.PingPong을 사용해 슬라이더 왕복 애니메이션을 심플하게 구현
        timingSlider.value = Mathf.PingPong(Time.time * currentSpeed, 1f);
    }

    /// <summary>
    /// 타이밍 슬라이더가 목표 지점 안에 들어왔는지 판정 (핵심 게임 루프)
    /// </summary>
    public void CheckHit()
    {

        if (!isPlaying)
        {
            GameManager.Instance.SFX.PlaySFX("OnClickBtnFail", 2f);
            return;
        }

        GameManager.Instance.SFX.PlaySFX("Smithing", 2f);

        if (GameManager.Instance.Vibration != null && isPlaying)
            GameManager.Instance.Vibration.Vibrate();

        if (cameraShake != null)
            cameraShake.TriggerShake();


        float currentVal = timingSlider.value; ;
        float distance = Mathf.Abs(currentVal - targetPosition);

        if (distance <= tolerance) // 성공
        {
            isPuased = true;
            SmithingEffect(true).Forget(); // 비동기 이펙트 코루틴 대신 UniTask 활용
            if (hitEffect != null) hitEffect.Play(); // 히트이펙트

            if (currentCombo >= maxCombo) AddToInventory();
            else choicePopup.SetActive(true);

        }
        else // 실패
        {
            currentCombo = 0;
            UpdateComboUI(); // 실패 시 성급 이미지 다 꺼짐
            SmithingEffect(false).Forget();
            AddToInventory();
            SetDifficulty();

            GameManager.Instance.SFX.PlaySFX("HitFail", 1);
        }
    }
    void CreateWeaponSlots()
    {
        foreach (var weapon in weaponDBSO.allWeapon)
        {
            GameObject newWeapon = Instantiate(craftingSlot, gridContent);
            CraftingSlot slot = newWeapon.GetComponent<CraftingSlot>();

            slot.Setup(weapon, this);
        }
    }

    void ResetUIState()
    {

        if (anvilIcon != null) anvilIcon.gameObject.SetActive(false);
        if (WeaponName != null) WeaponName.gameObject.SetActive(false);
        if (WeaponCost != null) WeaponCost.gameObject.SetActive(false);

        currentItem = null;
    }

    // 추가 도전버튼
    public void OnButtonChallenge()
    {
        choicePopup.SetActive(false);

        currentCombo++;
        SetDifficulty();
        UpdateComboUI();

        timingSlider.value = 0;

        isPuased = false;
    }


    // 그만하기 버튼
    public void OnButtonKeep()
    {
        choicePopup.SetActive(false);
        GameManager.Instance.SFX.PlaySFX("HitSuccess", 1);
        AddToInventory();
    }

    // 핸들 움직이기 시작 버튼
    public void OnClickGameStart()
    {
        if (isPlaying)
        {
            GameManager.Instance.SFX.PlaySFX("OnClickBtnFail", 1);
            return;
        }

        if (currentItem == null)
        {
            GameManager.Instance.SFX.PlaySFX("OnClickBtnFail", 1);
            return;
        }


        if (GameManager.Instance.Inven.myInven.Count >= 15)
        {
            GameManager.Instance.SFX.PlaySFX("OnClickBtnFail", 1);
            return;
        }

        bool isSuccess = GameManager.Instance.Resource.TrySpendOre(currentItem.cost);

        if (isSuccess)
        {
            isPlaying = true;
            timingSlider.value = 0;
            currentCombo = 0;
            SetDifficulty();
            UpdateComboUI();
        }
        else
        {
            GameManager.Instance.SFX.PlaySFX("OnClickBtnFail", 1);
        }
    }

    void AddToInventory()
    {
        float bonusValue = GameManager.Instance.Upgrade.GetTotalBonusValue(UpgradeType.ComboBonus);
        int bonusLuck = currentCombo + (int)bonusValue; // 콤보 시 가중치 반영

        ItemRarity finalRarity = rateData.GetRandomRarity(bonusLuck);

        GameManager.Instance.Inven.AddItem(currentItem, finalRarity);

        GameManager.Instance.Quest.NotifyQuestAction(QuestType.Smithing, 1);

        isPlaying = false;

        currentCombo = 0;
        SetDifficulty(); // 다음 난이도
        UpdateComboUI(); // 별 추가
        timingSlider.value = 0;
        isPuased = false;
    }

    // 난이도조절SO를 불러와서 업데이트
    void SetDifficulty()
    {
        var data = difficultyData.GetLevelData(currentCombo);

        currentSpeed = data.speed;
        tolerance = data.tolerance; // 판정용 변수
        currentTolerance = data.tolerance;  // UI용 변수

        UpdateTargetZoneUI(data.tolerance);

    }

    /// <summary>
    /// 클릭한 아이템의 정보를 보내주는 함수
    /// </summary>
    /// <param name="newItem"></param>
    public void SetTargetItem(ItemDataSO newItem)
    {
        if (newItem == null) return;

        currentItem = newItem;

        if (anvilIcon != null)
        {
            anvilIcon.sprite = newItem.icon;
            anvilIcon.gameObject.SetActive(true);
        }

        if (WeaponName != null)
        {
            WeaponName.text = newItem.itemName;
            WeaponName.gameObject.SetActive(true); // 다시 켜기!
        }

        if (WeaponCost != null)
        {
            WeaponCost.text = $"{newItem.cost} 광석";
            WeaponCost.gameObject.SetActive(true); // 다시 켜기!
        }
    }


    // 난이도 상승 시 타겟범위 변경 함수
    void UpdateTargetZoneUI(float currentTol)
    {
        RectTransform rect = targetZoneImage.rectTransform;

        float minX = 0.5f - currentTol;
        float maxX = 0.5f + currentTol;

        rect.anchorMin = new Vector2(minX, 0f);
        rect.anchorMax = new Vector2(maxX, 1f);

        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    // 성급 출력
    void UpdateComboUI()
    {
        for (int i = 0; i < comboStars.Length; i++)
        {
            if (i < currentCombo) comboStars[i].SetActive(true);
            else comboStars[i].SetActive(false);
        }
    }

    // 클릭시 성공 실패 색상처리
    async UniTaskVoid SmithingEffect(bool isSuccess)
    {
        if (isSuccess == true)
        {
            targetZoneImage.color = Color.green;

        }
        else targetZoneImage.color = Color.gray;

        await UniTask.Delay(300, cancellationToken: this.GetCancellationTokenOnDestroy());

        targetZoneImage.color = Color.red;

    }

}
