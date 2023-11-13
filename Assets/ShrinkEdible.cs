using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkEdible : BaseEdiblePowerUp
{
    public override void EatenEffect(Snake snake)
    {
        snake.RemovePart();
    }
}


