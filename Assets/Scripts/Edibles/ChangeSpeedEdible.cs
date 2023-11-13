using Sirenix.OdinInspector;
using UnityEngine;

public class ChangeSpeedEdible : BaseEdiblePowerUp
{
    [InfoBox("Lower values are FASTER and vice versa")]
    [SerializeField] private float _snakeSpeedModifier = 0.75f;
    [SerializeField] private float _buffDuration = 5f;

    public override void EatenEffect(Snake snake)
    {
        snake.ChangeSpeedModifier(_snakeSpeedModifier, _buffDuration);
    }
}
