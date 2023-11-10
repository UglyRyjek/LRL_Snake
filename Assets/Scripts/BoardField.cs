[System.Serializable]
public class BoardField
{
    public BoardField(int xPosition, int yPosition)
    {
        X = xPosition;
        Y = yPosition;
    }

    public new string ToString()
    {
        return $"{X} : {Y}";
    }

    public int X;
    public int Y;
}
