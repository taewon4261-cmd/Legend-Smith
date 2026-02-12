using UnityEngine;
using UnityEngine.UI;
using LootLocker.Requests;
using TMPro;

public class LeaderboardUI : MonoBehaviour
{
    [Header("연결할 것들")]
    public GameObject rankSlotPrefab; 
    public Transform contentParent;

    //랭킹 불러오기 버튼
    public void OnClickTab(string leaderboardKey)
    {
        if (rankSlotPrefab == null || contentParent == null) return;

        foreach (Transform child in contentParent) Destroy(child.gameObject);

        LootLockerSDKManager.GetScoreList(leaderboardKey, 100, (response) =>
        {
            if (response.success)
            {
                LootLockerLeaderboardMember[] members = response.items;

                if (members == null) return;

                for (int i = 0; i < members.Length; i++)
                {
                    if (members[i] == null) continue;

                    GameObject newSlot = Instantiate(rankSlotPrefab, contentParent);
                    RankSlot slotScript = newSlot.GetComponent<RankSlot>();

                    if (slotScript != null)
                    {
                        string pName = members[i].metadata;

                        if (string.IsNullOrEmpty(pName) && members[i].player != null)
                        {
                            pName = members[i].player.name;
                        }

                        if (string.IsNullOrEmpty(pName))
                        {
                            pName = members[i].member_id;
                        }

                        slotScript.SetUI(members[i].rank, pName, members[i].score);
                    }
                }
            }
        });
    }
}