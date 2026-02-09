using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("버튼 클릭 시 켜질 패널")]
    public GameObject smithingPanel;
    public GameObject weaponStorePanel;
    public GameObject storePanel;
    public GameObject upQuestPanel;
    

    [Header("메인화면 버튼")]
    public GameObject smithingPanelOnBtn;
    public GameObject weaponStorePanelBtn;
    public GameObject storePanelBtn;
    public GameObject upQuestPanelBtn;

    [Header("상점 내부 버튼")]
    public GameObject upgradePanel;
    public GameObject diaStorePanel;

    private void Awake()
    {
        //모든 서브 패널 비활성화 함수
        InitPanels();
    }

    public void ShowSmithingPanel()
    {
        if (smithingPanel != null)
        {
            smithingPanel.gameObject.SetActive(true);
        }
        MainPanelOff();
    }

    public void ShowWeaponStorePanel()
    {
        if (weaponStorePanel != null)
        {
            weaponStorePanel.gameObject.SetActive(true);
        }
        MainPanelOff();
    }
    public void ShowStorePanel()
    {
        if (storePanel != null)
        {
            storePanel.gameObject.SetActive(true);
        }
        MainPanelOff();
    }
    public void ShowQuestPanel()
    {
        if (upQuestPanel != null)
        {
            upQuestPanel.gameObject.SetActive(true);
        }
        MainPanelOff();
    }

    void MainPanelOff()
    {
        smithingPanelOnBtn.SetActive(false);
        weaponStorePanelBtn.SetActive(false);
        storePanelBtn.SetActive(false);
        upQuestPanelBtn.SetActive(false);
    }

    public void GoMain()
    {
        if (smithingPanel != null) smithingPanel.SetActive(false);
        if (weaponStorePanel != null) weaponStorePanel.SetActive(false);
        if(storePanel != null) storePanel.SetActive(false);
        if(upQuestPanel != null) upQuestPanel.SetActive(false);

        smithingPanelOnBtn.SetActive(true);
        weaponStorePanelBtn.SetActive(true);
        storePanelBtn.SetActive(true);
        upQuestPanelBtn.SetActive(true);
    }

    // 시작시 서브 패널들 비활성화
    private void InitPanels()
    {
        if (smithingPanel != null) smithingPanel.SetActive(false);
        if (weaponStorePanel != null) weaponStorePanel.SetActive(false);
        if(storePanel != null) storePanel.SetActive(false);
        if(upQuestPanel != null) upQuestPanel.SetActive(false);
    }

    public void ShowUpgradePanel()
    {
        if (upgradePanel != null)
        {
            upgradePanel.gameObject.SetActive(true);
        }
        diaStorePanel.gameObject.SetActive(false);
    }
    public void ShowDiaStorePanel()
    {
        if (diaStorePanel != null)
        {
            diaStorePanel.gameObject.SetActive(true);
        }
        upgradePanel.gameObject.SetActive(false);
    }
}
