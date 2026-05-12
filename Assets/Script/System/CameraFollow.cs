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
        FindPlayer();
    }

    private void LateUpdate()
    {
        FollowPlayer();
    }




    //プレイヤーを見つける関数
    private void FindPlayer()
    {
        playerPos = GameObject.FindWithTag("Player").transform;
    }

    //プレイヤーを追跡する関数
    private void FollowPlayer()
    {
        if (playerPos != null)
        {
            tempPos = transform.position;

            if (playerPos.position.x > transform.position.x +2)
            {
                tempPos.x = playerPos.position.x - 2;

            }

            if (playerPos.position.x < transform.position.x - offSetX)
            {
                tempPos.x = playerPos.position.x + offSetX;

            }

            if (playerPos.position.y > transform.position.y)
            {
                tempPos.y = playerPos.position.y;
            }

            if (playerPos.position.y < transform.position.y)
            {
                tempPos.y = playerPos.position.y;
            }

            if (tempPos.y < 0)
            {
                tempPos.y = 0;
            }


            transform.position = tempPos;

        }
    }
}
