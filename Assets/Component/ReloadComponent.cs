using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadComponent : MonoBehaviour
{
   
    private static int _checkPointHp = 0;
    private static int _checkPointCoins = 0;
    private static bool _checkPointArmed = false;
    private static bool _isCheckPointSaved = false;
    private void Awake()
    {
        Debug.Log("<color=orange>ReloadComponent: Awake сработал!</color>");
    }
    private void Start()
    {
        var session = FindObjectOfType<GameSession>();

        if (session != null)
        {
        
            if (!_isCheckPointSaved)
            {
                _checkPointHp = session.Data.Hp;
                _checkPointCoins = session.Data.Coin;
                _checkPointArmed = session.Data.IsArmed;
                _isCheckPointSaved = true;
                
            }
            else
            {
             
                session.Data.Hp = _checkPointHp;
                session.Data.Coin = _checkPointCoins;
                session.Data.IsArmed = _checkPointArmed;
            }
        }
      
    }

    public void ReloadLevel()
    {
        Debug.Log("<color=red>ReloadLevel ВЫЗВАН!</color>");

       
        Invoke(nameof(LoadSceneDelayed), 0.1f);
    }

    private void LoadSceneDelayed()
    {
       
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
       
    }
}
