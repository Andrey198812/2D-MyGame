using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuckedSwordDirection : MonoBehaviour
{

    void Start()
    {
        var hero = FindObjectOfType<Hero>();
        if (hero != null)
        {
            var sr = hero.GetComponent<SpriteRenderer>();

            if (sr.flipX)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

}
