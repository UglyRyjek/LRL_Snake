using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;
using SSnake.GameSession;

public class ApplicationFlow : MonoBehaviour
{
    [FoldoutGroup("Dependencie")]
    [SerializeField] private MonoBoard _board;
    [FoldoutGroup("Dependencie")]
    [SerializeField] private SnakeController _snake;
    [FoldoutGroup("Dependencie")]
    [SerializeField] private SnakeInput _snakeInput;
    [FoldoutGroup("Dependencie")]
    [SerializeField] private EdibleSpawner _edibleSpawner;
    [FoldoutGroup("Dependencie")]
    [SerializeField] private ParametersProfile _parametersProfile;

    private void Start()
    {
        InitializeApplication();
    }

    private void InitializeApplication()
    {
        OnFlowStart();

        _board.GenerateBoard(_parametersProfile.boardProfile.boardSize);
        _edibleSpawner.Initialized(_board, _parametersProfile.spawningProfile);
        _snake.Initialize(_board, _snakeInput, _parametersProfile.snakeProfile);
    }

    private void Update()
    {
        ApplicationFlowSequence();
    }

    private void ApplicationFlowSequence()
    {
        if (GameSessionService.I.SessionData.State == SessionState.GameOver)
        {
            return;
        }

        bool ticked = _snake.Tick();
        if(ticked == true)
        {
            bool gameOver = _snake.IsSnakeCollidedWithSomething();
            if (gameOver)
            {
                OnGameOver();
            }
        }

        _edibleSpawner.SolveEdibleEating(_snake, OnEdibleEaten);
        _edibleSpawner.SolveEdibleSpawning(_snake);
    }

    private void OnFlowStart()
    {
        GameSessionService.I.StartSession();
    }

    private void OnEdibleEaten(BaseEdible powerUp)
    {
        GameSessionService.I.AddEdible();
    }

    private void OnGameOver()
    {
        GameSessionService.I.EndSession();
    }
}
