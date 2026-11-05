using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniHeart : MonoBehaviour
{
    void Start()
    {
        Invoke(nameof(DestroyHeart), 10f);
    }

    private void DestroyHeart()
    {
        Destroy(gameObject);
    }

}
