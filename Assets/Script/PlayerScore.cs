using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    private int _totalScore = 0;
    private int _totalCoins    = 0;

    public int TotalCoins => _totalCoins;
    public int TotalScore => _totalScore;

    public void CollectCoin(int value)
    {
        _totalScore += value;
        _totalCoins++;
        Debug.Log($" Собрано монет: {_totalCoins}. Всего очков: {_totalScore} ");
    }
    public void RemoveCoins(int amount)
    {
        _totalCoins -= amount;
        if (_totalCoins < 0) _totalCoins = 0;
    }
}
