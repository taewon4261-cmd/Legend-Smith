using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class SmithingUI : MonoBehaviour
{
    [Header("SO")]
    public SmithingDifficultySO difficultyData;
    public SmithingRateSO rateData;

    [Header("UI")]
    public Image targetZoneImage;
    public Slider timingSlider;
    public GameObject smithingPanelOnBtn; // 스미싱패널버튼인데 패배나 성공후에 인벤에 들어간거 확인하고 x표시나 뒤로가기에서 넣으면 될듯

    private float targetPosition = 0.5f; // 중심 위치
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
    }

    void Update()
    {
        if (timingSlider == null || isPuased || !isPlaying) return;
        if (timingSlider != null)
        {
            // 데이터에서 가져온 속도를 붙여줌
            timingSlider.value = Mathf.PingPong(Time.time * currentSpeed, 1f);
        }
    }

    // 핵심 코드
    public void CheckHit()
    {
        if (!isPlaying) return;
       
        float currentVal = timingSlider.value; ;

        float diff = currentVal - targetPosition;
        float distance = Mathf.Abs(diff);

        if (distance <= tolerance) // 성공
        {
            isPuased = true;

            SmithingEffect(true).Forget(); // 이미지 바 색 변경으로 직관적 이미지 표현
            if (hitEffect != null)
            {
                hitEffect.Play();
            }
            if (currentCombo >= maxCombo)
            {
                AddToInventory();
            }
            else
            {
                choicePopup.SetActive(true);
            }
        }
        else // 실패
        {
            currentCombo = 0;
            UpdateComboUI(); // 실패 시 성급 이미지 다 꺼짐
            SmithingEffect(false).Forget();

            AddToInventory();
            SetDifficulty();

            //gameObject.SetActive(false);

            Debug.Log($"실패...");
        }
    }
    public void OnButtonChallenge() // 추가 도전버튼
    {
        choicePopup.SetActive(false);

        currentCombo++;
        SetDifficulty();
        UpdateComboUI();

        timingSlider.value = 0;

        isPuased = false;
    }

    public void OnButtonKeep() // 그만하기 버튼
    {
        choicePopup.SetActive(false);
        AddToInventory();
    }
    public void OnClickGameStart()
    {
        if (isPlaying) return;

        isPlaying = true;
        timingSlider.value = 0;
        currentCombo = 0;
        SetDifficulty();
        UpdateComboUI();
    }

    void AddToInventory()
    {
        ItemRarity finalRarity = ItemRarity.Normal; // 실패 시 노말

        if (currentCombo >= maxCombo)
        {
            finalRarity = rateData.GetRandomRarity(); // 콤보 수에 따른 확률 상승
        }

        Debug.Log($"제작 완료. 등급 {finalRarity} / 콤보 : {currentCombo}");

        isPlaying = false;

        currentCombo = 0;
        SetDifficulty();
        UpdateComboUI();
        timingSlider.value = 0;
        isPuased = false;
    }

    void SetDifficulty()
    {
        var data = difficultyData.GetLevelData(currentCombo);

        currentSpeed = data.speed;
        tolerance = data.tolerance; // 판정용 변수
        currentTolerance = data.tolerance;  // UI용 변수

        UpdateTargetZoneUI(data.tolerance);

    }

    void UpdateTargetZoneUI(float currentTol) // 난이도 상승 시 타겟범위 변경 함수
    {
        RectTransform rect = targetZoneImage.rectTransform;

        float minX = 0.5f - currentTol;
        float maxX = 0.5f + currentTol;

        rect.anchorMin = new Vector2(minX, 0f);
        rect.anchorMax = new Vector2(maxX, 1f);

        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
    }

    void UpdateComboUI() // 성급 출력
    {
        for (int i = 0; i < comboStars.Length; i++)
        {
            if (i < currentCombo)
            {
                comboStars[i].SetActive(true);
            }
            else
            {
                comboStars[i].SetActive(false);
            }

        }
    }

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
