
[System.Serializable]
public class QuestData
{
    public string questName;
    public QuestType type;
    public int goalAmount;
    public int rewardDia;

    public int currentAmount;
    public bool isClaimed;
}

public enum QuestType
{

    PlayTime,
    Smithing,
    Sell,
    Rarity
}