using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject smithingPanel;
    public GameObject smithingPanelOnBtn;

    private void Awake()
    {
        if (smithingPanel != null)
        {
            smithingPanel.gameObject.SetActive(false);
        }
    }

    public void ShowSmithingPanel()
    {
        if (smithingPanel != null)
        {
            smithingPanel.gameObject.SetActive(true);
        }
        smithingPanelOnBtn.SetActive(false);
    }

    public void StartSmithing()
    {
        // 원하는 무기 선택 후 버튼 클릭 가능
    }

    
}
