using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;


    [Header ("ÀÚ¿ø")]
    public int currentOre = 0;
    public int orePerSecond = 1;
    public int gold;
    public int diamond;

    [Header("UI")]
    public TextMeshProUGUI oreText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI diaText;

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
        UpdateAllUI();
        Income().Forget();
    }

    async UniTaskVoid Income()
    {
        while (true)
        {
            await UniTask.Delay(1000, cancellationToken: this.GetCancellationTokenOnDestroy());
            currentOre += orePerSecond;

            UpdateOreUI();
        }
    }

    public bool TrySpendOre(int amount)
    {
        if (currentOre >= amount)
        {
            currentOre -= amount;

            UpdateOreUI();

            return true;
        }
        return false;

    }

    public void AddGold(int amount)
    {
        gold += amount;
        UpdateGoldUI();
    }

    void UpdateAllUI()
    {
        UpdateOreUI();
        UpdateGoldUI();
        UpdateDiaUI();
    }
    void UpdateOreUI()
    {
        oreText.text = $"{currentOre}";
    }
    void UpdateGoldUI()
    {
        goldText.text = $"{gold}";
    }
    
    void UpdateDiaUI()
    {
        diaText.text = $"{diamond}";
    }
}
