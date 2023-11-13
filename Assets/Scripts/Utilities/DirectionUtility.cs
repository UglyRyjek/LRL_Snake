[System.Serializable]
public class DirectionUtility
{
    public static bool AreOpositeDirection(Direction a, Direction b)
    {
        if (a == Direction.Up && b == Direction.Down) return true;
        if (a == Direction.Down && b == Direction.Up) return true;
        if (a == Direction.Right && b == Direction.Left) return true;
        if (a == Direction.Left && b == Direction.Right) return true;

        return false;
    }

    public static Direction GetOpositeDirection(Direction a)
    {
        switch (a)
        {
            case Direction.None: return Direction.None;
            case Direction.Up: return Direction.Down;
            case Direction.Down: return Direction.Up;
            case Direction.Left: return Direction.Right;
            case Direction.Right: return Direction.Left;
            default: return Direction.None;
        }
    }

}