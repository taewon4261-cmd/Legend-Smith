using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class SmithingUI : MonoBehaviour
{
    public Image targetZoneImage;
    public Slider timingSlider;

    private float targetPosition = 0.5f; // 중심 위치
    private float tolerance = 0.2f; // 난이도 조절

    public GameObject[] comboStars;
    private int currentCombo = 0;
    private int maxCombo = 5;

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

    public void CheckHit()
    {
        float currentVal = timingSlider.value; ;

        float diff = currentVal - targetPosition;
        float distance =  Mathf.Abs(diff);

        if (distance <= tolerance) // 성공
        {           
            currentCombo++;

            UpdateComboUI(); // 성공시 별 이미지 출력

            SmithingEffect(true).Forget(); // 이미지 바 색 변경으로 직관적 이미지 표현

            if (currentCombo >= maxCombo)
            {
                Debug.Log($"성공!");

                currentCombo = 0;

                UpdateComboUI();
            }
        }
        else // 실패
        {
            currentCombo = 0;
            UpdateComboUI(); // 실패 시 이미지 다 꺼짐
            SmithingEffect(false).Forget();
            Debug.Log($"실패...");
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
    void Start()
    {
       if (timingSlider != null)
        {
            timingSlider.value = 0;
        }

        UpdateComboUI(); // 별 갯수 0개로 초기화
    }

    void Update()
    {
        if (timingSlider != null)
        {
            timingSlider.value = Mathf.PingPong(Time.time * 1f,1f);
            
        }
    }
}
