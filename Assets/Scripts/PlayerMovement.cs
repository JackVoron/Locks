using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 MapPosition { get; set; }
    public Action OnPlayerMove;

    public void Move(IField field)
    {
        MapPosition = field.MapPosition;
        transform.position =  new Vector3(MapPosition.x, 1, MapPosition.y);
        OnPlayerMove?.Invoke();
    }
}
