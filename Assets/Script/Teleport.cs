using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private Transform _distanation;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(!other.CompareTag("Player")) return;

        other.transform.position = _distanation.position;
    }
}
