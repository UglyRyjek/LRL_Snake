using System.Collections;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

[System.Serializable]
public class BoardService
{
    public BoardService(BoardParameters boardParameters)
    {
        _boardModel = new BoardModel();
        _boardModel.GenerateBoard(boardParameters);
    }

    [SerializeField] private BoardModel _boardModel;
    public BoardModel BoardModel => _boardModel;

    public BoardField GetFreeFieldOnBoard(IReadOnlyList<BoardField> snake, IReadOnlyList<BoardField> activeEdibles)
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
}
