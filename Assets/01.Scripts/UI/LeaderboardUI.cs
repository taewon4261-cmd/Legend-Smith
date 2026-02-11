using UnityEngine;
using UnityEngine.UI;
using LootLocker.Requests;
using TMPro;

public class LeaderboardUI : MonoBehaviour
{
    [Header("연결할 것들")]
    public GameObject rankSlotPrefab; 
    public Transform contentParent;

    [Header("탭 버튼 색상용 (선택사항)")]
    public Button[] tabButtons;

    // 랭킹 불러오기 (버튼에 연결할 함수)
    public void OnClickTab(string leaderboardKey)
    {
        if (rankSlotPrefab == null || contentParent == null) return;

        foreach (Transform child in contentParent) Destroy(child.gameObject);

        LootLockerSDKManager.GetScoreList(leaderboardKey, 100, (response) =>
        {
            if (response.success)
            {
                // Debug.Log(" 데이터 도착!"); // 이건 놔둬도 됨
                LootLockerLeaderboardMember[] members = response.items;

                if (members == null) return;

                for (int i = 0; i < members.Length; i++)
                {
                    if (members[i] == null) continue;

                    GameObject newSlot = Instantiate(rankSlotPrefab, contentParent);
                    RankSlot slotScript = newSlot.GetComponent<RankSlot>();

                    if (slotScript != null)
                    {
                        //  [최종 로직] 1.메타데이터 -> 2.플레이어이름 -> 3.ID 순서
                        string pName = members[i].metadata;

                        if (string.IsNullOrEmpty(pName) && members[i].player != null)
                        {
                            pName = members[i].player.name;
                        }

                        if (string.IsNullOrEmpty(pName))
                        {
                            pName = members[i].member_id;
                        }

                        // UI 적용
                        slotScript.SetUI(members[i].rank, pName, members[i].score);
                    }
                }
            }
            else
            {
                Debug.LogError("로드 실패: " + response.errorData.message);
            }
        });
    }
}