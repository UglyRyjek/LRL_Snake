using UnityEngine;

public class ChangeSizeEdible : BaseEdible
{
    [SerializeField] private int _changeBySize = 1;

    public override void EatenEffect(SnakeController snake)
    {
        snake.ChangeSnakeSize(_changeBySize);
    }
}
