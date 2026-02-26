using UnityEngine;
using TMPro; // TextMeshPro 사용

public class RankSlot : MonoBehaviour
{
    // 인스펙터에서 직접 연결할 통들
    public TextMeshProUGUI rankText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI scoreText;

    // 데이터를 받아서 텍스트를 갱신하는 함수
    public void SetUI(int rank, string name, int score)
    {
        rankText.SetText("{0}등", rank);
        nameText.text = name;
        scoreText.SetText("{0}점", score);
    }
}