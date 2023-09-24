using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Defines;
using UnityEngine.Rendering;

public class CursorCtrl : MonoBehaviour
{
    Texture2D cursor_Def;
    Texture2D cursor_Move;
    Texture2D cursor_Attack;
    CursorType _type = CursorType.UnKnown;
    int lMask;

    void Start()
    {
        Init();
    }

    void Update()
    {
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            return;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit rhit;
        if(Physics.Raycast(ray, out rhit, 100.0f, lMask))
        {
            if(rhit.collider.gameObject.layer == (int)LayerType.Floor)
            {
                if(_type != CursorType.Hand)
                {
                    Cursor.SetCursor(cursor_Move, new Vector2(cursor_Move.width / 3, 0), CursorMode.Auto);
                    _type = CursorType.Hand;
                }
            }
            else
            {
                if (_type != CursorType.Attack)
                {
                    Cursor.SetCursor(cursor_Attack, new Vector2(cursor_Attack.width / 5, 0), CursorMode.Auto);
                    _type = CursorType.Attack;
                }
            }
        }
        else
        {
            if (_type != CursorType.Default)
            {
                Cursor.SetCursor(cursor_Def, new Vector2(cursor_Def.width / 3, 0), CursorMode.Auto);
                _type = CursorType.Default;
            }
        }
    }

    void Init()
    {
        cursor_Def = Managers._scene.LoadCursor(CursorType.Default);
        cursor_Move = Managers._scene.LoadCursor(CursorType.Hand);
        cursor_Attack = Managers._scene.LoadCursor(CursorType.Attack);
        lMask = (1 << (int)LayerType.Floor) | (1 << (int)LayerType.Monster);
        
    }
}
