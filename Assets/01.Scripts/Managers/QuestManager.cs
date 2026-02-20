using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [Header("Äù½ºÆ® ¸ñ·Ï")]
    public List<QuestData> dailyQuests;

    private const string LastLoginKey = "LastLoinData_";

    public void Init()
    {
        CheckDailyReset();
        PlayTimeTask(this.GetCancellationTokenOnDestroy()).Forget();
    }


    void CheckDailyReset()
    {
        string today = DateTime.Now.ToString("yyyyMMdd");
        string lastLogin = PlayerPrefs.GetString(LastLoginKey, "");

        if (lastLogin != today)
        {
            Debug.Log("Äù½ºÆ® ½ÃÀÛ");
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
            if (type != QuestType.PlayTime)
            {
                SaveQuestProgress();
            }
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
    
    public void SaveQuestProgress()
    { 
        for (int i = 0; i < dailyQuests.Count; i++)
        {
            PlayerPrefs.SetInt($"Quest_{i}_Amount", dailyQuests[i].currentAmount);
            PlayerPrefs.SetInt($"Quest_{i}_Claimed", dailyQuests[i].isClaimed ? 1 : 0);
        }
    }

    void LoadQuestProgress()
    {
        for (int i = 0; i < dailyQuests.Count; i++)
        {
            dailyQuests[i].currentAmount = PlayerPrefs.GetInt($"Quest_{i}_Amount", 0);
            dailyQuests[i].isClaimed = PlayerPrefs.GetInt($"Quest_{i}_Claimed", 0) == 1;
        }
    }

    private async UniTaskVoid PlayTimeTask(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            bool isCanceled = await UniTask.Delay(1000, cancellationToken: token).SuppressCancellationThrow();

            if (isCanceled) return;

            if (GameManager.Instance != null)
            {
                GameManager.Instance.Quest.NotifyQuestAction(QuestType.PlayTime, 1);
            }
        }
    }
}
