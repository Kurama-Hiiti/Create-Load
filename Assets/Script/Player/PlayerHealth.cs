using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    //最大体力、現在の体力
    [SerializeField]
    public int maxHealth = 5;
    public int health;

    private void Start()
    {
        health = maxHealth;
    }


    //体力があるかの判定関数
    public bool IsAllive()
    {
        return health > 0 ? true : false;
    }


    //死亡時のオブジェクトの削除関数
    public void Destroy()
    {
        Destroy(gameObject);
    }

    //HPを減らす関数
    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            Destroy();
            SoundManager.instance.PlaySE(8);
        }

    }

    //アイテムで体力回復

    public void Heal(int healAmount)
    {
        if (health < maxHealth)
        {
            health += healAmount;
        }
        
    }

}
