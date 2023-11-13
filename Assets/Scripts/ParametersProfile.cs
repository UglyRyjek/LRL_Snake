using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ParametersProfile : MonoBehaviour
{
    public BoardParametersProfile boardProfile;
    public SnakeMovementProfile snakeProfile;
    public SpawningProfile spawningProfile;
}

[System.Serializable]
public struct BoardParametersProfile
{
    [SerializeField]
    public BoardParameters boardSize;
}

[System.Serializable]
public class SnakeMovementProfile
{
    [InfoBox("This represents snake speed coded as time intervals " +
        "\n smaller value means faster movement ")]
    [SerializeField, Range(0.05f, 0.40f)]
    public float baseSnakeSpeed;
}

[System.Serializable]
public class SpawningProfile 
{
    [InfoBox("This represents time intervals between edible objects spawning")]
    [SerializeField, Range(1f, 10f)]
    public float spawningRate;

}

