using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniHeart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyHeart", 10f);
    }

    private void DestroyHeart()
    {
        Destroy(gameObject);
    }

}
