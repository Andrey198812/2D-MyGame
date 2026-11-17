using UnityEngine;

using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    private GameSession _session;
    private int _totalScore = 0;

    private void Awake()
    {
        _session = FindObjectOfType<GameSession>();
    }

    public int TotalCoins => _session != null ? _session.Data.Coin : 0;
    public int TotalScore => _totalScore;

    public void CollectCoin(int value)
    {
        _totalScore += value;
        if (_session != null)
        {
            _session.Data.Coin += 1;
            Debug.Log($"Собрано монет: {_session.Data.Coin}");
        }
    }

    public void RemoveCoins(int amount)
    {
        if (_session != null)
        {
            _session.Data.Coin -= amount;
            if (_session.Data.Coin < 0) _session.Data.Coin = 0;
        }
    }
}