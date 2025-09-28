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

    // ���� ������ ���� ���� Ƚ�� ī��Ʈ(0 = �ٴ�, 1 = ���� ����, 2 = ���� ����)
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
            // y�ӵ��� �ʱ�ȭ�ؼ� ���������� �����ϰ� �۵��ϵ���
            PlayerRigid.linearVelocity = new Vector2(PlayerRigid.linearVelocity.x, 0);

            PlayerRigid.AddForce(Vector2.up * JupmForce, ForceMode2D.Impulse);
            jumpCount++;

            // �ִϸ��̼� ���� (�ʿ信 ���� ���� ����)
            PlayerAnimator.SetInteger("state", 1);
        }

        // ���� �ڵ�
        // if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2) // ���� isGrounded
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
        //�÷��̾� �⺻ ����= 3
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
            jumpCount = 0; //���� ������ ���� Ƚ�� �ʱ�ȭ
            PlayerAnimator.SetInteger("state", 2);

            // ���� �ڵ�
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
