using UnityEngine;

public class SwordStuck : MonoBehaviour
{
    [SerializeField] private GameObject _stuckedSwordPrefab;
    [SerializeField] private LayerMask _wallLayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (((1 << collision.gameObject.layer) & _wallLayer) != 0)
        {

            if (_stuckedSwordPrefab != null)
            {
                Vector2 hitPoint = collision.ClosestPoint(transform.position);
                Instantiate(_stuckedSwordPrefab, hitPoint, Quaternion.identity);
            }


            Destroy(gameObject);
        }
    }
}