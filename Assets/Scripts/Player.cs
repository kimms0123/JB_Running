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
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded) {
            //Rb.linearVelocityX = 2;
            //Rb.linearVelocityY = 3;
            Rb.AddForceY(JumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            Ani.SetInteger("state", 1);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) //다른 collider와 충돌했을 때
    {
        if(collision.gameObject.name == "Platfrom") {
            isGrounded = true;
            Ani.SetInteger("state", 2);
        }
    }
}
