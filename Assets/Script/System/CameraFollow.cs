using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //プレイヤーの位置情報格納
    private Transform playerPos;

    //プレイヤーとの距離
    [SerializeField]
    private float offSetX;

    //カメラが移動するべき位置
    private Vector3 tempPos;


    private void Awake()
    {
        //プレイヤーの位置取得
        playerPos = GameObject.FindWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        //追従関数
        FollowPlayer();
    }


    //プレイヤーを追跡する関数
    private void FollowPlayer()
    {
        if (playerPos != null)
        {
            //カメラの初期位置
            tempPos = transform.position;

            //プレイヤーがカメラの位置の前（右側）に行った時、カメラが遅れて追従する
            if (playerPos.position.x > transform.position.x + 2)
            {
                tempPos.x = playerPos.position.x - 2;

            }

            //プレイヤーがカメラの位置の後ろ（左側）に行った時、カメラが遅れて追従する
            if (playerPos.position.x < transform.position.x - offSetX)
            {
                tempPos.x = playerPos.position.x + offSetX;

            }

            //プレイヤーがカメラの位置の上に行った時、カメラが追従する
            if (playerPos.position.y > transform.position.y)
            {
                tempPos.y = playerPos.position.y;
            }

            //プレイヤーがカメラの位置の下に行った時、カメラが追従する
            if (playerPos.position.y < transform.position.y)
            {
                tempPos.y = playerPos.position.y;
            }

            //カメラのｙ軸方向がマイナスになる時はｙ軸の値を0にする
            if (tempPos.y < 0)
            {
                tempPos.y = 0;
            }


            //位置更新
            transform.position = tempPos;

        }
    }
}
