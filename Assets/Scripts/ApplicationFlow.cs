using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

public class ApplicationFlow : MonoBehaviour
{

    [FoldoutGroup("Dependencie")]
    [SerializeField] private BoardService _board;
    [FoldoutGroup("Dependencie")]
    [SerializeField] private BoardPresenter _boardPresenter;
    [FoldoutGroup("Dependencie")]
    [SerializeField] private BoardParameters _boardParameters;
    [FoldoutGroup("Dependencie")]
    [SerializeField] private Snake _snake;
    [FoldoutGroup("Dependencie")]
    [SerializeField] private SnakeInput _snakeInput;
    [FoldoutGroup("Dependencie")]
    [SerializeField] private EdibleSpawner _edibleSpawner;

    private void Start()
    {
        InitializeApplication();
    }

    private void InitializeApplication()
    {
        OnFlowStart();

        _board = new BoardService(_boardParameters);
        _boardPresenter.InitializeBoard(_board);
        _edibleSpawner.Initialized(_board, _boardPresenter);
        _snake.Initialize(_boardPresenter, _board.BoardModel, _snakeInput);
    }

    private void Update()
    {
        ApplicationFlowSequence();
    }

    private void ApplicationFlowSequence()
    {
        if (GameSessionService.I.SessionData.State == SessionData.SessionState.GameOver)
        {
            return;
        }

        bool ticked = _snake.Tick();
        if(ticked == true)
        {
            bool gameOver = _snake.CheckIfCollisionHappened();
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

    private void OnEdibleEaten(BaseEdiblePowerUp powerUp)
    {
        GameSessionService.I.AddEdible();
    }

    private void OnGameOver()
    {
        GameSessionService.I.EndSession();
    }
}

public class GameSessionService
{
    private static GameSessionService i;

    public static GameSessionService I
    {
        get
        {
            if (i == null)
            {
                i = new GameSessionService();
            }

            return i;
        }
    }

    public SessionData SessionData { get; private set; }

    public void StartSession()
    {
        SessionData = new SessionData();
        SessionData.SetSessionState(SessionData.SessionState.Playing);
    }

    public void EndSession()
    {
        SessionData.SetSessionState(SessionData.SessionState.GameOver);
    }

    public void AddEdible()
    {
        SessionData.AddEdible();
    }
}

[System.Serializable]
public class SessionData
{
    public SessionData()
    {
        eatenEdibles = 0;
        startTime = Time.time;
        sessionState = SessionState.Playing;
    }

    public void AddEdible()
    {
        eatenEdibles++;
    }

    public void SetSessionState(SessionState state)
    {
        sessionState = state;
    }

    private SessionState sessionState;
    private float startTime;
    private int eatenEdibles;

    public int EatenEdibles => eatenEdibles;
    public float GameTime => Time.time - startTime;
    public SessionState State => sessionState;

    [System.Serializable]
    public enum SessionState
    {
        Playing = 0,
        GameOver = 1,
    }
}