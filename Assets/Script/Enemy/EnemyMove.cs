using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyMove : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private int dir;

    private Rigidbody2D rb;

    //死亡フラグ
    public bool isdeath = false;

    //アイテム
    [SerializeField]
    private GameObject heart;

    //アイテムドロップ確率
    [SerializeField]
    private int dropRato;


    //衝突判定用オブジェクト
    [SerializeField]
    private Transform headCheckPos, rightCheckPos, leftCheckPos;

    //フィールドレイヤー、プレイヤーレイヤー
    [SerializeField]
    private LayerMask groundLayer, playerLayer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isdeath)
        {
            int rato = Random.Range(0, 100);

            if (rato <= dropRato)
            {
                Instantiate(heart, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(dir * moveSpeed, rb.velocity.y);

        if (IsLeftHit())
        {
            dir = 1;
        }

        if (IsRightHit())
        {
            dir = -1;
        }



        //if (IsHeadHit())
        //{
        //    Destroy(gameObject);
        //}

    }


    //private bool IsHeadHit()
    //{
    //    return Physics2D.OverlapBox(headCheckPos.position, new Vector2(0.4f, 0.1f), 0f, playerLayer);
    //}

    //壁用
    private bool IsRightHit()
    {
        return Physics2D.OverlapBox(rightCheckPos.position, new Vector2(0.1f, 0.4f), 0f, groundLayer);
    }

    private bool IsLeftHit()
    {
        return Physics2D.OverlapBox(leftCheckPos.position, new Vector2(0.1f, 0.4f), 0f, groundLayer);
    }

    private bool IsRightPlayerHit()
    {
        return Physics2D.OverlapBox(rightCheckPos.position, new Vector2(0.1f, 0.4f), 0f, playerLayer);
    }

    private bool IsLeftPlayerHit()
    {
        return Physics2D.OverlapBox(leftCheckPos.position, new Vector2(0.1f, 0.4f), 0f, playerLayer);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //Gizmos.DrawWireCube(headCheckPos.position,new Vector3(0.4f, 0.1f,0));

        Gizmos.DrawWireCube(leftCheckPos.position, new Vector3(0.1f, 0.4f, 0));

        Gizmos.DrawWireCube(rightCheckPos.position, new Vector3(0.1f, 0.4f, 0));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (IsRightPlayerHit())
            {
                dir = -1;
            }

            if (IsLeftPlayerHit())
            {
                dir = 1;
            }

        }

        if (collision.gameObject.tag == "Enemy")
        {
            dir *= -1;

        }

        if (collision.gameObject.tag == "Item")
        {
            dir *= -1;

        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DeathZone")
        {
            Destroy(gameObject);
        }
    }

}
