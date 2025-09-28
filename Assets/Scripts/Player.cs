using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Settings")]
    public float JupmForce;

    [Header("Reference")]
    public Rigidbody2D PlayerRigid;
    private bool isGrounded = true;
    public Animator PlayerAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {

            PlayerRigid.AddForceY(JupmForce, ForceMode2D.Impulse);
            isGrounded = false;
            PlayerAnimator.SetInteger("state", 1);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "Platform")
        {
            if (!isGrounded)
            {
                PlayerAnimator.SetInteger("state", 2);
            }
            isGrounded = true;
            
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "enemy")
        {

        }
        else if (collision.gameObject.tag == "food")
        {

        }
        else if (collision.gameObject.tag == "golden")
        {

        }
    }
}
