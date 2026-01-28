using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SmithingUI : MonoBehaviour
{
    //public Image slideImage;
    public Slider timingSlider;

    private float targetPositio = 0.5f; // 중심 위치
    private float tolerance = 0.2f; // 난이도 조절

    private bool isSuccess;

    public void CheckHit()
    {
        Time.timeScale = 0f;

        float currentVal = timingSlider.value; ;

        float diff = currentVal - targetPositio;
        float distance =  Mathf.Abs(diff);


        if (distance <= tolerance)
        {
            isSuccess = true;
            Debug.Log($"성공! 거리 : {distance}");
        }
        else
        {
            isSuccess = false;
            Debug.Log($"실패... 거리 : {distance}");
        }

        Time.timeScale = 1f;

    }


    void Start()
    {
       if (timingSlider != null)
        {
            timingSlider.value = 0;
        }
    }

    void Update()
    {
        if (timingSlider != null)
        {
            timingSlider.value = Mathf.PingPong(Time.time * 1f,1f);
        }
    }
}
