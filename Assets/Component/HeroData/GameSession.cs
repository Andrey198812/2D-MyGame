using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameSession : MonoBehaviour
{
    [SerializeField] private PlayerData _data;
    public PlayerData Data => _data;

    private void Awake()
    {
       
        var sessions = FindObjectsOfType<GameSession>();

       
        if (sessions.Length > 1)
        {
            foreach (var session in sessions)
            {
                
                if (session != this)
                {
                   
                    this._data = session._data;

                   
                    Destroy(session.gameObject);

                    
                    break;
                }
            }
        }

       
        DontDestroyOnLoad(this);
    }
}

