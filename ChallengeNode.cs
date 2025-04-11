public class Challenge
{
    public int Difficulty;
    public ChallengeType Type;
    public Challenge? Left;
    public Challenge? Right;

    public Challenge()
    {
        Difficulty = Random.Shared.Next(1, 21); // Random difficulty between 1 and 21
        Type = (ChallengeType)(Difficulty % 3); // Assign a type based on difficulty for demonstration
        Left = null;
        Right = null;
    }
}