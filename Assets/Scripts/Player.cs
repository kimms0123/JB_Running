using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Settings")]
    public float JupmForce;

    [Header("Reference")]
    public Rigidbody2D PlayerRigid;
    
    private bool isGrounded = true;
    
    public Animator PlayerAnimator;

    public BoxCollider2D PlayerCollider;

    // 더블 점프를 위한 점프 횟수 카운트(0 = 바닥, 1 = 공중 점프, 2 = 더블 점프)
    private int jumpCount = 0;

    private int lives = 3;
    private bool isInvincible = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2)
        {
            // y속도를 초기화해서 더블점프가 일정하게 작동하도록
            PlayerRigid.linearVelocity = new Vector2(PlayerRigid.linearVelocity.x, 0);

            PlayerRigid.AddForce(Vector2.up * JupmForce, ForceMode2D.Impulse);
            jumpCount++;

            // 애니메이션 제어 (필요에 따라 변경 가능)
            PlayerAnimator.SetInteger("state", 1);
        }

        // 기존 코드
        // if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2) // 원래 isGrounded
        // {
            //PlayerRigid.AddForceY(JupmForce, ForceMode2D.Impulse);
            //isGrounded = false;

            //PlayerAnimator.SetInteger("state", 1);
        //}
    }
    void KillPlayer()
    {
        PlayerCollider.enabled = false;
        PlayerAnimator.enabled = false;
        PlayerRigid.AddForceY(JupmForce, ForceMode2D.Impulse);
    }

    void Hit()
    {
        lives -= 1;
        if(lives == 0)
        {
            KillPlayer();
        }
    }

    void Heal()
    {
        //플레이어 기본 생명= 3
        lives = Mathf.Min(3, lives + 1);
    }

    void StartInvincible()
    {
        isInvincible = true;
        Invoke("StopInvincible", 5f);
    }

    void StopInvincible()
    {
        isInvincible = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Platform")
        {
            isGrounded = true;
            jumpCount = 0; //땅에 닿으면 점프 횟수 초기화
            PlayerAnimator.SetInteger("state", 2);

            // 기존 코드
            //if (!isGrounded)
            //{
            //    PlayerAnimator.SetInteger("state", 2);
            //}
            //isGrounded = true;
            
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "enemy")
        {
            if (!isInvincible)
            {
                Destroy(collider.gameObject);
                Hit();
            }
        }
        else if (collider.gameObject.tag == "food")
        {
            Destroy(collider.gameObject);
            Heal();
        }
        else if (collider.gameObject.tag == "golden")
        {
            Destroy(collider.gameObject);
            StartInvincible();
        }
    }
}
