using System.Collections.Generic;

[System.Serializable]
public class BoardModel
{
    private BoardParameters _parameters;

    private List<BoardField> _fields = new List<BoardField>();
    public List<BoardField> Fields => _fields;

    public void GenerateBoardData(BoardParameters parameters)
    {
        _parameters = parameters;

         _fields = new List<BoardField>();
        for (int x = 0; x < _parameters.Width; x++)
        {
            for (int y = 0; y < _parameters.Height; y++)
            {
                _fields.Add(new BoardField(x, y));
            }
        }
    }

    public BoardField GetNext(BoardField currentField, Direction direction)
    {
        int newX = currentField.X;
        int newY = currentField.Y;

        switch (direction)
        {
            case Direction.Up:
                newY = (currentField.Y + 1) % _parameters.Height;
                break;
            case Direction.Down:
                newY = (currentField.Y - 1 + _parameters.Height) % _parameters.Height;
                break;
            case Direction.Left:
                newX = (currentField.X - 1 + _parameters.Width) % _parameters.Width;
                break;
            case Direction.Right:
                newX = (currentField.X + 1) % _parameters.Width;
                break;
        }

        return _fields.Find(field => field.X == newX && field.Y == newY);
    }
}
