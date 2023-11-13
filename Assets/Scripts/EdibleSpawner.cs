using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EdibleSpawner : MonoBehaviour
{
    [SerializeField]
    private List<SpawnerWreper> _availableEdibles = new List<SpawnerWreper>();

    private List<ActiveEdible> _ediblesDictionary = new List<ActiveEdible>();

    private SpawningProfile _spawningProfile;
    private IBoard _board;
    private float _timer;

    public void Initialized(IBoard board, SpawningProfile spawningProfile)
    {
        _spawningProfile = spawningProfile;
        _board = board;
    }

    public BaseEdible GetEdibleToBeEaten(BoardField snakeHead)
    {
        ActiveEdible b = _ediblesDictionary.FirstOrDefault(x => x.boardField == snakeHead);
        if (b != null)
        {
            return b.mono;
        }

        return null;
    }

    public void EatEdible(BaseEdible powerUp)
    {
        ActiveEdible a = _ediblesDictionary.FirstOrDefault(x => x.mono == powerUp);
        if (a != null)
        {
            a.mono.RemoveFromBoard();
            _ediblesDictionary.Remove(a);
        }
    }

    public void SolveEdibleEating(SnakeController snake, Action<BaseEdible> OnEaten)
    {
        BaseEdible v = GetEdibleToBeEaten(snake.SnakeParts.First());
        if (v != null)
        {
            v.EatenEffect(snake);
            EatEdible(v);

            OnEaten.Invoke(v);
        }
    }

    public void SolveEdibleSpawning(SnakeController snake)
    {
        _timer += Time.deltaTime;
        if (_timer > _spawningProfile.spawningRate)
        {
            SpawnSingleRandom(snake);
            _timer = 0f;
        }
    }

    public void SpawnSingleRandom(SnakeController snake)
    {
        BoardField bf = _board.GetFreeFieldOnBoard(snake.SnakeParts, _ediblesDictionary.Select(x => x.boardField).ToList());

        SpawnEdible(bf, _board);
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
   
    private void SpawnEdible(BoardField bf, IBoard bp)
    {
        SpawnerWreper sw = WagedRandomSpawner(_availableEdibles);

        BaseEdible newEdible = Instantiate(sw.Prototype, sw.Prototype.transform.parent);

        newEdible.gameObject.SetActive(true);

        newEdible.PlaceOnBoard(bf, bp);

        ActiveEdible a = new ActiveEdible(newEdible, bf);
        _ediblesDictionary.Add(a);
    }

    [System.Serializable]
    private class SpawnerWreper
    {
        [SerializeField]
        private BaseEdible _prototype;
        public BaseEdible Prototype => _prototype;


        [SerializeField]
        private float _weight = 1;
        public float Weight => _weight;
    }

    private class ActiveEdible
    {
        public BaseEdible mono;
        public BoardField boardField;

        public ActiveEdible(BaseEdible m, BoardField b)
        {
            mono = m;
            boardField = b;
        }

    }

}

