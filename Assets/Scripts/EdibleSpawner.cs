using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class EdibleSpawner : MonoBehaviour
{
    private BoardPresenter _boardPresenter;
    private BoardService _board;
    private float _timer;

    [SerializeField, Range(0.2f, 10f)]
    private float _edibleSpawningRange = 0.5f;

    public void Initialized(BoardService board, BoardPresenter boardPresenter)
    {
        _boardPresenter = boardPresenter;
        _board = board;
    }

    [SerializeField]
    private List<SpawnerWreper> _availableEdibles = new List<SpawnerWreper>();

    [System.Serializable]
    private class SpawnerWreper
    {
        [SerializeField]
        private BaseEdiblePowerUp _prototype;

        [SerializeField]
        private float _weight = 1;

        public float Weight => _weight;
        public BaseEdiblePowerUp Prototype => _prototype;
    }

    private static SpawnerWreper WagedRandomSpawner(IReadOnlyList<SpawnerWreper> availableEdibles)
    {
        float totalWeight = availableEdibles.Sum(edible => edible.Weight);
        float randomValue = UnityEngine.Random.Range(0, totalWeight);
        float weightSum = 0;
        foreach (SpawnerWreper edible in availableEdibles)
        {
            weightSum += edible.Weight;
            if (randomValue <= weightSum)
            {
                return edible;
            }
        }

        return availableEdibles.LastOrDefault();
    }
    
    private List<ActiveEdible> _ediblesDictionary = new List<ActiveEdible>();

    private class ActiveEdible
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
        if (b != null)
        {
            return b.mono;
        }

        return null;
    }

    private void SpawnEdible(BoardField bf, BoardPresenter bp)
    {
        SpawnerWreper sw = WagedRandomSpawner(_availableEdibles);

        BaseEdiblePowerUp newEdible = Instantiate(sw.Prototype, sw.Prototype.transform.parent);

        newEdible.gameObject.SetActive(true);

        newEdible.PlaceOnBoard(bf, bp);

        ActiveEdible a = new ActiveEdible(newEdible, bf);
        _ediblesDictionary.Add(a);
    }

    public void EatEdible(BaseEdiblePowerUp powerUp)
    {
        ActiveEdible a = _ediblesDictionary.FirstOrDefault(x => x.mono == powerUp);
        if (a != null)
        {
            a.mono.RemoveFromBoard();
            _ediblesDictionary.Remove(a);
        }
    }

    public void SolveEdibleEating(Snake snake, Action<BaseEdiblePowerUp> OnEaten)
    {
        BaseEdiblePowerUp v = BEPU(snake.SnakeParts.First());
        if (v != null)
        {
            v.EatenEffect(snake);
            EatEdible(v);

            OnEaten.Invoke(v);
        }
    }

    public void SolveEdibleSpawning(Snake snake)
    {
        _timer += Time.deltaTime;
        if(_timer > _edibleSpawningRange)
        {
            SpawnSingleRandom(snake);
            _timer = 0f;
        }
    }

    public void SpawnSingleRandom(Snake snake)
    {
        BoardField bf = _board.GetFreeFieldOnBoard(snake.SnakeParts, _ediblesDictionary.Select(x => x.boardField).ToList());

        SpawnEdible(bf, _boardPresenter);
    }
}
