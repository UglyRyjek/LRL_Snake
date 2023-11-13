using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    [SerializeField] private Transform _headPartReference;
    [SerializeField] private Transform _bodyPartPrototype;

    private SnakeMovementProfile _snakeMovementProfile;
    private SnakeInput _input;
    private IBoard _board;
    private SnakePart _head;
    private List<SnakePart> _snakeParts = new List<SnakePart>();
    private Direction _currentDirection = Direction.Up;
    private float timer = 0f;
    private float _speedUpTimer;
    private float _speedMultiplayer = 1f;

    public IReadOnlyList<BoardField> SnakeParts => _snakeParts.Select(x => x.CurrentField).ToList();

    private float SnakeSpeed => _snakeMovementProfile.baseSnakeSpeed * _speedMultiplayer;

    public void Initialize(IBoard board, SnakeInput baseInput, SnakeMovementProfile moveProfile)
    {
        _board = board;
        _input = baseInput;
        _snakeMovementProfile = moveProfile;

        AddHead();
        AddPart();

        timer = _snakeMovementProfile.baseSnakeSpeed;
    }

    public void ChangeSpeedModifier(float newModifier, float buffTime)
    {
        _speedMultiplayer = newModifier;

        CancelInvoke();
        Invoke(nameof(ResetSpeed), buffTime);
    }

    public void Reverse()
    {
        Direction reveresedDirection = DirectionUtility.GetOpositeDirection(_currentDirection);
        ChangeSnakeHeadDirection(reveresedDirection);

        SnakePart tail = _snakeParts.Last();

        _head.PlaceIt(tail.CurrentField);
    }

    public void ChangeSnakeSize(int changeBy)
    {
        if (changeBy >= 1)
        {
            for (int i = 0; i < changeBy; i++)
            {
                AddPart();
            }
        }
        else
        if (changeBy < 0)
        {
            for (int i = 0; i < Mathf.Abs(changeBy); i++)
            {
                RemovePart();
            }
        }
    }

    public bool Tick()
    {
        timer += Time.deltaTime;
        if (timer >= SnakeSpeed)
        {
            CheckIfSnakeShouldChangeDirection();
            MoveSnake(_currentDirection);
            timer = 0f;

            return true;
        }

        return false;


        void CheckIfSnakeShouldChangeDirection()
        {
            Direction d = _input.GetInputDirection();
            if (d != Direction.None)
            {
                bool opositePressed = DirectionUtility.AreOpositeDirection(_currentDirection, d);
                if (opositePressed == false)
                {
                    ChangeSnakeHeadDirection(d);
                }
            }
        }

        void MoveSnake(Direction d)
        {
            BoardField previousHeadField = _head.CurrentField;

            // move head
            _head.PlaceIt(_board.GetNext(d, _head.CurrentField));

            // follow with the body
            for (int i = 1; i < _snakeParts.Count; i++)
            {
                BoardField temp = _snakeParts[i].CurrentField;
                _snakeParts[i].PlaceIt(previousHeadField);
                previousHeadField = temp;
            }
        }
    }

    public bool IsSnakeCollidedWithSomething()
    {
        if (ColisionWithSelfOccured())
        {
            return true;
        }

        return false;
    }

    private void ResetSpeed()
    {
        _speedMultiplayer = 1f;
    }
   
    private void AddPart()
    {
        Transform newPart = Instantiate(_bodyPartPrototype, _bodyPartPrototype.transform.parent).transform;
        newPart.gameObject.SetActive(true);

        SnakePart sp = new SnakePart(_head.CurrentField, newPart, _board);
        _snakeParts.Add(sp);
    }

    private void AddHead()
    {
        BoardField initalField = _board.GetInitialPlaceForSnakeHead();

        SnakePart sp = new SnakePart(initalField, _headPartReference, _board);
        _snakeParts.Add(sp);

        _head = _snakeParts.First();
    }

    private void RemovePart()
    {
        if (_snakeParts.Count > 1)
        {
            SnakePart lastPart = _snakeParts.Last();
            _snakeParts.Remove(lastPart);
            lastPart.DestroyIt();
        }
    }

    private void ChangeSnakeHeadDirection(Direction newDirection)
    {
        _currentDirection = newDirection;
    }

    private bool ColisionWithSelfOccured()
    {
        BoardField headField = _head.CurrentField;

        // Check if the head's current field is the same as any other part's field
        for (int i = 1; i < _snakeParts.Count; i++)
        {
            if (_snakeParts[i].CurrentField == headField)
            {
                return true;
            }
        }

        return false;
    }
   
    [System.Serializable]
    private class SnakePart
    {
        [SerializeField] private IBoard _bp;
        [SerializeField] private BoardField _field;
        [SerializeField] private Transform _gfx;

        private BoardField _currentField;
        public BoardField CurrentField
        {
            get
            {
                return _currentField;
            }
        }

        public SnakePart(BoardField f, Transform gfx, IBoard b)
        {
            _currentField = f;
            _gfx = gfx;
            _bp = b;
        }

        public void PlaceIt(BoardField currentField)
        {
            _currentField = currentField;

            _gfx.transform.position = _bp.GetFieldPresenter(_currentField).transform.position;
        }

        public void DestroyIt()
        {
            Destroy(_gfx.gameObject);
        }
    }
}
