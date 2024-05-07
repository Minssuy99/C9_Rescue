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
    public int CurrentAmount { get; set; }
    public int TargetAmount { get; }
    public int RewardGold { get; }
    public string TargetMonsterName { get; }
    public bool IsCompleted { get; private set; }
    public bool IsAccepted { get; set; }
    public bool IsRewarded { get; private set; }

    public Quest(string title, string description, QuestType type, QuestDifficulty difficulty, int targetAmount, int rewardGold, string targetMonsterName, bool isCompleted = false, bool isAccepted = false)
    {
        Title = title;
        Description = description;
        Type = type;
        Difficulty = difficulty;
        CurrentAmount = 0;
        TargetAmount = targetAmount;
        RewardGold = rewardGold;
        TargetMonsterName = targetMonsterName;
        IsCompleted = isCompleted;
        IsAccepted = isAccepted;
    }

    public void UpdateProgress(int amount)
    {
        CurrentAmount += amount; // ��� �� ������Ʈ
    }

    public string GetProgressText()
    {
        return $"{CurrentAmount}/{TargetAmount}"; // ���� ���� ��Ȳ
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

    public void CompleteQuest()
    {
        IsCompleted = true;
        IsAccepted = false;
    }

    public void RewardedQuest()
    {
        IsRewarded = true;
    }
}
