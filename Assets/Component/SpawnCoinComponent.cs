using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCoinComponent : MonoBehaviour
{
    [SerializeField] private Coin[] _coin;
    [SerializeField] private float _chans = 0.5f;
    [SerializeField] private int _coinsMin;
    [SerializeField] private int _coinsMax;

    [ContextMenu("Тест Spaawn Coin")]
    public void SpawnCoin()
    {

        var value = Random.Range(0f, 1f);

        if (value > _chans)
        {

            return;
        }

        var session = FindObjectOfType<GameSession>();
        if (session == null)
        {

            return;
        }

        var coinsToDrop = Random.Range(_coinsMin, _coinsMax + 1);
        coinsToDrop = Mathf.Min(coinsToDrop, session.Data.Coin);

        if (coinsToDrop <= 0)
        {

            return;
        }

        session.Data.Coin -= coinsToDrop;

        for (int i = 0; i < coinsToDrop; i++)
        {

            var randonIndex = Random.Range(0, _coin.Length);
            var coin = _coin[randonIndex];


            var transformCoin = transform.position;

            transformCoin.x += Random.Range(-0.5f, 0.5f);

            transformCoin.y += Random.Range(0.1f, 0.6f);


            Coin newCoin = Instantiate(coin, transformCoin, Quaternion.identity);
            Rigidbody2D rb = newCoin.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                float randomX = Random.Range(-3f, 3f);
                float randomY = Random.Range(4f, 7f);
                Vector2 force = new Vector2(randomX, randomY);
                rb.AddForce(force, ForceMode2D.Impulse);
            }

        }
    }
}

