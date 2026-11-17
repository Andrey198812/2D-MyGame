using System;
using UnityEngine;

public class SpawnListComponent : MonoBehaviour
{
    [SerializeField] private SpawnData[] _spawners;

    public void Spawn(string id)
    {
        Debug.Log($"<color=yellow>2. Пульт получил команду: '{id}'</color>"); // <--- ДОБАВЬ ЭТО

        foreach (var data in _spawners)
        {
            if (data.Id == id)
            {
                Debug.Log($"<color=green>3. Пульт нашел '{id}', вызываю SpawnComponent</color>"); // <--- И ЭТО
                data.Component.Spawn();
                break;
            }
        }
    }

    [Serializable]
    public class SpawnData
    {
        public string Id;
        public SpawnComponent Component;
    }
}