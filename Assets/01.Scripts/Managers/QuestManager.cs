using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [Header("퀘스트 목록")]
    public List<QuestData> dailyQuests;

    private const string LastLoginKey = "LastLoinData_";

    public void Init()
    {
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
            GameManager.Instance.Resource.AddDia(quest.rewardDia);
            GameManager.Instance.SFX.PlaySFX("Quest", 1);

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
            if (GameManager.Instance != null)
            {
                GameManager.Instance.Quest.NotifyQuestAction(QuestType.PlayTime, 1);
            }
        }
    }
}
