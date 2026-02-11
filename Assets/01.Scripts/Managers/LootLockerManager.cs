using UnityEngine;
using LootLocker.Requests; // 이게 있어야 작동함

public class LootLockerManager : MonoBehaviour
{
    public static LootLockerManager Instance;

    private string currentPlayerID = "";

    void Awake()
    {
        // 싱글톤 패턴 (게임 내내 살아있게 함)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 게임 시작하자마자 로그인 시도!
        StartGuestSession();
    }

    public void StartGuestSession()
    {
        Debug.Log("로그인 시도 중...");

        string deviceId = SystemInfo.deviceUniqueIdentifier;

        LootLockerSDKManager.StartGuestSession(deviceId, (response) =>
        {
            if (!response.success)
            {
                Debug.LogError("로그인 실패 ㅠㅠ: " + response.errorData.message);
                return;
            }

            currentPlayerID = response.player_id.ToString();
            Debug.Log("로그인 성공!  Player ID: " + currentPlayerID);
        });
    }

    public void SubmitScore(string boardKey, int score)
    {
        if (LootLockerSDKManager.CheckInitialized() == false) return;

        // 1. 저장된 이름 가져오기 (없으면 "NoName"으로 강제 설정)
        string playerName = PlayerPrefs.GetString("PlayerName", "NoName");

        // 내 ID 가져오기
        string memberID = currentPlayerID;
        if (string.IsNullOrEmpty(memberID))
        {
            // 만약 ID가 없으면 로컬에 저장된 ID라도 가져옴
            memberID = PlayerPrefs.GetString("LootLockerGuestPlayerID", "");
        }

        int scoreToSend = ResourceManager.Instance.totalGoldEarned;


        Debug.Log($"[전송 시작] ID: {memberID} / 이름: {playerName} / 점수: {score}");

        //  [핵심 수정] 함수 인자 명시 (순서 꼬임 방지)
        // memberID: 내 아이디
        // score: 점수
        // leaderboardId: 랭킹판 키
        // metadata: 내 이름 (여기에 이름이 들어가야 함!)
        LootLockerSDKManager.SubmitScore(memberID, scoreToSend, boardKey, playerName, (response) =>
        {
            if (response.success)
            {
                Debug.Log($" [성공] 서버에 등록 완료! (이름: {playerName})");
            }
            else
            {
                Debug.LogError($" [실패] 서버 거부: {response.errorData.message}");
                Debug.LogError("힌트: 웹사이트 리더보드 설정에서 Metadata가 켜져있는지 확인 필요 (이미 켜셨으면 이 에러 아님)");
            }
        });
    }

    // 랭킹 불러오기 (key: 랭킹판 이름, count: 몇 명 불러올지)
    public void GetRanking(string boardKey, int count)
    {
        LootLockerSDKManager.GetScoreList(boardKey, count, (response) =>
        {
            if (response.success)
            {
                Debug.Log($" [{boardKey}] 랭킹 로드 성공!");

                // 1등부터 순서대로 출력해보기 (나중에 UI에 연결할 부분)
                LootLockerLeaderboardMember[] members = response.items;
                for (int i = 0; i < members.Length; i++)
                {
                    Debug.Log($"{members[i].rank}등: {members[i].player.id} - {members[i].score}점");
                }
            }
            else
            {
                Debug.LogError("랭킹 불러오기 실패");
            }
        });
    }

    public void SetPlayerName(string newName)
    {
        LootLockerSDKManager.SetPlayerName(newName, (response) =>
        {
            if (response.success)
            {
                Debug.Log("이름 변경 성공! : " + newName);
                // 이름 바꿨으니 랭킹판도 새로고침 해주면 좋음 (선택사항)
            }
            else
            {
                Debug.LogError("이름 변경 실패 ㅠㅠ: " + response.errorData.message);
            }
        });
    }
}