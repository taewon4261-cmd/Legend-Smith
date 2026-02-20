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

        Application.targetFrameRate = 60; //모바일 발열/배터리 최적화를 위한 프레임 고정

        if (!Application.isEditor) // 릴리즈 빌드 최적화 (불필요한 로그 출력 방지)
        {
            Debug.unityLogger.filterLogType = LogType.Error;
        }

        // 각 매니저 초기화
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

    /// <summary>
    /// 앱 일시정지, 종료 또는 특정 주기마다 호출해서 유저의 진행 상황을 저장
    /// </summary>
    public void SaveAllGameData()
    {
        if (Resource != null) Resource.SaveAllData();
        if (Inven != null) Inven.SaveInventory();
        if (Quest != null) Quest.SaveQuestProgress();
        PlayerPrefs.Save();
    }
     
    private void OnApplicationQuit()
    {
        SaveAllGameData();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            SaveAllGameData();
        }
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
