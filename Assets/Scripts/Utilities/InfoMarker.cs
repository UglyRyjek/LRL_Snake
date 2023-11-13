using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class InfoMarker : MonoBehaviour
{
    [SerializeField]
    [TextArea]
    private string _info;

    [SerializeField]
    private GameObject _reference;

}
