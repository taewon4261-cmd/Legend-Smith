using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 0.5f;
    public float shakeMagnitude = 50f;

    private RectTransform rectTransform; 
    private Vector2 initialPosition;

    private CancellationTokenSource shakeCts;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    private void OnEnable()
    {
        shakeCts?.Cancel();
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

        shakeCts?.Cancel();
        shakeCts?.Dispose();
        shakeCts = new CancellationTokenSource();

        ShakeTask(shakeCts.Token).Forget();

        if (GameManager.Instance != null)
        {
            GameManager.Instance.Vibration.Vibrate();
        }
    }

    private async UniTaskVoid ShakeTask(CancellationToken token)
    {
        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            if (token.IsCancellationRequested) return;

            Vector2 randomPoint = Random.insideUnitCircle * shakeMagnitude;
            rectTransform.anchoredPosition = initialPosition + randomPoint;

            elapsed += Time.deltaTime;

            bool isCanceled = await UniTask.Yield(PlayerLoopTiming.Update, token).SuppressCancellationThrow();
            if (isCanceled) return;
        }

        if (!token.IsCancellationRequested)
        {
            rectTransform.anchoredPosition = initialPosition;
        }
    }

    private void OnDestroy()
    {
        shakeCts?.Cancel();
        shakeCts?.Dispose();
    }
}
