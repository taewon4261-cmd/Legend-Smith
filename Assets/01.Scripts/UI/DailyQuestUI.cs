using UnityEngine;
using System.Collections.Generic;

public class DailyQuestUI : MonoBehaviour
{
    public QuestSlotUI[] slots;

    private void OnEnable()
    {
        RefreshAllSlots();
    }

    public void RefreshAllSlots()
    {
        if (QuestManager.Instance == null) return;

        List<QuestData> questList = QuestManager.Instance.dailyQuests;

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < questList.Count)
            {
                slots[i].gameObject.SetActive(true);
                slots[i].Setup(questList[i]);
            }
            else
            {
                slots[i].gameObject.SetActive(false);
            }
        }
    }
}