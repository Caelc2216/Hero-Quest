public class Edge
{
    public Room To;
    public int Distance;
    
    public Edge(Room to)
    {
        Random r = new Random(15);
        To = to;
        Distance = r.Next();
    }
}
