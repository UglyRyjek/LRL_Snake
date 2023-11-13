using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

public class ApplicationFlow : MonoBehaviour
{
    [SerializeField] private BoardService _board;
    [SerializeField] private BoardPresenter _boardPresenter;
    [SerializeField] private BoardParameters _boardParameters;
    [SerializeField] private Snake _snake;
    [SerializeField] private SnakeInput _snakeInput;
    [SerializeField] private GameOverUI _gameOverUI;

    [SerializeField] private EdibleSpawner _edibleSpawner;

    private bool _gameOverFlag;

    private void Start()
    {
        InitializeApplication();
    }

    private void InitializeApplication()
    {
        _board = new BoardService(_boardParameters);
        _boardPresenter.InitializeBoard(_board);

        _snake.Initialize(_boardPresenter, _board.BoardModel, _snakeInput);

        InvokeSnakeTick();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _snake.AddPart();
        }

        if (_gameOverFlag == true)
        {
            return;
        }

        InvokeSnakeTick();



        if (Input.GetKeyDown(KeyCode.Z))
        {
            BoardField bf = GetFreeFieldOnBoard(_snake.SnakeParts);
            _edibleSpawner.SpawnEdible(bf, _boardPresenter);
        }
    }

    private void InvokeSnakeTick()
    {
        bool ticked = _snake.Tick();

        if(ticked == true)
        {
            bool gameOver = _snake.CheckIfCollisionHappened();
            if (gameOver)
            {
                Debug.Log("Game Over");
                _gameOverFlag = true;
                _gameOverUI.LoadGameOver();
            }

            var v = _edibleSpawner.BEPU(_snake.SnakeParts.First());
            if (v != null)
            {
                Debug.Log("EAT EDIBLE");
                v.EatenEffect(_snake);
                _edibleSpawner.EatEdible(v);

            }
        }
    }

    // assume there is always one
    private BoardField GetFreeFieldOnBoard(IReadOnlyList<BoardField> snake)
    {
        List<BoardField> freeFields = new List<BoardField>();

        // all fields on board
        _board.BoardModel.Fields.ForEach(x => freeFields.Add(x));

        // minus ones occupied by snake
        for (int i = 0; i < snake.Count; i++)
        {
            freeFields.Remove(snake[i]);
        }

        //TODO
        // minus already occupied by edibles

        //TODO
        // also exclude these that are too close too head
        BoardField snakeHead = snake.First();


        BoardField randomField = freeFields[Random.Range(0, freeFields.Count - 1)];
        return randomField;
    }
}


[System.Serializable]
public class AppProfile
{
    public int minimalSnakeSize = 1;
    
}