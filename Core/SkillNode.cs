public class SkillNode
{
    public string Id;
    public string Name;
    public int Cost;
    public List<string> Prerequisites = new List<string>();
    public bool IsUnlocked;

    public SkillNode(string id, string name)
    {
        Id = id;
        Name = name;
    }
}
