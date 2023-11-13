using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EdibleSpawner : MonoBehaviour
{
    [SerializeField]
    private List<SpawnerWreper> _availableEdibles = new List<SpawnerWreper>();

    [System.Serializable]
    private class SpawnerWreper
    {
        [SerializeField]
        private BaseEdiblePowerUp _prototype;

        [SerializeField]
        private float _weight = 1;

        public BaseEdiblePowerUp Prototype => _prototype;
    }

    private SpawnerWreper WagedRandomSpawner()
    {
        return _availableEdibles[Random.Range(0, _availableEdibles.Count)];
    }


    private List<ActiveEdible> _ediblesDictionary = new List<ActiveEdible>();
    

    public class ActiveEdible
    {
        public ActiveEdible(BaseEdiblePowerUp m, BoardField b)
        {
            mono = m;
            boardField = b;
        }

        public BaseEdiblePowerUp mono;
        public BoardField boardField;

    }

    public BaseEdiblePowerUp BEPU(BoardField snakeHead)
    {
        ActiveEdible b = _ediblesDictionary.FirstOrDefault(x => x.boardField == snakeHead);
        if(b != null)
        {
            return b.mono;
        }

        return null;
    }

    
    public void SpawnEdible(BoardField bf, BoardPresenter bp)
    {
        SpawnerWreper sw = WagedRandomSpawner();

        BaseEdiblePowerUp newEdible = Instantiate(sw.Prototype, sw.Prototype.transform.parent);

        newEdible.gameObject.SetActive(true);

        newEdible.PlaceOnBoard(bf, bp);

        ActiveEdible a = new ActiveEdible(newEdible, bf);
        _ediblesDictionary.Add(a);
    }

    public void EatEdible(BaseEdiblePowerUp powerUp)
    {
        ActiveEdible a = _ediblesDictionary.FirstOrDefault(x => x.mono == powerUp);
        if(a != null)
        {
            a.mono.RemoveFromBoard();
            _ediblesDictionary.Remove(a);
        }
    }
}

public abstract class BaseEdiblePowerUp : MonoBehaviour
{
    [SerializeField] private Transform _gfx;
    private BoardField _currentField;

    public abstract void EatenEffect(Snake snake);

    public void PlaceOnBoard(BoardField bf, BoardPresenter bp)
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




