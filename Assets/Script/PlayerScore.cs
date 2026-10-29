using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    private int _totalScore = 0;

    public int TotalCoins
    {
        get
        {
            
            var session = FindObjectOfType<GameSession>();
            if (session != null) return session.Data.Coin;
            return 0;
        }
    }
    public int TotalScore => _totalScore;
    public void CollectCoin(int value)
    {
        _totalScore += value;
        var session = FindObjectOfType<GameSession>();

        if (session != null)
        {
            session.Data.Coin += 1;
            Debug.Log($" Собрано монет: {session.Data.Coin}. Всего очков: {_totalScore} ");
        }
    }
}