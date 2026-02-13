using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("매니저 연결")]
    public InventoryManager Inven;
    public SoundManager Sound;
    public AppManager App;
    public LootLockerManager LootLocker;
    public VibrationManager Vibration;
    public SFXManager SFX;
    public PanelManager Panel;
    public QuestManager Quest;
    public UpgradeManager Upgrade;
    public UnlockManager Unlock;
    public ResourceManager Resource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Application.targetFrameRate = 60; // 프레임 설정

        if (!Application.isEditor) // 최적화
        {
            Debug.unityLogger.filterLogType = LogType.Error;
        }

        if (Sound != null) Sound.Init();
        if (Inven != null) Inven.Init();
        if (App != null) App.Init();
        if (LootLocker != null) LootLocker.Init();
        if( Vibration != null) Vibration.Init();
        if (SFX != null) SFX.Init();
        if (Panel != null) Panel.InitPanels();
        if (Quest != null) Quest.Init();
        if(Upgrade != null) Upgrade.Init();
        if (Unlock != null) Unlock.Init();
        if(Resource != null) Resource.Init();
    }

    public void OnClickExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
Application.Quit();
#endif
    }
}
