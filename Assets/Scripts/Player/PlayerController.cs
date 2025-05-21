using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameObject gamePauseUI;
    private bool isPaused = false;

    [SerializeField] private AudioSource sfxSource;         // 효과음 재생용 AudioSource
    [SerializeField] private AudioClip jumpBoostSFX;        // 점프 강화 효과음
    [SerializeField] private AudioClip stopSFX;

    //참조들
    [SerializeField] private BGMController bgmController;
    public BossTraceController bossTraceController;
    private Item_StrawBerry_Collected_Effect item_StrawBerry_Collected_Effect;
    private Item_Cherry_Collected_Effect item_Cherry_Collected_Effect;
    private Item_Banana_Collected_Effect item_Banana_Collected_Effect;
    private Item_Box_Collected item_Box_Collected;

    //아이템 지속시간, 색깔 변경시간
    public SpriteRenderer spriteRenderer;
    public float effectDuration = 6f;
    public float colorChangeSpeed = 0.01f;

    //점프
    public Transform groundCheck;
    public LayerMask groundLayer;
    private bool isGrounded;

    public float playerMoveSpeed;
    public float playerJumpHigh;

    //애니메이션
    private Rigidbody2D rb;
    private Animator pAni;

    //아이템 상태
    public bool Strong = false;
    public bool isInvincible = false; // 무적 상태 플래그

    // 이동 속도 관련
    private float baseMoveSpeed = 2.5f;
    public float moveSpeed = 2.5f;
    public float speedBoostDuration = 4f; // 속도 증가 지속 시간
    public bool isSpeedBoosted = false; // 속도 증가 여부

    // 점프 증가 관련
    private float baseJumpForce = 2.5f;
    public float jumpForce = 2.5f;
    public float jumpBoostDuration = 4f; // 속도 증가 지속 시간
    public bool isJumpBoosted = false; // 속도 증가 여부

    //잔상관련
    public GameObject afterImagePrefab; // 잔상 프리팹
    public int maxAfterImages = 5; // 잔상의 최대 개수
    public float spawnInterval = 0.08f; // 잔상 생성 간격
    public float fadeSpeed = 2.0f; // 투명도 감소 속도

    private float spawnTimer;

    //아이템 박스
    public bool isBox = false; //아이템 상자 획득 여부

    public float score;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pAni = GetComponent<Animator>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        score = 1000f;
    }

    void PauseGame()
    {
        gamePauseUI.SetActive(true);   // UI 보이기
        Time.timeScale = 0f;           // 게임 정지
        isPaused = true;
    }

    void ResumeGame()
    {
        gamePauseUI.SetActive(false);  // UI 숨기기
        Time.timeScale = 1f;           // 게임 재개
        isPaused = false;
    }

    private void Update()
    {
        if (Strong)
        {
            bgmController.SwitchToInvincibleBGM();
        }
        else if (isSpeedBoosted)
        {
            bgmController.SwitchToSpeedBoostBGM();
        }
        else
        {
            bgmController.SwitchToNormalBGM();
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }

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

            if (isJumpBoosted)
            {
                sfxSource.PlayOneShot(jumpBoostSFX);
            }
        }


        // 이동 속도가 일정 이상일 때만 실행
        if (isSpeedBoosted)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnInterval)
            {
                SpawnAfterImage();
                spawnTimer = 0;
            }
        }


        score -= Time.deltaTime;
    }//.Update


    

    //잔상 관련
    void SpawnAfterImage()
    {
        GameObject afterImage = Instantiate(afterImagePrefab, transform.position, transform.rotation);
        afterImage.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;

        // 이동 방향에 따라 잔상 좌우 반전
        Vector3 scale = afterImage.transform.localScale;

        if (isMovingRight()) // 오른쪽 이동 중
        {
            scale.x = Mathf.Abs(scale.x); // X 스케일을 양수로 설정
        }
        else if (isMovingLeft()) // 왼쪽 이동 중
        {
            scale.x = -Mathf.Abs(scale.x); // X 스케일을 음수로 설정 (좌우 반전)
        }

        afterImage.transform.localScale = scale;

        // 투명도 설정
        StartCoroutine(FadeOut(afterImage));
    }


    // 이동 방향 확인 함수
    bool isMovingRight()
    {
        return GetComponent<Rigidbody2D>().velocity.x > 0.1f; // 오른쪽 이동 중
    }

    bool isMovingLeft()
    {
        return GetComponent<Rigidbody2D>().velocity.x < -0.1f; // 왼쪽 이동 중
    }

    //잔상관련2
    System.Collections.IEnumerator FadeOut(GameObject obj)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        Color color = sr.color;

        // 잔상이 점점 투명해지도록 처리
        while (color.a > 0)
        {
            color.a -= Time.deltaTime * fadeSpeed;
            sr.color = color;
            yield return null;
        }

        Destroy(obj); // 잔상 제거
    }


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
            //HighScore.TrySet(SceneManager.GetActiveScene().buildIndex, (int)score);
            StageResultSaver.SaveStage(SceneManager.GetActiveScene().buildIndex, (int)score);

            collision.GetComponent<LevelObject>().MoveToNextLevel();
        }

        if (collision.CompareTag("Boss"))
        {
            if (Strong)
            {
                bossTraceController.BossStopAnimation();
                sfxSource.PlayOneShot(stopSFX);
            }
            else if (bossTraceController.isMoving == true)
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
            item_Banana_Collected_Effect = collision.GetComponent<Item_Banana_Collected_Effect>();
            StartCoroutine(ActivateStrongEffect());
            item_Banana_Collected_Effect.PlayAndDestroy();
        }

        if (collision.CompareTag("Item_Run"))
        {
            item_StrawBerry_Collected_Effect = collision.GetComponent<Item_StrawBerry_Collected_Effect>();
            StartCoroutine(ActivateSpeedBoost());
            item_StrawBerry_Collected_Effect.PlayAndDestroy();
        }

        if (collision.CompareTag("Item_Jump"))
        {
            item_Cherry_Collected_Effect = collision.GetComponent<Item_Cherry_Collected_Effect>();
            StartCoroutine(ActivateJumpBoost());
            item_Cherry_Collected_Effect.PlayAndDestroy();
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.C) && collision.CompareTag("Item_Box"))
        {
            isBox = true; // 상태 변경
            item_Box_Collected = collision.GetComponent<Item_Box_Collected>();
            item_Box_Collected.PlayAndDestroy();
        }
    }



    //무적 상태 기능
    IEnumerator ActivateStrongEffect()
    {
        float elapsedTime = 0f;
        

        while (elapsedTime < effectDuration)
        {
            Strong = true;
            isInvincible = true;

            float intensity = Mathf.Sin(Time.time * 6f) * 0.5f + 0.5f; // 밝기 변화 추가

            spriteRenderer.color = new Color(
                Mathf.Sin(Time.time * 15f) * 0.8f + 0.5f,
                Mathf.Sin(Time.time * 15f + 2) * 0.8f + 0.5f,
                Mathf.Sin(Time.time * 15f + 4) * 0.8f + 0.5f
            );

            elapsedTime += Time.deltaTime;
            yield return null;
            
        }

        spriteRenderer.color = Color.white; // 원래 색으로 복귀
        //잠깐 false 일때 버그 생길수도
        Strong = false;
        isInvincible = false;
    }
    
    // 속도 증가 기능
    IEnumerator ActivateSpeedBoost()
    {
        float SpeedStartTime = 0f;

        while (SpeedStartTime < speedBoostDuration)
        {
            moveSpeed = baseMoveSpeed * 2f; // 속도 증가
            isSpeedBoosted = true; // 속도 증가 상태 표시
            SpeedStartTime += Time.deltaTime;
            yield return null;
        }

        moveSpeed = baseMoveSpeed; // 원래 속도로 복귀
        isSpeedBoosted = false; // 속도 증가 종료
    }

    // 점프 관련 기능
    IEnumerator ActivateJumpBoost()
    {
        float JumpBoostStartTime = 0f;

        while (JumpBoostStartTime < jumpBoostDuration)
        {
            jumpForce = baseJumpForce * 2f; // 점프증가
            isJumpBoosted = true; // 점프 증가 상태 표시
            JumpBoostStartTime += Time.deltaTime;
            yield return null;
        }

        jumpForce = baseJumpForce; // 원래 점프로 복귀
        isJumpBoosted = false; // 점프 증가 종료
    }

}
