using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSizeEdible : BaseEdiblePowerUp
{
    [SerializeField] private int _changeBySize = 1;

    public override void EatenEffect(Snake snake)
    {
        snake.ChangeSnakeSize(_changeBySize);
    }
}
