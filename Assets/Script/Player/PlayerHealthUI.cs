using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthUI : MonoBehaviour
{
    //体力UIの表示管理


    //ハートUIを格納する配列
    [SerializeField]
    private GameObject[] healthHearts;

    //現在表示しているハートの数,現在の体力
    public int currentHealthIndex, health;

    //体力スクリプト格納
    private PlayerHealth playerHealth;


    private void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();

        health = playerHealth.health;

        currentHealthIndex = health - 1;

    }


    //ダメージでハートの表示数を減らす関数
    public void SubtractHealth(int damageAmount)
    {
        for (int i = 0; i < damageAmount; i++)
        {
            healthHearts[currentHealthIndex].SetActive(false);

            currentHealthIndex--;
            health--;

            if (health <= 0)
            {
                GameManager.instance.GameOver();

                break;
            }

        }
    }

    //アイテムで回復する関数

    public void HealHealth(int healAmount)
    {
        for (int i = 0; i < healAmount; i++)
        {
            if (health < playerHealth.maxHealth)
            {
                currentHealthIndex++;
                health++;

                healthHearts[currentHealthIndex].SetActive(true);
            }

        }
    }

}
