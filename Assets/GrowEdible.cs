public class GrowEdible : BaseEdiblePowerUp
{
    public override void EatenEffect(Snake snake)
    {
        snake.AddPart();
    }
}




