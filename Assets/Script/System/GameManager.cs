using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private PlayerMove player;

    private Vector2 mousePos;

    private Vector2 screenToWorldPointPosition;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private GameObject gameOverPanel, stageClearPanel, menuPanel;

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



    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerMove>();
    }

    void Update()
    {
        if (player.buildMode)
        {
            if (Input.GetMouseButtonDown(0))
            {
                mousePos = Input.mousePosition;
                screenToWorldPointPosition = cam.ScreenToWorldPoint(mousePos);

                Ray ray = cam.ScreenPointToRay(mousePos);

                RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

                if (!hit2d)
                {
                    return;
                }

                if (hit2d.collider.gameObject.CompareTag("Material"))
                {
                    hit2d.collider.gameObject.GetComponent<BuildMaterial>().onClick = true;
                    hit2d.collider.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
                }

            }
        }
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
    }

    public void StageClear()
    {
        stageClearPanel.SetActive(true);
    }

    public void OpenMenu()
    {
        menuPanel.SetActive(true);
    }

    public void CloseMenu()
    {
        menuPanel.SetActive(false);
    }

}
