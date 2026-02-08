using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestSlotUI : MonoBehaviour
{
    [Header("데이터")]
    private QuestData myQuestData;

    [Header("UI 연결")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI progressText;
    public TextMeshProUGUI rewardText; 

    public Button claimButton; 
    public TextMeshProUGUI buttonText;

    private void Update()
    {
        // 데이터가 연결되어 있다면 계속 새로고침
        if (myQuestData != null)
        {
            RefreshUI();
        }
    }
    public void Setup(QuestData data)
    {
        myQuestData = data;
       
        claimButton.onClick.RemoveAllListeners();
        claimButton.onClick.AddListener(OnClickClaim);

        RefreshUI();
    }

    public void RefreshUI()
    {
        if (myQuestData == null) return;

        titleText.text = myQuestData.questName;
        progressText.text = $"{myQuestData.currentAmount} / {myQuestData.goalAmount}";
        rewardText.text = $"보상: {myQuestData.rewardDia} G";

        if (myQuestData.isClaimed)
        {
            claimButton.interactable = false; // 버튼 끄기
            buttonText.text = "완료됨";
        }
        else if (myQuestData.currentAmount >= myQuestData.goalAmount)
        {
            claimButton.interactable = true; // 버튼 켜기
            buttonText.text = "보상 받기";
        }
        else
        {
            claimButton.interactable = false; // 버튼 끄기
            buttonText.text = "진행중";
        }
    }

    void OnClickClaim()
    {
        QuestManager.Instance.ClaimReward(myQuestData);

        RefreshUI();
    }
}
