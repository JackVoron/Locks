using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HackField : MonoBehaviour, IField
{
    [SerializeField] private Material _availableMaterial;
    [SerializeField] private Material _interactMaterial;
    [SerializeField] private Material _currentMaterial;

    private Renderer _renderer;
    private Material _defaultMaterial, _previousMaterial;
    private bool _isCurrent, _isAvailable;
    private HackStatuses _hackStatus = HackStatuses.Locked;

    public HackStatuses HackStatus { 
        get => _hackStatus;
        set {
            _hackStatus = value;
            if (value == HackStatuses.Succesful) 
            { 
                OnHacked?.Invoke(this);
                SetChildrenColor(Color.green);
            }
            else if (value == HackStatuses.Failed)
            {
                SetChildrenColor(Color.red);
            }
        }
    }

    public bool IsCurrent { 
        get => _isCurrent;
        set 
        {
            _isCurrent = value;
            ChangeMaterial();
            if (value)
            {
                OnSetCurrent?.Invoke(this);
            }
            else
            {
                OnDisableCurrent?.Invoke(this);
            }
        }
    } 
    public bool IsAvailable { 
        get => _isAvailable;
        set 
        {
            _isAvailable = value;
            ChangeMaterial();
        }
    }

    public Vector2 MapPosition { get; set; }
    public Action<IField> OnClicked { get; set; }
    public Action<IField> OnHacked { get; set; }
    public Action<IField> OnSetCurrent { get; set; }
    public Action<IField> OnDisableCurrent { get; set; }

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _defaultMaterial = _renderer.material;
        OnSetCurrent += (field) => field.IsAvailable = false;
        OnHacked += (IField field) =>
        {
            foreach (Renderer renderer in this.GetComponentsInChildren<Renderer>())
            {
                renderer.material = _availableMaterial;
            }
        };
    }

    private void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        _previousMaterial = _renderer.material;
        if (!IsCurrent)
            _renderer.material = _interactMaterial;
    }

    private void OnMouseExit()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (!IsCurrent)
            _renderer.material = _previousMaterial;
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (IsAvailable)
            OnClicked?.Invoke(this);
    }

    public void ChangeMaterial()
    {
        if(_isCurrent)
        {
            _renderer.material = _currentMaterial;
        }
        else if (_isAvailable)
        {
            _renderer.material = _availableMaterial;
        }
        else
        {
            _renderer.material = _defaultMaterial;
        }
    }

    public void SetChildrenColor(Color color)
    {
        foreach(Renderer renderer in this.GetComponentsInChildren<Renderer>())
        {
            renderer.material.color = color;
        }
    }

    public void Reset()
    {
        IsAvailable = false;
        IsCurrent = false;
    }
}
