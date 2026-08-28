using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOBJ : MonoBehaviour
{
    [SerializeField] private GameObject _obj;
    public void OnTriggerEnter2D (Collider2D _obj)
    {
        if(_obj.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
