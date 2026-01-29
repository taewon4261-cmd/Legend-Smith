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


        [Range(0.01f, 0.5f)]
        public float tolerance;
    }

    public List<LevelData> levels;

    public LevelData GetLevelData(int currentCombo)
    {
        int index = Mathf.Clamp(currentCombo, 0, levels.Count - 1); 
        return levels[index];
    }
}
