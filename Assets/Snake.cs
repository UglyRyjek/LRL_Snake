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

    public SnakePart Head => _snakeParts.First();

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

        public void SpawnIt()
        {

        }
    }

    [Button]
    public void AddPart()
    {
        Transform newPart = Instantiate(_bodyPartPrototype, _bodyPartPrototype.transform.parent).transform;
        //Debug.Log("Move it where it should be");
        newPart.gameObject.SetActive(true);

        SnakePart sp = new SnakePart(Head.CurrentField, newPart, _boardPresenter);
        _snakeParts.Add(sp);
    }

    private void AddHead()
    {
        BoardField initalField = _boardPresenter.GetPlaceForSnakePart(null);

        SnakePart sp = new SnakePart(initalField, _headPartReference, _boardPresenter);
        _snakeParts.Add(sp);
    }

    [Button]
    public void RemovePart()
    {
        //if (_snakeParts.Count > 1)
        //{
        //    Transform lastPart = _snakeParts[_snakeParts.Count - 1];
        //    _snakeParts.Remove(lastPart);
        //    Destroy(lastPart.gameObject);
        //}
    }

    public void Initialize(BoardPresenter bp, BoardModel bm, SnakeInput input)
    {
        _boardPresenter = bp;
        _input = input;

        //asume that snake starts with head + 1 segment
        AddHead();
        AddPart();
    }

    private BoardField _headDeltaPlace;

    private void SimulateInput(Direction d)
    {
        if (_boardPresenter != null)
        {
            _headDeltaPlace = Head.CurrentField;

            Head.PlaceIt(_boardPresenter.GetNext(d, Head.CurrentField));

            BoardField previousHeadField = _headDeltaPlace;

            for (int i = 1; i < _snakeParts.Count; i++)
            {
                BoardField temp = _snakeParts[i].CurrentField;
                _snakeParts[i].PlaceIt(previousHeadField);
                previousHeadField = temp;
            }
        }
    }

    private Direction _currentDirection = Direction.Up;

    public void Tick()
    {
        Direction d = _input.GetInputDirection();
        if (d != Direction.None)
        {
            //asume that we ignore pressing oposite direcion
            bool opositePressed = AreOpositeDirection(_currentDirection, d);
            if (opositePressed)
            {
                SimulateInput(_currentDirection);
            }
            else
            {
                _currentDirection = d;
                SimulateInput(d);
            }
        }
        else
        {
            SimulateInput(_currentDirection);
        }

        if(ColisionWithSelfOccured())
        {
            Debug.LogError("Bada boom");
        }

    }

    public bool ColisionWithSelfOccured()
    {
        BoardField headField = Head.CurrentField;

        // Check if the head's current field is the same as any other part's field
        for (int i = 1; i < _snakeParts.Count; i++)
        {
            if (_snakeParts[i].CurrentField == headField)
            {
                Debug.LogError($"COLLSION!!! Part{i.ToString()}  {_snakeParts[i].CurrentField.ToString()} and {_snakeParts[i].CurrentField.ToString()}");
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
}


public abstract class SnakeInput : MonoBehaviour
{
    public abstract Direction GetInputDirection();
}
