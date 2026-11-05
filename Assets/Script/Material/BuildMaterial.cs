using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;


//RequireComponent属性を追加して依存関係を強制する
[RequireComponent(typeof(Rigidbody2D))]
public class BuildMaterial : MonoBehaviour
{
    private PlayerMove player;

    private Rigidbody2D rb;

    private Vector2 mousePos;

    private Vector2 screenToWorldPointPosition;

    public bool onClick = false;

    [SerializeField]
    private Camera cam;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMove>();
        rb = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (player.buildMode)
        {
            if (Input.GetMouseButton(0) && onClick)
            {
                mousePos = Input.mousePosition;
                screenToWorldPointPosition = cam.ScreenToWorldPoint(mousePos);

                //マウスの位置にオブジェクトを移動させる
                transform.position = screenToWorldPointPosition;

                
                //オブジェクト操作中に回転処理（A、D）
                if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    transform.Rotate(0, 0, 0.1f);
                }

                if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    transform.Rotate(0, 0, -0.1f);
                }

            }
            else
            {
                onClick = false;
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //上下左右の範囲操作範囲外へオブジェクトが触れた際に位置を操作範囲内に戻し選択を解除する

        if (collision.gameObject.CompareTag("BuildFieldLeft"))
        {
            onClick = false;

            Vector3 myPos = transform.position;

            Vector3 collisonPos = collision.transform.position;

            Vector3 offSet = new (1.5f, 0, 0);

            myPos.x = collisonPos.x + offSet.x;

            transform.position = myPos;

        }

        if (collision.gameObject.CompareTag("BuildFieldRight"))
        {
            onClick = false;

            Vector3 myPos = transform.position;

            Vector3 collisonPos = collision.transform.position;

            Vector3 offSet = new (1.5f, 0, 0);

            myPos.x = collisonPos.x - offSet.x;

            transform.position = myPos;

        }

        if (collision.gameObject.CompareTag("BuildFieldTop"))
        {
            onClick = false;

            Vector3 myPos = transform.position;

            Vector3 collisonPos = collision.transform.position;

            Vector3 offSet = new (0, 0.5f, 0);

            myPos.y = collisonPos.y - offSet.y;

            transform.position = myPos;

        }

        if (collision.gameObject.CompareTag("BuildFieldBottom"))
        {
            onClick = false;

            Vector3 myPos = transform.position;

            Vector3 collisonPos = collision.transform.position;

            Vector3 offSet = new (0, 0.5f, 0);

            myPos.y = collisonPos.y + offSet.y;

            transform.position = myPos;

        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //上下左右の範囲操作範囲外へオブジェクトが触れた際に位置を操作範囲内に戻し選択を解除する

        if (collision.gameObject.CompareTag("BuildFieldLeft"))
        {
            onClick = false;

            Vector3 myPos = transform.position;

            Vector3 collisonPos = collision.transform.position;

            Vector3 offSet = new (1.5f, 0, 0);

            myPos.x = collisonPos.x + offSet.x;

            transform.position = myPos;

        }

        if (collision.gameObject.CompareTag("BuildFieldRight"))
        {
            onClick = false;

            Vector3 myPos = transform.position;

            Vector3 collisonPos = collision.transform.position;

            Vector3 offSet = new (1.5f, 0, 0);

            myPos.x = collisonPos.x - offSet.x;

            transform.position = myPos;

        }

        if (collision.gameObject.CompareTag("BuildFieldTop"))
        {
            onClick = false;

            Vector3 myPos = transform.position;

            Vector3 collisonPos = collision.transform.position;

            Vector3 offSet = new (0, 0.5f, 0);

            myPos.y = collisonPos.y - offSet.y;

            transform.position = myPos;

        }

        if (collision.gameObject.CompareTag("BuildFieldBottom"))
        {
            onClick = false;

            Vector3 myPos = transform.position;

            Vector3 collisonPos = collision.transform.position;

            Vector3 offSet = new (0, 0.5f, 0);

            myPos.y = collisonPos.y + offSet.y;

            transform.position = myPos;

        }

    }

}