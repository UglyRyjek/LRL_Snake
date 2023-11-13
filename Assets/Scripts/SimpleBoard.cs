using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

public class SimpleBoard : MonoBoard
{
    [SerializeField] private BaseFieldPresenter _boardFieldPrototype;

    private Dictionary<BoardField, BaseFieldPresenter> _keyValuePairs = new Dictionary<BoardField, BaseFieldPresenter>();

    private BoardModel _boardModel;
    public BoardModel BoardModel => _boardModel;

    public override void GenerateBoard(BoardParameters boardParameters)
    {
        _boardModel = new BoardModel();
        _boardModel.GenerateBoardData(boardParameters);
        Initialize(BoardModel);
    }

    public override BaseFieldPresenter GetFieldPresenter(BoardField bf)
    {
        if (_keyValuePairs.TryGetValue(bf, out BaseFieldPresenter v))
        {
            return v;
        }
        else
        {
            return null;
        }
    }

    public override BoardField GetNext(Direction d, BoardField cf)
    {
        return BoardModel.GetNext(cf, d);
    }

    public override BoardField GetInitialPlaceForSnakeHead()
    {
        return _keyValuePairs.First().Key;
    }

    public override BoardField GetFreeFieldOnBoard(IReadOnlyList<BoardField> snake, IReadOnlyList<BoardField> activeEdibles)
    {
        List<BoardField> freeFields = new List<BoardField>();

        // all fields on board
        BoardModel.Fields.ForEach(x => freeFields.Add(x));

        // minus ones occupied by snake
        for (int i = 0; i < snake.Count; i++)
        {
            freeFields.Remove(snake[i]);
        }

        // minus ones occupied by edibles already
        for (int i = 0; i < activeEdibles.Count; i++)
        {
            freeFields.Remove(activeEdibles[i]);
        }

        //TODO
        // lets asume we dont need / want this rule yet
        // also exclude these that are too close too head
        BoardField snakeHead = snake.First();

        BoardField randomField = freeFields[Random.Range(0, freeFields.Count - 1)];
        return randomField;
    }

    private void Initialize(BoardModel model)
    {
        foreach (BoardField field in model.Fields)
        {
            float x = field.X;
            float y = field.Y;
            Vector2 position = new Vector2(x * _boardFieldPrototype.transform.localScale.x, y * _boardFieldPrototype.transform.localScale.y);
            BaseFieldPresenter go = Instantiate(_boardFieldPrototype, position, Quaternion.identity, _boardFieldPrototype.transform.parent);
            go.gameObject.SetActive(true);
            go.Initialize(field);

            _keyValuePairs.Add(field, go);
        }
    }
}

public interface IBoard
{
    public void GenerateBoard(BoardParameters boardParameters);

    public BaseFieldPresenter GetFieldPresenter(BoardField bf);

    public BoardField GetNext(Direction d, BoardField cf);

    public BoardField GetInitialPlaceForSnakeHead();

    public BoardField GetFreeFieldOnBoard(IReadOnlyList<BoardField> snake, IReadOnlyList<BoardField> activeEdibles);
}

public abstract class MonoBoard : MonoBehaviour, IBoard
{
    public abstract void GenerateBoard(BoardParameters boardParameters);

    public abstract BaseFieldPresenter GetFieldPresenter(BoardField bf);

    public abstract BoardField GetFreeFieldOnBoard(IReadOnlyList<BoardField> snake, IReadOnlyList<BoardField> activeEdibles);

    public abstract BoardField GetInitialPlaceForSnakeHead();

    public abstract BoardField GetNext(Direction d, BoardField cf);
}