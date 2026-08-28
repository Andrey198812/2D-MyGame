using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
   private int _totalScore = 0;
    private int _totalCoins    = 0;

    public void CollectCoin(int value)
    {
        _totalScore += value;
        _totalCoins++;
        Debug.Log($" Собрано монет: {_totalCoins}. Всего очков: {_totalScore} ");
    }
}
