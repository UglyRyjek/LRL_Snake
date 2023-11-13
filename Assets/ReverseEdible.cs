using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseEdible : BaseEdiblePowerUp
{
    public override void EatenEffect(Snake snake)
    {
        snake.Reverse();
    }
}
