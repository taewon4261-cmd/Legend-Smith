using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 50f;

    private RectTransform rectTransform; 
    private Vector2 initialPosition;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    private void OnEnable()
    {
        if (rectTransform != null)
        {
            initialPosition = rectTransform.anchoredPosition;
        }
    }

    public void TriggerShake()
    {

        if (GameManager.Instance != null && !GameManager.Instance.Vibration.isVibrationOn)
        {
            return;
        }

        StopAllCoroutines(); // øÚ¡˜¿Ã∞Ì¿÷¥Ÿ∏È ∏ÿ√„
        StartCoroutine(Shake());

        if (GameManager.Instance != null)
        {
            GameManager.Instance.Vibration.Vibrate();
        }
    }

    IEnumerator Shake()
    {
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            Vector2 randomPoint = Random.insideUnitCircle * shakeMagnitude;
            rectTransform.anchoredPosition = initialPosition + randomPoint;

            elapsed += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = initialPosition;
    }
}
