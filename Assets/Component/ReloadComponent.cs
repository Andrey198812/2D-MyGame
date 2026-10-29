using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadComponent : MonoBehaviour
{
    
    private int _starCoin;
    private int _starHp;
    private bool _startIsArmed;
    private bool _isRestarting = false;
    public void Start()
    {
        var session = FindObjectOfType<GameSession>();

        if (session != null)
        {
            _starCoin = session.Data.Coin;
            _starHp = session.Data.Hp;
            _startIsArmed = session.Data.IsArmed;
            Debug.Log($"Старт уровня сохранен: НР = {_starHp}, Coin = {_starCoin}");
          
        }
    }

    public void ReloadLevel()
    {
        Debug.Log($"!!! RELOAD: возвращаю Coin = {_starCoin}, HP = {_starHp}");
        _isRestarting = true;
        var session = FindObjectOfType<GameSession>();

        if (session != null) 
        {
            session.Data.Coin = _starCoin;
            session.Data.Hp = _starHp;
            session.Data.IsArmed = _startIsArmed;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
