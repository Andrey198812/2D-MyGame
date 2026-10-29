using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] private float _lifetime = 0.5f;
    [SerializeField] private  bool _destroyOnStart = true;

    private void Start()
    {
        if (_destroyOnStart)
        {
            Destroy(gameObject, _lifetime);
        }
    }

    public void DestroyMe()
    {
        Destroy(gameObject, _lifetime);
    }
}
