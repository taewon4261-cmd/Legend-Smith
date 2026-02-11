using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class NameChanger : MonoBehaviour
{
    public TMP_InputField nameInput;
    public LeaderboardUI leaderboardUI;

    public void OnClickChangeName()
    {
        string inputName = nameInput.text;

        if (inputName.Length > 0)
        {
            // 1. 이름 변경 및 로컬 저장
            LootLockerManager.Instance.SetPlayerName(inputName);
            PlayerPrefs.SetString("PlayerName", inputName);
            PlayerPrefs.Save();

            // 2. 모든 랭킹판에 새 이름을 등록하기 위해 점수들 다시 전송
            SubmitAllScores();

            // 3. 코루틴 호출 (StartCoroutine을 사용해야 합니다!)
            StopAllCoroutines(); // 중복 방지
            StartCoroutine(RefreshRankingCoroutine());
        }
    }

    void SubmitAllScores()
    {
        //  골드 랭킹 (ResourceManager)
        int currentGold = ResourceManager.Instance.totalGoldEarned;
        LootLockerManager.Instance.SubmitScore("rank_gold", currentGold);

        //  무기 티어 (UnlockManager)
        int weaponScore = UnlockManager.Instance.GetHighestWeaponCost();
        LootLockerManager.Instance.SubmitScore("rank_weapon_tier", weaponScore);

        //  강화 횟수 (UpgradeManager)
        int upgradeScore = UpgradeManager.Instance.GetAllTotalLevel();
        LootLockerManager.Instance.SubmitScore("rank_upgrade_total", upgradeScore);
    }

    IEnumerator RefreshRankingCoroutine()
    {
        // 서버가 이름을 처리하고 랭킹을 재계산할 시간을 줍니다 (1.5초 정도가 안전합니다)
        yield return new WaitForSeconds(1.5f);

        if (leaderboardUI != null)
        {
            // 현재 유저가 보고 있는 탭을 새로고침 (여기서는 예시로 rank_gold)
            // 실제로는 현재 열려있는 탭의 변수를 받아서 넣는 것이 좋습니다.
            leaderboardUI.OnClickTab("rank_gold");
            Debug.Log("이름 변경 후 모든 랭킹판 정보 갱신 요청 완료");
        }
    }
}