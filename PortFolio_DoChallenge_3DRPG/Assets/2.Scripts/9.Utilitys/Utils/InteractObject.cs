using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;

[AddComponentMenu("Custom/InteractObject")]
[System.Serializable]
public class InteractObject : MonoBehaviour
{
    [SerializeField] InteractType type = InteractType.Unknown;

    public InteractType Type { get { return type; } }
}

