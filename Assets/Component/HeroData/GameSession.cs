using UnityEngine;

public class GameSession : MonoBehaviour
{
    [SerializeField] private PlayerData _data;
    public PlayerData Data => _data;

    private int _startHp;
    private int _startCoins;
    private bool _isArmedStart;
    private int _startSwordAmount;

    private void Awake()
    {

        var sessions = FindObjectsOfType<GameSession>();
        if (sessions.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        _startHp = _data.Hp;
        _startCoins = _data.Coin;
        _isArmedStart = _data.IsArmed;
        _startSwordAmount = _data.SwordAmount;
    }

    public void ResetToStart()
    {
        _data.Hp = _startHp;
        _data.Coin = _startCoins;
        _data.IsArmed = _isArmedStart;
        _data.SwordAmount = _startSwordAmount;
    }
}