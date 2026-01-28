using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject smithingPanel;

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
       
    }

    public void StartSmithing()
    {

    }
}
