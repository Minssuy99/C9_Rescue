public class Quest
{
    public enum QuestType
    {
        Hunt,
        Collect,
        Equip
    }

    public enum QuestDifficulty
    {
        Easy,
        Normal,
        Hard
    }

    public string Title { get; }
    public string Description { get; }
    public QuestType Type { get; }
    public QuestDifficulty Difficulty { get; }
    public int TargetAmount { get; }
    public int RewardGold { get; }

    public Quest(string title, string description, QuestType type, QuestDifficulty difficulty, int targetAmount, int rewardGold)
    {
        Title = title;
        Description = description;
        Type = type;
        Difficulty = difficulty;
        TargetAmount = targetAmount;
        RewardGold = rewardGold;
    }

    public static string GetQuestTypeName(QuestType type)
    {
        return type switch
        {
            QuestType.Hunt => "���",
            QuestType.Collect => "����",
            QuestType.Equip => "����",
            _ => "����"
        };
    }

    public static string GetQuestDifficultyName(QuestDifficulty difficulty)
    {
        return difficulty switch
        {
            QuestDifficulty.Easy => "����",
            QuestDifficulty.Normal => "����",
            QuestDifficulty.Hard => "�����",
            _ => "����"
        };
    }
}
