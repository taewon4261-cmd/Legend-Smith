using UnityEngine;
using UnityEngine.UI;

public class VibrationManager : MonoBehaviour
{
    public static VibrationManager Instance;

    public Toggle vibrateToggle;
    private bool isVibrationOn = true;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        int savedVal = PlayerPrefs.GetInt("VibrateOn", 1);
        isVibrationOn = (savedVal == 1);

        if (vibrateToggle != null)
        {
            vibrateToggle.isOn = isVibrationOn;

            vibrateToggle.onValueChanged.AddListener(OnVibrateToggle);
        }
    }

    public void OnVibrateToggle(bool isOn)
    {
        isVibrationOn = isOn;

        PlayerPrefs.SetInt("VibrateOn", isOn ? 1 : 0);
        PlayerPrefs.Save();

        if (isOn) Vibrate();
    }

    public void Vibrate()
    {

        if (!isVibrationOn) return;

        Handheld.Vibrate();
    }
}