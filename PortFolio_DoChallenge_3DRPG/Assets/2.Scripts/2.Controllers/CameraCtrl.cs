using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;

public class CameraCtrl : MonoBehaviour
{
    [SerializeField] CameraMode _mode;
    [SerializeField] Vector3 _offSet;
    [SerializeField] GameObject _player;

    void Start()
    {
        
    }

    void LateUpdate()
    {
        if(_mode == CameraMode.QuaterView)
        {
            transform.position = _player.transform.position + _offSet;
            transform.LookAt(_player.transform);
        }
    }

    public void SetQuaterView(Vector3 offset)
    {
        _mode = CameraMode.QuaterView;
        _offSet = offset;
    }
}
