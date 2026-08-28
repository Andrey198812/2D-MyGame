using UnityEngine;

public enum CoinType
{
    Gold = 10,
    Silver = 1
}

public class Coin : MonoBehaviour
{
    [SerializeField] private CoinType _coin;
    private Animator _animator;
    private bool _isCollected = false;
    private Collider2D _collider;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isCollected) return;

        if (collision.CompareTag("Player"))
        {
            _isCollected = true;

          
            collision.GetComponent<PlayerScore>()?.CollectCoin((int)_coin);

            
            if (_collider != null) _collider.enabled = false;

            
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0;
                rb.bodyType = RigidbodyType2D.Static; 
            }

           
            if (_animator != null)
            {
                _animator.Play("CoinsDestroy");
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    
    public void DestroyCoin()
    {
        Destroy(gameObject);
    }
}
