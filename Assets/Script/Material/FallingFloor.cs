using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//RequireComponent属性を追加して依存関係を強制する
[RequireComponent(typeof(Rigidbody2D))]
public class FallingFloor : MonoBehaviour
{
    //初期位置（足場復活位置）
    private Vector3 initialPos;

    //オブジェクトの基準位置（床を振動させる際の基準位置）
    private Vector3 objPos;

    private Rigidbody2D rb;

    //プレイヤーレイヤー
    [SerializeField]
    private LayerMask playerLayer;

    [SerializeField]
    private Transform playerCheckPos;

    [SerializeField]
    private float fallSpeed;


    //実際に揺らす数値格納
    private float shakingOffset_X, shakingOffset_Y;

    //揺れる強さ
    [SerializeField]
    private float shakeAmount;

    [SerializeField]
    private float shakeTime;

    private bool isRide = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        initialPos = transform.position;
    }



    void Update()
    {
        if (IsPlayer() && !isRide)
        {
            isRide = true;
            ShakeObj(shakeTime);
        }


    }

    //プレイヤーが乗ったのか判定
    private bool IsPlayer()
    {
        return Physics2D.OverlapBox(playerCheckPos.position, new Vector2(0.9f, 0.1f), 0f, playerLayer);
    }

    //シーンビュー確認用
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(playerCheckPos.position, new Vector3(0.9f, 0.1f, 0));

    }

    //落ちる関数
    private void Fall()
    {
        rb.velocity = new Vector2(0,fallSpeed);
    }

    //オブジェクトを揺らし止める関数
    public void ShakeObj(float shaketime)
    {
        //0.01秒間隔で揺らす
        InvokeRepeating(nameof(Shake), 0f, 0.01f);

        //止める
        Invoke(nameof(StopShaking), shaketime);

    }


    //乗った時に揺れる関数
    private void Shake()
    {
        if (shakeAmount > 0)
        {
            objPos = transform.position;

            shakingOffset_X = Random.value * shakeAmount * 2 - shakeAmount;
            shakingOffset_Y = Random.value * shakeAmount * 2 - shakeAmount;

            objPos.x += shakingOffset_X;
            objPos.y += shakingOffset_Y;

            transform.position = objPos;
        }
    }


    //揺れを止める関数
    void StopShaking()
    {
        CancelInvoke(nameof(Shake));

        //振動後初期位置に戻す
        transform.position = initialPos;

        //落下
        Fall();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //落下しきったら初期位置へ移動
        if (collision.gameObject.CompareTag("DeathZone"))
        {
            this.gameObject.SetActive(false);
            isRide = false;
            this.transform.position = initialPos;

            this.gameObject.SetActive(true);

        }
    }



}
