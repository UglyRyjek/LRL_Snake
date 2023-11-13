using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SimpleScoringUI : MonoBehaviour
{
    [SerializeField] private Text _text;

    private int _bufferScore = -1;

    private void Update()
    {
        int score = GameSessionService.I.GetSessionData.EatenEdibles;

        if(_bufferScore != score)
        {
            _text.text = $"Eaten : {score}";
        }

        _bufferScore = score;
    }
}
