using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spwan : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy;

    private bool spawn = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !spawn)
        {
            Instantiate(enemy, transform.position, Quaternion.identity);
            spawn = true;
        }
    }
}
