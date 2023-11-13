public class ReverseEdible : BaseEdible
{
    public override void EatenEffect(SnakeController snake)
    {
        snake.Reverse();
    }
}
