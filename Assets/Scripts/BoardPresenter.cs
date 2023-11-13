using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

public class BoardPresenter : MonoBehaviour
{
    private BoardService _boardReference;

    [SerializeField] private BaseFieldPresenter _boardFieldPrototype;

    private Dictionary<BoardField, BaseFieldPresenter> _keyValuePairs = new Dictionary<BoardField, BaseFieldPresenter>();

    public void InitializeBoard(BoardService boardService)
    {
        _boardReference = boardService;
        Initialize(boardService.BoardModel);
    }

    public BaseFieldPresenter GetFieldPresenter(BoardField bf)
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

    public BoardField GetNext(Direction d, BoardField cf)
    {
        return _boardReference.BoardModel.GetNext(cf, d);
    }

    public BoardField GetPlaceForSnakePart(BoardField bf)
    {
        if(bf == null)
        {
            return _keyValuePairs.First().Key;
        }

        if(_keyValuePairs.TryGetValue(bf, out BaseFieldPresenter v))
        {
            return bf;
        }
        else
        {
            Debug.LogError("Should never happened, application flow prevent it");
            return _keyValuePairs.First().Key;
        }
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
