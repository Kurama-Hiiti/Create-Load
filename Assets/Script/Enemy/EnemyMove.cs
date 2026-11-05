using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

//RequireComponent属性を追加して依存関係を強制する
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMove : MonoBehaviour
{
    //移動速度
    [SerializeField]
    private float moveSpeed;

    //移動方向
    [SerializeField]
    private int dir;

    private Rigidbody2D rb;

    //死亡フラグ
    public bool isdeath = false;

    //ドロップアイテム
    [SerializeField]
    private GameObject heart;

    //アイテムドロップ確率
    [SerializeField]
    private int dropRate;


    //衝突判定用オブジェクト
    [SerializeField]
    private Transform headCheckPos, rightCheckPos, leftCheckPos;

    //フィールドレイヤー、プレイヤーレイヤー
    [SerializeField]
    private LayerMask groundLayer, playerLayer;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        //死亡処理
        if (isdeath)
        {
            int rate = Random.Range(0, 100);

            //アイテムドロップ処理
            if (rate <= dropRate)
            {
                Instantiate(heart, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        //移動処理
        rb.velocity = new Vector2(dir * moveSpeed, rb.velocity.y);

        //壁と接触したときに反転
        if (IsLeftHit())
        {
            dir = 1;
        }

        if (IsRightHit())
        {
            dir = -1;
        }


    }


    //右側が壁に接触判定
    private bool IsRightHit()
    {
        return Physics2D.OverlapBox(rightCheckPos.position, new Vector2(0.1f, 0.4f), 0f, groundLayer);
    }

    //左側が壁に接触判定
    private bool IsLeftHit()
    {
        return Physics2D.OverlapBox(leftCheckPos.position, new Vector2(0.1f, 0.4f), 0f, groundLayer);
    }

    //右側がプレイヤーに接触判定
    private bool IsRightPlayerHit()
    {
        return Physics2D.OverlapBox(rightCheckPos.position, new Vector2(0.1f, 0.4f), 0f, playerLayer);
    }

    //左側がプレイヤーに接触判定
    private bool IsLeftPlayerHit()
    {
        return Physics2D.OverlapBox(leftCheckPos.position, new Vector2(0.1f, 0.4f), 0f, playerLayer);
    }

    //シーンビューでの当たり判定確認用
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(leftCheckPos.position, new Vector3(0.1f, 0.4f, 0));

        Gizmos.DrawWireCube(rightCheckPos.position, new Vector3(0.1f, 0.4f, 0));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //プレイヤーとの接触で反転
            if (IsRightPlayerHit())
            {
                dir = -1;
            }

            if (IsLeftPlayerHit())
            {
                dir = 1;
            }

        }

        //敵同士の接触で反転
        if (collision.gameObject.CompareTag("Enemy"))
        {
            dir *= -1;

        }

        //アイテムの接触で反転
        if (collision.gameObject.CompareTag("Item"))
        {
            dir *= -1;

        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //落下したときに削除
        if (collision.gameObject.CompareTag("DeathZone"))
        {
            Destroy(gameObject);
        }
    }

}
