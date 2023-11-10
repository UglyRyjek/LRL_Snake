using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Snake : MonoBehaviour
{
    private BoardField _currentHeadPart;
    private BoardModel _board;
    private BoardPresenter _boardPresenter;
    [SerializeField] private Transform _snakeHeadPresenter;
    [SerializeField] private List<Transform> _snakeParts = new List<Transform>();

    public GameObject snakePartPrefab; // Assign in the Unity Editor

    [Button]
    public void AddPart()
    {
        Transform newPart = Instantiate(snakePartPrefab, _snakeHeadPresenter.position, Quaternion.identity, snakePartPrefab.transform.parent).transform;
        newPart.gameObject.SetActive(true);
        _snakeParts.Add(newPart);
    }

    [Button]
    public void RemovePart()
    {
        if (_snakeParts.Count > 1)
        {
            Transform lastPart = _snakeParts[_snakeParts.Count - 1];
            _snakeParts.Remove(lastPart);
            Destroy(lastPart.gameObject);
        }
    }

    public void Initialize(BoardPresenter bp, BoardModel bm)
    {
        _boardPresenter = bp;
        _board = bm;

        BoardField initalField = _boardPresenter.GetPlaceForSnakePart(null);
        _currentHeadPart = initalField;
        MoveSnakeHeadTo(_currentHeadPart);
    }

    public void MoveSnakeHeadTo(BoardField bf)
    {
        _currentHeadPart = _boardPresenter.GetPlaceForSnakePart(_currentHeadPart);
        SnakeMovementUpdate();
    }

    public void SnakeMovementUpdate()
    {
        Vector3 previousPosition = _snakeHeadPresenter.transform.position;

        BaseFieldPresenter bfp = _boardPresenter.GetFieldPresenter(_currentHeadPart);
        _snakeHeadPresenter.transform.position = bfp.transform.position;



        for (int i = 0; i < _snakeParts.Count; i++)
        {
            Vector3 temp = _snakeParts[i].position;
            _snakeParts[i].position = previousPosition;
            previousPosition = temp;
        }
    }

    [Button("Up")]
    private void MoveUp()
    {
        SimulateInput(Direction.Up);
    }

    [Button("Down")]
    private void MoveDown()
    {
        SimulateInput(Direction.Down);
    }

    [Button("Right")]
    private void MoveRight()
    {
        SimulateInput(Direction.Right);
    }

    [Button("Left")]
    private void MoveLeft()
    {
        SimulateInput(Direction.Left);
    }

    private void SimulateInput(Direction d)
    {
        if(_boardPresenter != null)
        {
            _currentHeadPart = _boardPresenter.GetNext(d, _currentHeadPart);
            SnakeMovementUpdate();
        }
    }
}
