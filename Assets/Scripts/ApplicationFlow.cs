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
    [SerializeField] private SnakeInput _snakeInput;
    [SerializeField] private GameOverUI _gameOverUI;

    [SerializeField]
    private float _tickTime;

    private float timer;
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

        InvokeTick();
    }

    

    private void Update()
    {
        WaitForTimerToTick();
    }

    private void WaitForTimerToTick()
    {
        if(_gameOverFlag == true)
        {
            return;
        }

        timer += Time.deltaTime;

        if(timer >= _tickTime)
        {
            InvokeTick();
            timer = 0f;
        }
    }

    private void InvokeTick()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            _snake.AddPart();
        }

        _snake.Tick();

        bool gameOver = _snake.CheckIfCollisionHappened();

        if(gameOver)
        {
            Debug.Log("Game Over");
            _gameOverFlag = true;
            _gameOverUI.LoadGameOver();
        }
    }
}