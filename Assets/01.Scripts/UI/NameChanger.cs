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
            LootLockerManager.Instance.SetPlayerName(inputName);
            PlayerPrefs.SetString("PlayerName", inputName);
            PlayerPrefs.Save();

            SubmitAllScores();
            StopAllCoroutines();
            StartCoroutine(RefreshRankingCoroutine());
        }
    }

    void SubmitAllScores()
    {
        //  °ñµå ·©Å· 
        int currentGold = ResourceManager.Instance.totalGoldEarned;
        LootLockerManager.Instance.SubmitScore("rank_gold", currentGold);

        //  ¹«±â ·©Å·
        int weaponScore = UnlockManager.Instance.GetHighestWeaponCost();
        LootLockerManager.Instance.SubmitScore("rank_weapon_tier", weaponScore);

        //  °­È­ ·©Å·
        int upgradeScore = UpgradeManager.Instance.GetAllTotalLevel();
        LootLockerManager.Instance.SubmitScore("rank_upgrade_total", upgradeScore);
    }

    IEnumerator RefreshRankingCoroutine()
    {
        yield return new WaitForSeconds(1.5f);

        if (leaderboardUI != null)
        {
            leaderboardUI.OnClickTab("rank_gold");
        }
    }
}