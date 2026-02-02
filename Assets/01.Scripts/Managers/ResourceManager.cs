using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;


    [Header ("ÀÚ¿ø")]
    public int currentIron = 0;
    public int ironPerSecond = 1;
    public int gold;
    public int diamond;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        Income().Forget();
    }


    async UniTaskVoid Income()
    {
        while (true)
        {
            currentIron += ironPerSecond;
            await UniTask.Delay(1000, cancellationToken: this.GetCancellationTokenOnDestroy());
        }
    }

    public bool TrySpendIron(int amount)
    {
        if (currentIron >= amount)
        {
            currentIron -= amount;
            return true;
        }
        return false;

    }
}
