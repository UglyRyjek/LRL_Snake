using UnityEngine;

[System.Serializable]
public struct BoardParameters
{
    [SerializeField, Range(5, 10)]
    private int _height;
    [SerializeField, Range(5, 18)]
    private int _width;

    public BoardParameters(int width, int height)
    {
        _width = width;
        _height = height;
    }

    public int Width => _width;
    public int Height => _height;
}
