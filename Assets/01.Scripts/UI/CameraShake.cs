using System.Collections;
using System.Collections.Generic;
using TreeEditor;
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
        StopAllCoroutines(); // 움직이고있다면 멈춤
        StartCoroutine(Shake()); 

        //유니티 내부 기능, 휴대폰에 진동 울리는 코드
        Handheld.Vibrate();
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
