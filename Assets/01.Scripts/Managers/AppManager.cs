using UnityEngine;

public class AppManager : MonoBehaviour
{
    public GameObject quitPopup;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (quitPopup.activeSelf)
            {
                quitPopup.SetActive(false);
            }
            else
            {
                quitPopup.SetActive(true);
            }
        }
    }

    public void Init()
    {
        if(quitPopup != null) quitPopup.gameObject.SetActive(false);
    }
    public void OnClickYesQuit()
    {
        Application.Quit(); 
    }
    public void OnClickNoCancel()
    {
        quitPopup.SetActive(false);
    }
}