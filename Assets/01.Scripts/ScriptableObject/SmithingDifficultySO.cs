using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SmithingDifficultyData", menuName = "Game / SmithingDifficultyData")]
public class SmithingDifficultySO : ScriptableObject
{
    [System.Serializable]
    public struct LevelData
    {
        public float speed;

        [Range(0.01f, 0.5f)] // 숫자 입력 실수방지
        public float tolerance;
    }

    public List<LevelData> levels;


    // 안전장치 레벨 데이터의 수보다 초과되지않도록 막음
    public LevelData GetLevelData(int currentCombo)
    {
        int index = Mathf.Clamp(currentCombo, 0, levels.Count - 1); 
        return levels[index];
    }
}
