using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 0.1f;

    Vector3 initialPosition;

    private void OnEnable()
    {
        initialPosition = transform.localPosition;
    }

    public void TriggerShake()
    {
        StopAllCoroutines(); // øÚ¡˜¿Ã∞Ì¿÷¥Ÿ∏È ∏ÿ√„
        StartCoroutine(Shake());

        if (VibrationManager.Instance != null)
        {
            VibrationManager.Instance.Vibrate();
        }
    }

    IEnumerator Shake()
    {
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = initialPosition;
    }
}
