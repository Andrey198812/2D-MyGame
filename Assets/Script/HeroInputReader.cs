using UnityEngine.InputSystem;
using UnityEngine;

public class HeroInputReader : MonoBehaviour
{
    [SerializeField] private Hero _hero;

    private void Awake()
    {
        if (_hero == null)
        {
            _hero = FindObjectOfType<Hero>();
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (_hero == null) return;

        Vector2 direction = context.ReadValue<Vector2>();
        _hero.SetDirection(direction);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (_hero == null) return;

        if (context.performed) 
        {
            _hero.JumpPressed(); 
        }
        else if (context.canceled) 
        {
            _hero.JumpReleased();  
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _hero.Interact();
        }
    }
}