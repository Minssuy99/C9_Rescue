internal class Quest
{


    public string Title { get; }
    public string Description { get; }
    public string Summary { get; }
    public string RewardItem { get; }
    public int RewardGold { get; }
    public bool IsAccepted { get; set; }
    public bool IsCompeleted { get; set; }

    public Quest(string title, string description, string summary, string rewardItem,
                                             int rewardGold, bool isAccepted = false, bool isCompleted = false)
    {
        Title = title;
        Description = description;
        Summary = summary;
        RewardItem = rewardItem;
        RewardGold = rewardGold;
        IsAccepted = isAccepted;
        IsCompeleted = isCompleted;
    }
}