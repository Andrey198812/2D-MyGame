using UnityEngine;

public class LayerCheck : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;

   
    public bool IsTouchingLayer { get; private set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
      
        if ((_layerMask & (1 << collision.gameObject.layer)) != 0)
        {
            IsTouchingLayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((_layerMask & (1 << collision.gameObject.layer)) != 0)
        {
            IsTouchingLayer = false;
        }
    }

  
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = IsTouchingLayer ? Color.green : Color.red;
      
        var collider = GetComponent<CircleCollider2D>();
        if (collider != null)
        {
            Gizmos.DrawWireSphere(transform.position, collider.radius);
        }
    }
}