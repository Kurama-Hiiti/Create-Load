using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//RequireComponent属性を追加して依存関係を強制する
[RequireComponent(typeof(Rigidbody2D))]
public class MoveFloor : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    private float moveX, moveY, Speed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = new Vector2(moveX, moveY).normalized * Speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Invert"))
        {
            moveX *= -1;
            moveY *= -1;
        }
    }

}
