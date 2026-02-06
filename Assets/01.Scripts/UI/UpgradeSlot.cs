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
        int level = UpgradeManager.Instance.GetLevel(data);
        int cost = UpgradeManager.Instance.GetCurrentCost(data);
        float curruentEffect = UpgradeManager.Instance.GetTotalEffect(data);
        float nextEffect = (level + 1) * data.valuePerLevel;

        nameText.text = data.upgradeName;

        levelText.text = $"Lv.{level}";

        effectText.text = $" 현재: + {curruentEffect + 1}\n 다음 : + {nextEffect + 1} ";

        costText.text = $"{cost}G";
    }

    void OnClickBuy()
    {
        bool isSuccess = UpgradeManager.Instance.TryUpgrade(data);
        if (isSuccess)
        {
            RefreshUI();
        }
        else
        {
            // 실패 소리 출력
        }

    }
}
