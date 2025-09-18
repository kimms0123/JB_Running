using System;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Settings")]
    public float JumpForce;

    [Header("References")]
    public Rigidbody2D Rb;
    public Animator Ani;

    private bool isGrounded = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            //Rb.linearVelocityX = 2;
            //Rb.linearVelocityY = 3;
            Rb.AddForceY(JumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            Ani.SetInteger("state", 1);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) //collider에 닿았는지 감지
    {
        if (collision.gameObject.name == "Platfrom")
        {
            isGrounded = true;
            Ani.SetInteger("state", 2);
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
