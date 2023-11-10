using UnityEngine;
// could have used new unity input system
public class KeyboardSnakeInput : SnakeInput
{
    [SerializeField] private KeyCode _upButton;
    [SerializeField] private KeyCode _downButton;
    [SerializeField] private KeyCode _rightButton;
    [SerializeField] private KeyCode _leftButton;


    // ASUME that we cancel inputs if we have more than 1 pressed
    public override Direction GetInputDirection()
    {
        Direction finalDirection = Direction.None;
        int pressedCount = 0;

        if (Input.GetKey(_upButton))
        {
            pressedCount++;
            finalDirection = Direction.Up;
        }

        if(Input.GetKey(_downButton))
        {
            pressedCount++;
            finalDirection = Direction.Down;
        }

        if (Input.GetKey(_rightButton))
        {
            pressedCount++;
            finalDirection = Direction.Right;
        }

        if (Input.GetKey(_leftButton))
        {
            pressedCount++;
            finalDirection = Direction.Left;
        }

        if(pressedCount > 1)
        {
            return Direction.None;
        }

        return finalDirection;

    }
}
