using UnityEngine;
using UnityEngine.Events;



public class EnterTrigger : MonoBehaviour
{
    [SerializeField] private string _tag;
    [SerializeField] private LayerMask _layer = ~0;
    [SerializeField] private UnityEvent _action;

    public void OnTriggerEnter2D(Collider2D collision)
    {

        if ((_layer.value & (1 << collision.gameObject.layer)) == 0) return;


        if (!string.IsNullOrEmpty(_tag) && !collision.gameObject.CompareTag(_tag)) return;

        transform.position = collision.ClosestPoint(transform.position);
        _action?.Invoke();
    }
}