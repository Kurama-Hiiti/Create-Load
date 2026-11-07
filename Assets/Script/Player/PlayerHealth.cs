using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerHealth : MonoBehaviour
{

    //最大体力、現在の体力
    private readonly int maxHealth = 5;
    public int Health { get; private set; }



    // 体力が変化したときのイベント
    public event Action<int, int> OnHealthChanged;

    // 死亡したときのイベント
    public event Action OnDied;

    //死亡フラグ
    private bool isDead;

    private void Awake()
    {
        Health = maxHealth;
    }

    //HPを減らす関数
    public void TakeDamage(int damageAmount)
    {
        if (isDead || damageAmount <= 0) return;

        Health = Mathf.Max(0,Health - damageAmount);

        // 体力変更を通知
        OnHealthChanged?.Invoke(Health, maxHealth);


        if (Health <= 0)
        {
            isDead = true;

            OnDied?.Invoke();

            Destroy(gameObject);
        }

    }

    //アイテムで体力回復

    public void Heal(int healAmount)
    {
        if (isDead || healAmount <= 0) return;

        Health = Mathf.Min(maxHealth, Health + healAmount);

        // 体力変更を通知
        OnHealthChanged?.Invoke(Health, maxHealth);

    }

}
