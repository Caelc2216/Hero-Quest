public class Edge
{
    public Room To;
    public int Requirement;
    public enum Stat { Strength = 0, Agility = 1, Intelligence = 2, None = 3 };
    public Stat RequirementStat { get; set; }
    
    public Edge(Room to)
    {
        Random r = new Random();
        To = to;
        Requirement = r.Next(1, 6);
        RequirementStat = (Stat)r.Next(0, 4);
    }
}
