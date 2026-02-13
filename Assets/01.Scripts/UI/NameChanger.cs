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
            GameManager.Instance.LootLocker.SetPlayerName(inputName);
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
        int currentGold = GameManager.Instance.Resource.totalGoldEarned;
        GameManager.Instance.LootLocker.SubmitScore("rank_gold", currentGold);

        //  ¹«±â ·©Å·
        int weaponScore = GameManager.Instance.Unlock.GetHighestWeaponCost();
        GameManager.Instance.LootLocker.SubmitScore("rank_weapon_tier", weaponScore);

        //  °­È­ ·©Å·
        int upgradeScore = GameManager.Instance.Upgrade.GetAllTotalLevel();
        GameManager.Instance.LootLocker.SubmitScore("rank_upgrade_total", upgradeScore);
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