using UnityEngine;

public abstract class BaseEdible : MonoBehaviour
{
    [SerializeField] private Transform _gfx;
    private BoardField _currentField;

    public abstract void EatenEffect(SnakeController snake);

    public void PlaceOnBoard(BoardField bf, IBoard bp)
    {
        _currentField = bf;

        _gfx.transform.position = bp.GetFieldPresenter(_currentField).transform.position;
    }

    public void RemoveFromBoard()
    {
        Destroy(_gfx.gameObject);
        Destroy(gameObject);
    }
}


