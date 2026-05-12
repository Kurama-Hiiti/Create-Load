using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class BuildMaterial : MonoBehaviour
{
    private PlayerMove player;

    private Rigidbody2D rb;

    private Vector2 mousePos;

    private Vector2 screenToWorldPointPosition;

    public bool onClick = false;

    [SerializeField]
    private Camera cam;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMove>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player.buildMode)
        {
            if (Input.GetMouseButton(0) && onClick)
            {
                mousePos = Input.mousePosition;
                screenToWorldPointPosition = cam.ScreenToWorldPoint(mousePos);

                transform.position = screenToWorldPointPosition;

                

                if (Input.GetAxisRaw("Horizontal") < 0)
                {
                    transform.Rotate(0, 0, 1);
                }

                if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    transform.Rotate(0, 0, -1);
                }

            }
            else
            {
                onClick = false;
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BuildFieldLeft")
        {
            onClick = false;

            Vector3 myPos = transform.position;

            Vector3 collisonPos = collision.transform.position;

            Vector3 offSet = new Vector3(1.5f, 0, 0);

            myPos.x = collisonPos.x + offSet.x;

            transform.position = myPos;

        }

        if (collision.gameObject.tag == "BuildFieldRight")
        {
            onClick = false;

            Vector3 myPos = transform.position;

            Vector3 collisonPos = collision.transform.position;

            Vector3 offSet = new Vector3(1.5f, 0, 0);

            myPos.x = collisonPos.x - offSet.x;

            transform.position = myPos;

        }

        if (collision.gameObject.tag == "BuildFieldTop")
        {
            onClick = false;

            Vector3 myPos = transform.position;

            Vector3 collisonPos = collision.transform.position;

            Vector3 offSet = new Vector3(0, 0.5f, 0);

            myPos.y = collisonPos.y - offSet.y;

            transform.position = myPos;

        }

        if (collision.gameObject.tag == "BuildFieldBottom")
        {
            onClick = false;

            Vector3 myPos = transform.position;

            Vector3 collisonPos = collision.transform.position;

            Vector3 offSet = new Vector3(0, 0.5f, 0);

            myPos.y = collisonPos.y + offSet.y;

            transform.position = myPos;

        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BuildFieldLeft")
        {
            onClick = false;

            Vector3 myPos = transform.position;

            Vector3 collisonPos = collision.transform.position;

            Vector3 offSet = new Vector3(1.5f, 0, 0);

            myPos.x = collisonPos.x + offSet.x;

            transform.position = myPos;

        }

        if (collision.gameObject.tag == "BuildFieldRight")
        {
            onClick = false;

            Vector3 myPos = transform.position;

            Vector3 collisonPos = collision.transform.position;

            Vector3 offSet = new Vector3(1.5f, 0, 0);

            myPos.x = collisonPos.x - offSet.x;

            transform.position = myPos;

        }

        if (collision.gameObject.tag == "BuildFieldTop")
        {
            onClick = false;

            Vector3 myPos = transform.position;

            Vector3 collisonPos = collision.transform.position;

            Vector3 offSet = new Vector3(0, 0.5f, 0);

            myPos.y = collisonPos.y - offSet.y;

            transform.position = myPos;

        }

        if (collision.gameObject.tag == "BuildFieldBottom")
        {
            onClick = false;

            Vector3 myPos = transform.position;

            Vector3 collisonPos = collision.transform.position;

            Vector3 offSet = new Vector3(0, 0.5f, 0);

            myPos.y = collisonPos.y + offSet.y;

            transform.position = myPos;

        }

    }

}