using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ApplicationFlow : MonoBehaviour
{
    [SerializeField] private BoardService _board;
    [SerializeField] private BoardPresenter _boardPresenter;

    [SerializeField] private BoardParameters _boardParameters;
    [SerializeField] private Snake _snake;


    private void Start()
    {
        _board = new BoardService(_boardParameters);
        _boardPresenter.InitializeBoard(_board);

        _snake.Initialize(_boardPresenter, _board.BoardModel);
    }
}
