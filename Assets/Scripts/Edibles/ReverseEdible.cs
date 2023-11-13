public class ReverseEdible : BaseEdiblePowerUp
{
    public override void EatenEffect(Snake snake)
    {
        snake.Reverse();
    }
}
