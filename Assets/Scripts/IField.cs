using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public interface IField
{
    public Vector2 MapPosition { get; set; }
    public Action<IField> OnClicked { get; set; }
    public Action<IField> OnSetCurrent { get; set; }
    public Action<IField> OnDisableCurrent { get; set; }

    public Transform transform { get;}
    public bool IsCurrent { get; set; }
    public bool IsAvailable { get; set; }

    private void OnMouseEnter() { }

    private void OnMouseExit() { }

    private void OnMouseDown() { }

    public void Reset() { }
}