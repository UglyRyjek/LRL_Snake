using System.Collections;
using UnityEngine;

public class BoardService
{
    public BoardService(BoardParameters boardParameters)
    {
        _boardModel = new BoardModel();
        _boardModel.GenerateBoard(boardParameters);
    }

    [SerializeField] private BoardModel _boardModel;
    public BoardModel BoardModel => _boardModel;
}
