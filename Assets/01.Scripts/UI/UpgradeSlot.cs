using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSlot : MonoBehaviour
{
    [Header("데이터")]
    public UpgradeDataSO data;

    [Header("UI")]
    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI effectText;
    public TextMeshProUGUI costText;
    public Button buyButton;

    private void Start()
    {
        if (data != null) Setup(data);

        buyButton.onClick.AddListener(OnClickBuy);
    }

    public void Setup(UpgradeDataSO newData)
    {
        data = newData;
        icon.sprite = data.Icon;
        nameText.text = data.upgradeName;
        RefreshUI();
    }

    void RefreshUI()
    {
        int level = GameManager.Instance.Upgrade.GetLevel(data);
        int cost = GameManager.Instance.Upgrade.GetCurrentCost(data);
        float curruentEffect = GameManager.Instance.Upgrade.GetTotalEffect(data);
        float nextEffect = (level + 1) * data.valuePerLevel;

        nameText.text = data.upgradeName;

        levelText.text = $"Lv.{level}";

        costText.text = $"{cost}G";

        string unit = ""; 

        switch (data.type)
        {
            case UpgradeType.MiningAmount:
                unit = "개";
                break;

            case UpgradeType.SellPrice:
            case UpgradeType.ComboBonus: 
                unit = "%";
                break;

            default:
                unit = ""; // 예외 처리
                break;
        }

        effectText.text = $" 현재: +{curruentEffect + 1}{unit}\n 다음 : +{nextEffect + 1}{unit}";
    }

    void OnClickBuy()
    {
        bool isSuccess = GameManager.Instance.Upgrade.TryUpgrade(data);
        if (isSuccess)
        {
            RefreshUI();
        }

    }
}
