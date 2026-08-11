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

    public void OnHorizontalMovement(InputAction.CallbackContext context)
    {
        
        if (_hero == null) return;

        Vector2 direction = context.ReadValue<Vector2>();
        _hero.SetDurection(direction);
    }
}