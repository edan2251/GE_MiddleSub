using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //아이템 지속시간, 색깔 변경시간
    public SpriteRenderer spriteRenderer;
    public float effectDuration = 10f;
    public float colorChangeSpeed = 0.01f;

    //점프, 이동 관련
    public float moveSpeed = 5f;
    public float jumpForce = 5;
    public Transform groundCheck;
    public LayerMask groundLayer;
    private bool isGrounded;

    //애니메이션
    private Rigidbody2D rb;
    private Animator pAni;

    //아이템
    private bool Strong = false;
    public bool isInvincible = false; // 무적 상태 플래그

    // 이동 속도 관련
    public float playerMoveSpeed;
    public float speedBoostDuration = 7f; // 속도 증가 지속 시간
    public bool isSpeedBoosted = false; // 속도 증가 여부

    // 점프 증가 관련
    public float playerJumpHigh;
    public float jumpBoostDuration = 7f; // 속도 증가 지속 시간
    public bool isJumpBoosted = false; // 속도 증가 여부


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pAni = GetComponent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        


    }


    private void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);


        if (moveInput > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
            pAni.SetBool("Run", true);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            pAni.SetBool("Run", true);
        }
        else
        {
            pAni.SetBool("Run", false); // 정지 상태일 때 애니메이션 멈춤
        }



        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            pAni.SetTrigger("JumpAction");
        }







    }//.Update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Respawn"))
        {
            if (!isInvincible) // 무적이 아니라면 함정 적용
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        if (collision.CompareTag("Finish"))
        {
            collision.GetComponent<LevelObject>().MoveToNextLevel();
        }

        if (collision.CompareTag("Enemy"))
        {
            if (Strong)
                Destroy(collision.gameObject);
            else
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (collision.CompareTag("Item_Mujuck"))
        {
            StartCoroutine(ActivateStrongEffect());
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("Item_Run"))
        {
            StartCoroutine(ActivateSpeedBoost());
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("Item_Jump"))
        {
            StartCoroutine(ActivateJumpBoost());
            Destroy(collision.gameObject);
        }

    }


    //무적 상태 기능
    IEnumerator ActivateStrongEffect()
    {
        Strong = true;
        isInvincible = true;
        float elapsedTime = 0f;

        while (elapsedTime < effectDuration)
        {
            float intensity = Mathf.Sin(Time.time * 10f) * 0.5f + 0.5f; // 밝기 변화 추가

            spriteRenderer.color = new Color(
                Mathf.Sin(Time.time * 15f) * 0.8f + 0.5f,
                Mathf.Sin(Time.time * 15f + 2) * 0.8f + 0.5f,
                Mathf.Sin(Time.time * 15f + 4) * 0.8f + 0.5f
            );

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = Color.white; // 원래 색으로 복귀
        Strong = false;
        isInvincible = false;
    }
    
    // 속도 증가 기능
    IEnumerator ActivateSpeedBoost()
    {
        playerMoveSpeed = moveSpeed; // 기본 이동 속도
        float originalSpeed = playerMoveSpeed; // 기존 속도 저장
        moveSpeed *= 2f; // 속도 증가
        isSpeedBoosted = true; // 속도 증가 상태 표시

        float SpeedStartTime = 0f;
        while (SpeedStartTime < speedBoostDuration)
        {
            SpeedStartTime += Time.deltaTime;
            yield return null;
        }

        moveSpeed = originalSpeed; // 원래 속도로 복귀
        isSpeedBoosted = false; // 속도 증가 종료
    }

    // 점프 관련 기능
    IEnumerator ActivateJumpBoost()
    {
        playerJumpHigh = jumpForce; // 기본 점프
        float originalJump = playerJumpHigh; // 기존 점프 저장
        jumpForce *= 2f; // 점프증가
        isJumpBoosted = true; // 점프 증가 상태 표시

        float JumpBoostStartTime = 0f;
        while (JumpBoostStartTime < jumpBoostDuration)
        {
            JumpBoostStartTime += Time.deltaTime;
            yield return null;
        }

        jumpForce = originalJump; // 원래 점프로 복귀
        isJumpBoosted = false; // 점프 증가 종료
    }

}
