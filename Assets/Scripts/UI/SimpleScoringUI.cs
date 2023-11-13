using UnityEngine;
using UnityEngine.UI;
using SSnake.GameSession;

public class SimpleScoringUI : MonoBehaviour
{
    [SerializeField] private Text _text;

    private int _bufferScore = -1;

    private void Update()
    {
        int score = GameSessionService.I.SessionData.EatenEdibles;

        if(_bufferScore != score)
        {
            _text.text = $"Eaten : {score}";
        }

        _bufferScore = score;
    }
}
