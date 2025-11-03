using UnityEngine;
using System.Collections;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("Settings")]
    public float JupmForce = 12f;
    private const float PushbackDistance = 0.5f;

    [Header("Hit Counter")]
    private int knockbackCount = 0;
    public int maxKnockbackHits = 3;
    public float FinalKnockbackForce = 15f;

    [Header("Reference")]
    public Rigidbody2D PlayerRigid;
    public Animator PlayerAnimator;
    public BoxCollider2D PlayerCollider;

    // UI 관리 스크립트 참조 변수
    private UIManager uiManager;

    [Header("State")]
    public float platformScrollSpeed = 0.8f;
    private bool isGrounded = true;
    private int jumpCount = 0;
    private bool isInvincible = false;
    private bool isDead = false;
    private Coroutine invincibilityCoroutine;

    // 거리 측정용 변수
    private float startX;
    private float currentDistance;

    void Start()
    {
        if (PlayerRigid == null)
        {
            PlayerRigid = GetComponent<Rigidbody2D>();
        }
        Time.timeScale = 1f;

        uiManager = FindObjectOfType<UIManager>();
        if (uiManager == null)
        {
            Debug.LogError("UIManager를 찾을 수 없습니다! Canvas에 스크립트를 연결했는지 확인하세요.");
        }
        // 시작 X 위치 저장
        startX = transform.position.x;
    }

    void Update()
    {
        if (isDead || isInvincible) return;

        // 현재 이동 거리 계산
        currentDistance = transform.position.x - startX;

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2)
        {
            PlayerRigid.linearVelocity = new Vector2(PlayerRigid.linearVelocity.x, 0);
            PlayerRigid.AddForce(Vector2.up * JupmForce, ForceMode2D.Impulse);
            jumpCount++;
            PlayerAnimator.SetInteger("state", 1);
        }
    }

    void HandleHitCount()
    {
        knockbackCount++;

        if (knockbackCount >= maxKnockbackHits)
        {
            FinalKnockbackToBird();
        }
    }

    // 3회째 피격 시 최종 넉백 (게임 오버 유도)
    void FinalKnockbackToBird()
    {
        if (isDead) return;

        // isDead = true; // <-- GameOver()가 호출되지 못하게 막던 이 줄을 제거했습니다!

        Debug.Log("최종 피격! 왼쪽 새를 향해 밀려납니다.");

        PlayerRigid.linearVelocity = Vector2.zero;
        PlayerRigid.linearVelocity = Vector2.zero;

        Vector2 finalKnockbackDirection = new Vector2(-1.5f, 0.7f).normalized;
        PlayerRigid.AddForce(finalKnockbackDirection * FinalKnockbackForce, ForceMode2D.Impulse);

        // GameOver()가 isDead 설정을 포함한 모든 처리를 담당합니다.
        GameOver();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.name == "Platform")
        {
            isGrounded = true;
            jumpCount = 0;
            PlayerAnimator.SetInteger("state", 2);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (isDead) return;

        if (collider.gameObject.CompareTag("obstacle"))
        {
            if (isInvincible) return;
            PushBackLeftAndStop();
            HandleHitCount();
            Destroy(collider.gameObject);
            invincibilityCoroutine = StartCoroutine(InvincibilityDelay());
        }

        else if (collider.gameObject.CompareTag("emeny"))
        {
            GameOver();
        }

        else if (collider.gameObject.CompareTag("food") || collider.gameObject.CompareTag("golden"))
        {
            Destroy(collider.gameObject);
        }
    }

    void PushBackLeftAndStop()
    {
        Vector3 pushVector = new Vector3(-PushbackDistance, 0, 0);
        transform.position += pushVector;
        PlayerRigid.linearVelocity = Vector2.zero;
        PlayerRigid.angularVelocity = 0f;
    }

    IEnumerator InvincibilityDelay()
    {
        isInvincible = true;
        yield return new WaitForSeconds(0.5f);
        isInvincible = false;
    }

    // 게임 오버 함수: Player의 물리 비활성화 및 UIManager 호출 담당
    void GameOver()
    {
        if (isDead) return; // 중복 호출 방지
        isDead = true; // 사망 상태 설정
        Debug.Log("게임 오버! (Player 스크립트)");

        // 1. UIManager에게 전체 게임 정지 및 UI 표시를 위임
        if (uiManager != null)
        {
            uiManager.HandleGameOver();
            if(uiManager.finalDistanceText.text != null)
            {
                uiManager.finalDistanceText.text = $"Distance: {currentDistance:F1}m";
            }
        }
        else
        {
            Debug.LogError("GameOver가 호출되었으나, UIManager가 null입니다!");
        }

        // 2. 플레이어의 물리 및 애니메이션 비활성화
        PlayerRigid.simulated = false;
        PlayerAnimator.enabled = false;
        PlayerCollider.enabled = false;
    }
}
// (사용되지 않는 기타 함수들은 필요하다면 주석 해제하여 사용하세요)
// void KillPlayer() { ... }
// void Hit() { ... }
// void Heal() { ... }
// void StartInvincible() { ... }
// void StopInvincible() { ... }