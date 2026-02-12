using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DiaStoreSlot : MonoBehaviour
{
    [Header("데이터")]
    public DiaStoreDataSO data;

    [Header("UI")]
    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI rewardText;
    public TextMeshProUGUI priceText;
    public Button buyButton;

    private void Start()
    {
        if (data != null) Setup(data);

        buyButton.onClick.AddListener(OnClickBtn);
    }

    public void Setup(DiaStoreDataSO newData)
    {
        data = newData;
        icon.sprite = data.icon;
        nameText.text = data.rewardName;
        rewardText.text = $"{data.reward}";
        priceText.text = $"{data.price} 다이아";

        string unit = "";
        switch (data.type)
        {
            case DiaStoreType.Gold:
                unit = "G";
                break;
            case DiaStoreType.Ore:
                unit = "Ore";
                break;
        }

        rewardText.text = $"{data.reward}{unit}";
    }


    public void OnClickBtn()
    {
        bool isSuccess = ResourceManager.Instance.TrySpendDia(data.price);

        if (isSuccess)
        {
            switch (data.type)
            {
                case DiaStoreType.Gold:
                    ResourceManager.Instance.AddGold(data.reward);
                    break;

                case DiaStoreType.Ore:
                    ResourceManager.Instance.AddOre(data.reward);
                    break;
            }

            // 성공 사운드
            SFXManager.Instance.PlaySFX("BuySellUpgrade", 1);
        }
        else
        {
            // 실패 사운드
            SFXManager.Instance.PlaySFX("OnClickBtnFail", 1);
        }

    }
}
