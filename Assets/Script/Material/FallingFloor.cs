using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingFloor : MonoBehaviour
{
    private Vector3 initialPos;

    private Vector3 objPos;

    private Rigidbody2D rb;

    //ÉvÉåÉCÉÑÅ[ÉåÉCÉÑÅ[
    [SerializeField]
    private LayerMask playerLayer;

    [SerializeField]
    private Transform playerCheckPos;

    [SerializeField]
    private float fallSpeed;


    //é¿ç€Ç…óhÇÁÇ∑êîíläiî[
    private float shakingOffset_X, shakingOffset_Y;

    //óhÇÍÇÈã≠Ç≥
    [SerializeField]
    private float shakeAmount;

    [SerializeField]
    private float shakeTime;

    private bool isRide = false;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        initialPos = transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlayer() && !isRide)
        {
            isRide = true;
            ShakeObj(shakeTime);
        }


    }


    private bool IsPlayer()
    {
        return Physics2D.OverlapBox(playerCheckPos.position, new Vector2(0.9f, 0.1f), 0f, playerLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(playerCheckPos.position, new Vector3(0.9f, 0.1f, 0));

    }

    //óéÇøÇÈä÷êî
    private void Fall()
    {
        rb.velocity = new Vector2(0,fallSpeed);
    }

    //ÉIÉuÉWÉFÉNÉgÇóhÇÁÇµé~ÇﬂÇÈä÷êî
    public void ShakeObj(float shaketime)
    {
        //0.01ïbä‘äuÇ≈óhÇÁÇ∑
        InvokeRepeating("Shake", 0f, 0.01f);

        //é~ÇﬂÇÈ
        Invoke("StopShaking", shaketime);

    }


    //èÊÇ¡ÇΩéûÇ…óhÇÍÇÈä÷êî
    private void Shake()
    {
        if (shakeAmount > 0)
        {
            objPos = transform.position;

            shakingOffset_X = Random.value * shakeAmount * 2 - shakeAmount;
            shakingOffset_Y = Random.value * shakeAmount * 2 - shakeAmount;

            objPos.x += shakingOffset_X;
            objPos.y += shakingOffset_Y;

            transform.position = objPos;
        }
    }


    //óhÇÍÇé~ÇﬂÇÈä÷êî
    void StopShaking()
    {
        CancelInvoke("Shake");

        transform.position = initialPos;

        Fall();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DeathZone")
        {
            this.gameObject.SetActive(false);
            isRide = false;
            this.transform.position = initialPos;

            this.gameObject.SetActive(true);

        }
    }



}
