using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //static‰»
    public static SoundManager instance;

    //SE‚Ì”z—ñ
    [SerializeField]
    private AudioSource[] se;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    //‰¹‚ğ–Â‚ç‚·ŠÖ”
    public void PlaySE(int No)
    {
        se[No].Play();
    }
}
