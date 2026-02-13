using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [Header("오디오 믹서 연결")]
    public AudioMixer mainMixer;

    [Header("UI 슬라이더 연결")]
    public Slider masterSlider;

    public void Init()
    { 
        float savedVol = PlayerPrefs.GetFloat("MasterVol", 1.0f);

        if (masterSlider != null)
        {
            masterSlider.value = savedVol;
            masterSlider.onValueChanged.AddListener(SetMasterVolume);
        }

        SetMasterVolume(savedVol);
    }

    public void SetMasterVolume(float sliderVal)
    {
        // 핵심 공식: 슬라이더(0~1)를 데시벨(-80~0)로 변환
        // Mathf.Log10(0)은 에러나니까 0.0001f로 안전장치
        float dbVolume = Mathf.Log10(Mathf.Max(sliderVal, 0.0001f)) * 20;

        mainMixer.SetFloat("Master", dbVolume);

        PlayerPrefs.SetFloat("MasterVol", sliderVal);
    }
}