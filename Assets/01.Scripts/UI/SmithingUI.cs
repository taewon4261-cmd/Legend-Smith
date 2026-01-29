using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class SmithingUI : MonoBehaviour
{
    public SmithingDifficultySO difficultyData;

    private float currentSpeed;
    private float currentTolerance;

    public Image targetZoneImage;
    public Slider timingSlider;

    private float targetPosition = 0.5f; // 중심 위치
    private float tolerance = 0.1f; // 난이도 조절

    public GameObject[] comboStars;
    private int currentCombo = 0;
    private int maxCombo = 5;

    public GameObject choicePopup;
    private bool isPuased;

    public ParticleSystem hitEffect;

    void Start()
    {
        if (timingSlider != null)
        {
            timingSlider.value = 0;
        }

        UpdateComboUI(); // 별 갯수 0개로 초기화
        SetDifficulty();
    }

    void Update()
    {
        if (timingSlider == null || isPuased) return;
        if (timingSlider != null)
        {
            timingSlider.value = Mathf.PingPong(Time.time * currentSpeed, 1f);

        }
    }

    public void CheckHit()
    {
        float currentVal = timingSlider.value; ;

        float diff = currentVal - targetPosition;
        float distance =  Mathf.Abs(diff);

        if (distance <= tolerance) // 성공
        {           
            isPuased = true;

            SmithingEffect(true).Forget(); // 이미지 바 색 변경으로 직관적 이미지 표현
            if (hitEffect != null)
            {
                Debug.Log("스파크 튀냐?");
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
            UpdateComboUI(); // 실패 시 이미지 다 꺼짐
            SmithingEffect(false).Forget();

            AddToInventory();
            SetDifficulty();

            Debug.Log($"실패...");
        }
    }
    public void OnButtonChallenge() // 추가 도전버튼
    {
        choicePopup.SetActive(false);

        currentCombo++;
        SetDifficulty();
        UpdateComboUI() ;

        timingSlider.value = 0;

        isPuased = false;
    }

    public void OnButtonKeep() // 그만하기 버튼
    {
        choicePopup.SetActive(false);
        AddToInventory();
    }

    void AddToInventory()
    {
        // TODO : 인벤토리 넣기

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

    void UpdateComboUI()
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
