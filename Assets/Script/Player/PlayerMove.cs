using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;


//RequireComponent属性を追加して依存関係を強制する
[RequireComponent(typeof(PlayerHealth)), RequireComponent(typeof(PlayerHealthUI)), RequireComponent(typeof(Animator)), RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    //移動速度、ダッシュ速度、ジャンプ力
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

    //ゴールチェック
    private bool goalCheck;

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

    //文字列をハッシュ（int）に変換して事前にキャッシュしておく
    private static readonly int rightTriggerHash = Animator.StringToHash("Right");
    private static readonly int leftTriggerHash = Animator.StringToHash("Left");
    private static readonly int upTriggerHash = Animator.StringToHash("Up");


    private void Awake()
    {

        health = GetComponent<PlayerHealth>();

        healthUI = GetComponent<PlayerHealthUI>();

        checkPoint = PlayerPrefs.GetInt("Check", 0);
    }



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
        goalCheck = false;

    }


    void Update()
    {
        if (!goal)
        {
            //ビルドモード解除
            if (buildMode)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    SoundManager.instance.PlaySE(6);
                    buildMode = false;
                    subCamera.SetActive(false);
                    mainCamera.GetComponent<Camera>().enabled = true;
                    backGround.SetActive(true);

                }
                return;
            }

            PlayerJump();
            BuildMode();

            //メニュー画面表示
            if (!menu && !buildMode && Input.GetKeyDown(KeyCode.Escape))
            {
                menu = true;
                SoundManager.instance.PlaySE(0);
                Time.timeScale = 0;
                GameManager.instance.OpenMenu();
            }

            //メニュー画面非表示
            if (menu && Input.GetKeyDown(KeyCode.E))
            {
                menu = false;
                SoundManager.instance.PlaySE(6);
                Time.timeScale = 1f;
                GameManager.instance.CloseMenu();
            }

            //ドアでの移動処理
            if (Input.GetKeyDown(KeyCode.W) && door)
            {
                SoundManager.instance.PlaySE(9);
                transform.position = teleportPos - new Vector3(0,0.4f,0);
            }


        }

        //ゴール処理
        if (goal)
        {
            if (goalCheck) return;

            goalCheck = true;
            //チェックポイントリセット
            PlayerPrefs.SetInt("Check", 0);
            //クリアUI表示
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

            //敵に当たった場合一定時間移動無効
            if (enemyHit)
            {
                elapsedTime = Time.time; 

                if (elapsedTime - takeDamageTime > 0.8f)
                {
                    enemyHit = false;
                }

                return;
            }

            //ダッシュ時
            if (Input.GetKey(KeyCode.LeftShift))
            {
                //右への入力
                if (Input.GetAxisRaw("Horizontal") > 0 && !IsRightHit())
                {
                    //キー入力を受け取り移動させる
                    rb.velocity = new Vector2(Input.GetAxis("Horizontal") * dashSpeed, rb.velocity.y);
                    animator.SetTrigger(rightTriggerHash);

                }
                else if (Input.GetAxisRaw("Horizontal") < 0 && !IsLeftHit())//左への入力
                {
                    //キー入力を受け取り移動させる
                    rb.velocity = new Vector2(Input.GetAxis("Horizontal") * dashSpeed, rb.velocity.y);
                    animator.SetTrigger(leftTriggerHash);
                }
                else if (IsGrounded() || IsPedestal())
                {
                    if (!enemyHit)
                    {
                        //移動床での処理
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
                    animator.SetTrigger(rightTriggerHash);
                }
                else if (Input.GetAxisRaw("Horizontal") < 0 && !IsLeftHit())
                {
                    //キー入力を受け取り移動させる
                    rb.velocity = new Vector2(Input.GetAxis("Horizontal") * playerSpeed, rb.velocity.y);
                    animator.SetTrigger(leftTriggerHash);
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




            //梯子を上る処理
            if (Input.GetAxisRaw("Vertical") > 0 && laddar)
            {
                //キー入力を受け取り移動させる
                rb.velocity = new Vector2(rb.velocity.x, Input.GetAxisRaw("Vertical") * playerSpeed);
                animator.SetTrigger(upTriggerHash);
            }

            //梯子を下る処理
            if (Input.GetAxisRaw("Vertical") < 0 && laddar)
            {
                //キー入力を受け取り移動させる
                rb.velocity = new Vector2(rb.velocity.x, Input.GetAxisRaw("Vertical") * playerSpeed);
                animator.SetTrigger(upTriggerHash);
            }

            


        }


    }

    //ジャンプ関数
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

    //プレイヤーの右側が壁に接地しているか
    private bool IsRightHit()
    {
        return Physics2D.OverlapBox(rightCheckPos.position, new Vector2(0.1f, 0.6f), 0f, groundLayer);
    }

    //プレイヤーの左側が壁に接地しているか
    private bool IsLeftHit()
    {
        return Physics2D.OverlapBox(leftCheckPos.position, new Vector2(0.1f, 0.6f), 0f, groundLayer);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //落下処理
        if (collision.gameObject.CompareTag("DeathZone"))
        {

            health.Die();
        }

        //ゴール処理
        if (collision.gameObject.CompareTag("Goal"))
        {
            if (!goal)
            {
                rb.velocity = new Vector2(0f, 0f);
            }
            goal = true;
            SoundManager.instance.PlaySE(7);

        }

        //チェックポイント処理（中間地点）
        if (collision.gameObject.CompareTag("CheckPoint") && checkPoint == 0)
        {
            PlayerPrefs.SetInt("Check", 1);
            checkPoint = PlayerPrefs.GetInt("Check");
            SoundManager.instance.PlaySE(0);
        }

    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        //梯子との接触処理
        if (collision.gameObject.CompareTag("Laddar"))
        {
            laddar = true;
            rb.velocity = new Vector2(0, 0);
            rb.gravityScale = 0;
        }

        //ドアとの接触処理
        if (collision.gameObject.CompareTag("Door"))
        {
            door = true;
            //ドアの移動先取得
            teleportPos = collision.GetComponent<Door>().movePos.position;
        }


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //梯子からの離れる処理
        if (collision.gameObject.CompareTag("Laddar"))
        {
            laddar = false;
            rb.velocity = new Vector2(0, 0);
            rb.gravityScale = 1f;
        }

        //ドアから離れる処理
        if (collision.gameObject.CompareTag("Door"))
        {
            door = false;
        }


    }

    //ビルドモード移行関数
    private void BuildMode()
    {
        if (IsPedestal() && Input.GetKeyDown(KeyCode.B))
        {
            SoundManager.instance.PlaySE(5);
            buildMode = true;
            subCamera.SetActive(true); 
            mainCamera.GetComponent<Camera>().enabled = false;
            backGround.SetActive(false);

        }
    }

    //敵を踏みつけた時の判定
    private bool IsEnmeyHit()
    {
        return Physics2D.OverlapCircle(groundCheckPos.position, 0.2f, enemyLayer);
    }

    //プレイヤーの右側が敵との接触判定
    private bool IsRightEnemyHit()
    {
        return Physics2D.OverlapBox(rightCheckPos.position, new Vector2(0.1f, 0.6f), 0f, enemyLayer);
    }

    //プレイヤーの左側が敵との接触判定
    private bool IsLeftEnemyHit()
    {
        return Physics2D.OverlapBox(leftCheckPos.position, new Vector2(0.1f, 0.6f), 0f, enemyLayer);
    }

    //プレイヤーの上側が敵との接触判定
    private bool IsHeadEnemyHit()
    {
        return Physics2D.OverlapBox(headCheckPos.position, new Vector2(0.6f, 0.1f), 0f, enemyLayer);
    }

    //シーンビューでの判定確認用処理
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(leftCheckPos.position, new Vector3(0.1f, 0.6f, 0));

        Gizmos.DrawWireCube(rightCheckPos.position, new Vector3(0.1f, 0.6f, 0));

        Gizmos.DrawWireCube(headCheckPos.position, new Vector3(0.6f, 0.1f, 0));

        Gizmos.DrawWireSphere(groundCheckPos.position, 0.2f);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //敵を踏んだ時
            if (IsEnmeyHit())
            {
                //SE
                SoundManager.instance.PlaySE(2);
                //踏んだ反動でのジャンプ
                rb.AddForce(new Vector2(0f, jumpPower), ForceMode2D.Impulse);

                //接触した敵の死亡フラグ更新
                collision.gameObject.GetComponent<EnemyMove>().isdeath = true;

            }

            //敵と接触したとき（右側）
            if (IsRightEnemyHit() && !enemyHit)
            {
                //SE
                SoundManager.instance.PlaySE(4);
                //敵との接触フラグ更新
                enemyHit = true;
                //ダメージヒット時間設定
                takeDamageTime = Time.time;

                //体力更新
                health.TakeDamage(1);

                //プレイヤーの速度を0にしたのちにノックバック処理
                rb.velocity = new Vector2(0f, 0f);
                rb.AddForce(new Vector2(-knockBackX, knockBackY), ForceMode2D.Impulse);
            }

            //敵と接触したとき（左側）
            if (IsLeftEnemyHit() && !enemyHit)
            {
                SoundManager.instance.PlaySE(4);
                enemyHit = true;
                takeDamageTime = Time.time;

                health.TakeDamage(1);

                rb.velocity = new Vector2(0f, 0f);
                rb.AddForce(new Vector2(knockBackX, knockBackY), ForceMode2D.Impulse);
            }

            //敵と接触したとき（上側）
            if (IsHeadEnemyHit() && !enemyHit)
            {
                SoundManager.instance.PlaySE(4);
                enemyHit = true;
                takeDamageTime = Time.time;

                health.TakeDamage(1);

                rb.velocity = new Vector2(0f, 0f);
                rb.AddForce(new Vector2(0, -knockBackY), ForceMode2D.Impulse);
            }

        }

        //アイテムで体力回復
        if (collision.gameObject.CompareTag("Item"))
        {
            health.Heal(1);
            SoundManager.instance.PlaySE(3);

            Destroy(collision.gameObject);
        }

        //移動床接触判定
        if (collision.gameObject.CompareTag("MoveFloor"))
        {
            moveFloor = true;
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //移動床離脱判定
        if (collision.gameObject.CompareTag("MoveFloor"))
        {
            moveFloor = false;
        }
    }

}
