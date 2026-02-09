using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [Header("퀘스트 목록")]
    public List<QuestData> dailyQuests;

    private const string LastLoginKey = "LastLoinData_";

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        CheckDailyReset();

        StartCoroutine(PlayTimeCoroutine());
    }


    void CheckDailyReset()
    {
        string today = DateTime.Now.ToString("yyyyMMdd");
        string lastLogin = PlayerPrefs.GetString(LastLoginKey, "");

        if (lastLogin != today)
        {
            Debug.Log("퀘스트 시작");
            ResetAllQuests();

            PlayerPrefs.SetString(LastLoginKey, today);
        }
        else
        {
            LoadQuestProgress();
        }
    }

    void ResetAllQuests()
    {
        foreach (var quest in dailyQuests)
        {
            quest.currentAmount = 0;
            quest.isClaimed = false;
        }
        SaveQuestProgress();
    }

    public void NotifyQuestAction(QuestType type, int amount)
    {
        bool isChanged = false;

        foreach (var quest in dailyQuests)
        {
            if (quest.type == type && !quest.isClaimed && quest.currentAmount < quest.goalAmount)
            {
                quest.currentAmount += amount;

                if (quest.currentAmount > quest.goalAmount)
                    quest.currentAmount = quest.goalAmount;

                isChanged = true;
            }
        }
        if (isChanged)
        {
            SaveQuestProgress();
        }
    }

    public void ClaimReward(QuestData quest)
    {
        if (quest.currentAmount >= quest.goalAmount && !quest.isClaimed)
        {
            quest.isClaimed = true;
            ResourceManager.Instance.AddDia(quest.rewardDia);
            SFXManager.Instance.PlaySFX("Quest", 1);

            SaveQuestProgress();
        }
    }
    
    void SaveQuestProgress()
    {
        for (int i = 0; i < dailyQuests.Count; i++)
        {
            PlayerPrefs.SetInt($"Quest_{i}_Amount", dailyQuests[i].currentAmount);
            PlayerPrefs.SetInt($"Quest_{i}_Claimed", dailyQuests[i].isClaimed ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    void LoadQuestProgress()
    {
        for (int i = 0; i < dailyQuests.Count; i++)
        {
            dailyQuests[i].currentAmount = PlayerPrefs.GetInt($"Quest_{i}_Amount", 0);
            dailyQuests[i].isClaimed = PlayerPrefs.GetInt($"Quest_{i}_Claimed", 0) == 1;
        }
    }

    IEnumerator PlayTimeCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // 1초 대기

            // 1초 지날 때마다 퀘스트 매니저한테 "1초 지남" 신고
            if (QuestManager.Instance != null)
            {
                QuestManager.Instance.NotifyQuestAction(QuestType.PlayTime, 1);
            }
        }
    }
}
