using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelComponent : MonoBehaviour
{
    [SerializeField] private string _nextLevel;

    private void NextLevel()
    {
        if (string.IsNullOrEmpty(_nextLevel))
        {
            Debug.LogError("Имя следующей сцены не указано в инспекторе!");
            return;
        }
       SceneManager.LoadScene(_nextLevel);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            NextLevel();
        }
    }
}
