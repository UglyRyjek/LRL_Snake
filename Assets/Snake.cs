using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Linq;

public class Snake : MonoBehaviour
{
    //private BoardField _currentHeadPart;
    private BoardPresenter _boardPresenter;

    //[SerializeField] private SnakePart _head;
    [SerializeField] private Transform _headPartReference;
    [SerializeField] private Transform _bodyPartPrototype;
    [SerializeField] private List<SnakePart> _snakeParts = new List<SnakePart>();

    [SerializeField, ReadOnly] private SnakeInput _input;

    private SnakePart _head;
    private Direction _currentDirection = Direction.Up;

    // while use of BoardField is not optimal it allows to do the task quickly and is flexible to modify if requirements will change

    [System.Serializable]
    public class SnakePart
    {
        public SnakePart(BoardField f, Transform gfx, BoardPresenter b)
        {
            _currentField = f;
            _gfx = gfx;
            _bp = b;
        }

        [SerializeField] private BoardPresenter _bp;
        [SerializeField] private BoardField _field;
        [SerializeField] private Transform _gfx;

        [SerializeField, ReadOnly]
        private BoardField _currentField;
        public BoardField CurrentField
        {
            get
            {
                return _currentField;
            }
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

    [Button]
    public void Reverse()
    {
        Direction reveresedDirection = GetOpositeDirection(_currentDirection);
        ChangeSnakeHeadDirection(reveresedDirection);

        SnakePart tail = _snakeParts.Last();

        _head.PlaceIt(tail.CurrentField);
    }

    public void AddPart()
    {
        Transform newPart = Instantiate(_bodyPartPrototype, _bodyPartPrototype.transform.parent).transform;
        //Debug.Log("Move it where it should be");
        newPart.gameObject.SetActive(true);

        SnakePart sp = new SnakePart(_head.CurrentField, newPart, _boardPresenter);
        _snakeParts.Add(sp);
    }

    private void AddHead()
    {
        BoardField initalField = _boardPresenter.GetPlaceForSnakePart(null);

        SnakePart sp = new SnakePart(initalField, _headPartReference, _boardPresenter);
        _snakeParts.Add(sp);

        _head = _snakeParts.First();
    }

    public void RemovePart()
    {
        // asume that head cannot be eaten down
        if (_snakeParts.Count > 1)
        {
            SnakePart lastPart = _snakeParts.Last();
            _snakeParts.Remove(lastPart);
            lastPart.DestroyIt();
        }
    }

    public void Initialize(BoardPresenter bp, BoardModel bm, SnakeInput input)
    {
        _boardPresenter = bp;
        _input = input;

        //asume that snake starts with head + 1 segment
        AddHead();
        AddPart();
    }

    public void Tick()
    {
        CheckIfSnakeShouldChangeDirection();

        MoveSnake(_currentDirection);

        CheckIfCollisionHappened();

        void CheckIfSnakeShouldChangeDirection()
        {
            Direction d = _input.GetInputDirection();
            if (d != Direction.None)
            {
                //asume that we ignore pressing oposite direcion
                bool opositePressed = AreOpositeDirection(_currentDirection, d);
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
            _head.PlaceIt(_boardPresenter.GetNext(d, _head.CurrentField));

            // follow with the body
            for (int i = 1; i < _snakeParts.Count; i++)
            {
                BoardField temp = _snakeParts[i].CurrentField;
                _snakeParts[i].PlaceIt(previousHeadField);
                previousHeadField = temp;
            }
        }

        void CheckIfCollisionHappened()
        {
            if (ColisionWithSelfOccured())
            {
                Debug.LogError("Bada boom");
            }

        }
    }

    private void ChangeSnakeHeadDirection(Direction newDirection)
    {
        _currentDirection = newDirection;
    }

    public bool ColisionWithSelfOccured()
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

    private static bool AreOpositeDirection(Direction a, Direction b)
    {
        if (a == Direction.Up && b == Direction.Down) return true;
        if (a == Direction.Down && b == Direction.Up) return true;
        if (a == Direction.Right && b == Direction.Left) return true;
        if (a == Direction.Left && b == Direction.Right) return true;

        return false;
    }

    private static Direction GetOpositeDirection(Direction a)
    {
        switch (a)
        {
            case Direction.None: return Direction.None;
            case Direction.Up: return Direction.Down;
            case Direction.Down: return Direction.Up;
            case Direction.Left: return Direction.Right;
            case Direction.Right: return Direction.Left;
            default: return Direction.None;
        }
    }
}


public abstract class SnakeInput : MonoBehaviour
{
    public abstract Direction GetInputDirection();
}
