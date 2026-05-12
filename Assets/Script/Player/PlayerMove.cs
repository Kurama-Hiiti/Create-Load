using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed, dashSpeed, jumpPower;

    private Animator animator;

    private Rigidbody2D rb;

    //ジャンプ判定用オブジェクト
    [SerializeField]
    private Transform groundCheckPos, rightCheckPos, leftCheckPos, headCheckPos;

    //地面レイヤー
    [SerializeField]
    private LayerMask groundLayer;

    //台座レイヤー
    [SerializeField]
    private LayerMask pedestalLayer;

    //敵レイヤー
    [SerializeField]
    private LayerMask enemyLayer;

    //はしごフラグ
    private bool laddar = false;

    //建築フラグ
    public bool buildMode = false;

    //カメラ
    [SerializeField]
    private GameObject mainCamera, subCamera;

    private bool enemyHit = false;

    //ノックバック
    [SerializeField]
    private float knockBackX, knockBackY;

    //ゴールフラグ
    private bool goal;

    //メニュー表示
    private bool menu;

    //体力関係
    private PlayerHealth health;
    private PlayerHealthUI healthUI;

    //ダメージを受けた時間
    private float takeDamageTime;

    //経過時間
    private float elapsedTime;

    [SerializeField]
    private GameObject backGround;

    [SerializeField]
    private bool moveFloor = false;

    private bool door = false;

    private Vector3 teleportPos;

    //チェックポイント
    public int checkPoint;

    //スポーンポイント
    [SerializeField]
    private Transform startPos, checkPointPos;

    private void Awake()
    {

        health = GetComponent<PlayerHealth>();

        healthUI = GetComponent<PlayerHealthUI>();

        checkPoint = PlayerPrefs.GetInt("Check", 0);
    }


    // Start is called before the first frame update
    void Start()
    {
        if (checkPoint == 1)
        {
            transform.position = checkPointPos.position;
        }
        else
        {
            transform.position = startPos.position;
        }
        
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        goal = false;
        menu = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (!goal)
        {
            if (buildMode)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    SoundManager.instance.PlaySE(6);
                    buildMode = false;
                    subCamera.SetActive(false);
                    mainCamera.GetComponent<Camera>().enabled = true;
                    backGround.SetActive(true);

                    //subCamera.GetComponent<Camera>().enabled = false;
                    //mainCamera.GetComponent<Camera>().enabled = true;
                }
                return;
            }

            PlayerJump();
            BuildMode();

            if (!menu && !buildMode && Input.GetKeyDown(KeyCode.Escape))
            {
                menu = true;
                SoundManager.instance.PlaySE(0);
                Time.timeScale = 0;
                GameManager.instance.OpenMenu();
            }

            if (menu && Input.GetKeyDown(KeyCode.E))
            {
                menu = false;
                SoundManager.instance.PlaySE(6);
                Time.timeScale = 1f;
                GameManager.instance.CloseMenu();
            }

            if (Input.GetKeyDown(KeyCode.W) && door)
            {
                SoundManager.instance.PlaySE(9);
                transform.position = teleportPos - new Vector3(0,0.4f,0);
            }


        }

        
        if (goal)
        {
            PlayerPrefs.SetInt("Check", 0);
            GameManager.instance.StageClear();
        }


    }

    private void FixedUpdate()
    {
        if (!goal)
        {
            if (buildMode)
            {
                return;
            }

            if (enemyHit)
            {
                elapsedTime = Time.time; 

                if (elapsedTime - takeDamageTime > 0.8f)
                {
                    enemyHit = false;
                }

                return;
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {

                if (Input.GetAxisRaw("Horizontal") > 0 && !IsRightHit())
                {
                    //キー入力を受け取り移動させる
                    rb.velocity = new Vector2(Input.GetAxis("Horizontal") * dashSpeed, rb.velocity.y);
                    animator.SetTrigger("Right");

                }
                else if (Input.GetAxisRaw("Horizontal") < 0 && !IsLeftHit())
                {
                    //キー入力を受け取り移動させる
                    rb.velocity = new Vector2(Input.GetAxis("Horizontal") * dashSpeed, rb.velocity.y);
                    animator.SetTrigger("Left");
                }
                else if (IsGrounded() || IsPedestal())
                {
                    if (!enemyHit)
                    {
                        if (moveFloor)
                        {
                            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
                            return;
                        }

                        rb.velocity = new Vector2(0f, rb.velocity.y);

                    }
                }
                else
                {
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
                }

            }
            else
            {
                if (Input.GetAxisRaw("Horizontal") > 0 && !IsRightHit())
                {
                    //キー入力を受け取り移動させる
                    rb.velocity = new Vector2(Input.GetAxis("Horizontal") * playerSpeed, rb.velocity.y);
                    animator.SetTrigger("Right");
                }
                else if (Input.GetAxisRaw("Horizontal") < 0 && !IsLeftHit())
                {
                    //キー入力を受け取り移動させる
                    rb.velocity = new Vector2(Input.GetAxis("Horizontal") * playerSpeed, rb.velocity.y);
                    animator.SetTrigger("Left");
                }
                else if (IsGrounded() || IsPedestal())
                {
                    if (!enemyHit)
                    {
                        if (moveFloor)
                        {
                            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
                            return;
                        }

                        rb.velocity = new Vector2(0f, rb.velocity.y);

                    }

                }
                else
                {
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
                }

            }




            if (Input.GetAxisRaw("Vertical") > 0 && laddar)
            {
                //キー入力を受け取り移動させる
                rb.velocity = new Vector2(rb.velocity.x, Input.GetAxisRaw("Vertical") * playerSpeed);
                animator.SetTrigger("Up");
            }

            if (Input.GetAxisRaw("Vertical") < 0 && laddar)
            {
                //キー入力を受け取り移動させる
                rb.velocity = new Vector2(rb.velocity.x, Input.GetAxisRaw("Vertical") * playerSpeed);
                animator.SetTrigger("Up");
            }

            


        }


    }


    private void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            SoundManager.instance.PlaySE(1);
            rb.AddForce(new Vector2(0f, jumpPower), ForceMode2D.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.Space) && IsPedestal())
        {
            SoundManager.instance.PlaySE(1);
            rb.AddForce(new Vector2(0f, jumpPower), ForceMode2D.Impulse);
        }

    }


    //地面に接地しているか（ジャンプ可能か）判定関数
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheckPos.position, 0.2f, groundLayer);
    }

    //台座に接地しているか（ジャンプ可能か）判定関数
    private bool IsPedestal()
    {
        return Physics2D.OverlapCircle(groundCheckPos.position, 0.3f, pedestalLayer);
    }


    private bool IsRightHit()
    {
        return Physics2D.OverlapBox(rightCheckPos.position, new Vector2(0.1f, 0.6f), 0f, groundLayer);
    }

    private bool IsLeftHit()
    {
        return Physics2D.OverlapBox(leftCheckPos.position, new Vector2(0.1f, 0.6f), 0f, groundLayer);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DeathZone")
        {
            SoundManager.instance.PlaySE(8);

            Destroy(gameObject);
            GameManager.instance.GameOver();
        }

        if (collision.gameObject.tag == "Goal")
        {
            if (!goal)
            {
                rb.velocity = new Vector2(0f, 0f);
            }
            goal = true;
            SoundManager.instance.PlaySE(7);

        }

        if (collision.gameObject.tag == "CheckPoint" && checkPoint == 0)
        {
            PlayerPrefs.SetInt("Check", 1);
            checkPoint = PlayerPrefs.GetInt("Check");
            SoundManager.instance.PlaySE(0);
        }

    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Laddar")
        {
            laddar = true;
            rb.velocity = new Vector2(0, 0);
            rb.gravityScale = 0;
        }

        if (collision.gameObject.tag == "Door")
        {
            door = true;
            teleportPos = collision.GetComponent<Door>().movePos.position;
        }


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Laddar")
        {
            laddar = false;
            rb.velocity = new Vector2(0, 0);
            rb.gravityScale = 1f;
        }

        if (collision.gameObject.tag == "Door")
        {
            door = false;
        }


    }

    private void BuildMode()
    {
        if (IsPedestal() && Input.GetKeyDown(KeyCode.B))
        {
            SoundManager.instance.PlaySE(5);
            buildMode = true;
            subCamera.SetActive(true); 
            mainCamera.GetComponent<Camera>().enabled = false;
            backGround.SetActive(false);
            //subCamera.GetComponent<Camera>().enabled = true;
            //mainCamera.GetComponent<Camera>().enabled = false;
        }
    }

    private bool IsEnmeyHit()
    {
        return Physics2D.OverlapCircle(groundCheckPos.position, 0.2f, enemyLayer);
    }

    //壁用
    private bool IsRightEnemyHit()
    {
        return Physics2D.OverlapBox(rightCheckPos.position, new Vector2(0.1f, 0.6f), 0f, enemyLayer);
    }

    private bool IsLeftEnemyHit()
    {
        return Physics2D.OverlapBox(leftCheckPos.position, new Vector2(0.1f, 0.6f), 0f, enemyLayer);
    }

    private bool IsHeadEnemyHit()
    {
        return Physics2D.OverlapBox(headCheckPos.position, new Vector2(0.6f, 0.1f), 0f, enemyLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        //Gizmos.DrawWireCube(groundCheckPos.position, new Vector3(0.4f, 0.1f, 0));

        Gizmos.DrawWireCube(leftCheckPos.position, new Vector3(0.1f, 0.6f, 0));

        Gizmos.DrawWireCube(rightCheckPos.position, new Vector3(0.1f, 0.6f, 0));

        Gizmos.DrawWireCube(headCheckPos.position, new Vector3(0.6f, 0.1f, 0));

        Gizmos.DrawWireSphere(groundCheckPos.position, 0.2f);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (IsEnmeyHit())
            {
                SoundManager.instance.PlaySE(2);
                rb.AddForce(new Vector2(0f, jumpPower), ForceMode2D.Impulse);

                collision.gameObject.GetComponent<EnemyMove>().isdeath = true;

            }

            if (IsRightEnemyHit() && !enemyHit)
            {
                SoundManager.instance.PlaySE(4);
                enemyHit = true;
                takeDamageTime = Time.time;

                health.TakeDamage(1);
                healthUI.SubtractHealth(1);

                rb.velocity = new Vector2(0f, 0f);
                rb.AddForce(new Vector2(-knockBackX, knockBackY), ForceMode2D.Impulse);
            }

            if (IsLeftEnemyHit() && !enemyHit)
            {
                SoundManager.instance.PlaySE(4);
                enemyHit = true;
                takeDamageTime = Time.time;

                health.TakeDamage(1);
                healthUI.SubtractHealth(1);

                rb.velocity = new Vector2(0f, 0f);
                rb.AddForce(new Vector2(knockBackX, knockBackY), ForceMode2D.Impulse);
            }

            if (IsHeadEnemyHit() && !enemyHit)
            {
                SoundManager.instance.PlaySE(4);
                enemyHit = true;
                takeDamageTime = Time.time;

                health.TakeDamage(1);
                healthUI.SubtractHealth(1);

                rb.velocity = new Vector2(0f, 0f);
                rb.AddForce(new Vector2(0, -knockBackY), ForceMode2D.Impulse);
            }

        }

        //アイテムで体力回復
        if (collision.gameObject.tag == "Item")
        {
            health.Heal(1);
            healthUI.HealHealth(1);
            SoundManager.instance.PlaySE(3);

            Destroy(collision.gameObject);
        }


        if (collision.gameObject.tag == "MoveFloor")
        {
            moveFloor = true;
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "MoveFloor")
        {
            moveFloor = false;
        }
    }

}
