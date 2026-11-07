using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//RequireComponent属性を追加して依存関係を強制する
[RequireComponent(typeof(PlayerHealth))]
public class PlayerHealthUI : MonoBehaviour
{
    //体力UIの表示管理


    //ハートUIを格納する配列
    [SerializeField]
    private GameObject[] healthHearts;

    //体力スクリプト格納
    private PlayerHealth playerHealth;


    private void Awake()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void OnEnable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged += UpdateHealthUI;
        }
    }

    private void OnDisable()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= UpdateHealthUI;
        }
    }


    private void Start()
    {

        UpdateHealthUI(playerHealth.Health,healthHearts.Length);

    }


    //UI更新処理
    private void UpdateHealthUI(int currentHealth, int maxHealth)
    {

        for (int i = 0; i < healthHearts.Length; i++)
        {
            healthHearts[i].SetActive(i < currentHealth);
        }
    }


}
