using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFloor : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    private float moveX, moveY, Speed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(moveX, moveY).normalized * Speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Invert")
        {
            moveX *= -1;
            moveY *= -1;
        }
    }

}
