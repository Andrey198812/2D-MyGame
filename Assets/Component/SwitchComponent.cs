using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwitchComponent : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private bool _state;
    [SerializeField] private string _animationKey;
    [SerializeField] private UnityEvent<bool> _onSwitched;
    public void Switch()
    {
        Debug.Log($"Switch вызван. Было {_state}");
        Debug.Log($"SwitchComponent.Switch вызван на {gameObject.name}");
        _state = !_state;
        _animator.SetBool(_animationKey, _state);
        _onSwitched?.Invoke(_state);
    }

    [ContextMenu("Switch")]
    public void SwitchIt()
    {
        Switch();
    }
}
