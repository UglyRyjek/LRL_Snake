using UnityEngine;

[System.Serializable]
public struct BoardParameters
{
    public BoardParameters(int width, int height)
    {
        _width = width;
        _height = height;
    }

    public int Width => _width;
    public int Height => _height;

    [Range(5, 10)]
    [SerializeField] private int _height;
    [Range(5, 18)]

    [SerializeField] private int _width;
}
