using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldPresenter : BaseFieldPresenter
{
    [SerializeField] private Text _text;

    public override void Initialize(BoardField bf)
    {
        _text.text = $"{bf.X}x : {bf.Y}y";
    }
}


public abstract class BaseFieldPresenter : MonoBehaviour
{
    public abstract void Initialize(BoardField bf);
}