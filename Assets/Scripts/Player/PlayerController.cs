using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public GameObject gamePauseUI;
    private bool isPaused = false;

    [SerializeField] private AudioSource sfxSource;         // ȿ���� ����� AudioSource
    [SerializeField] private AudioClip jumpBoostSFX;        // ���� ��ȭ ȿ����
    [SerializeField] private AudioClip stopSFX;

    //������
    [SerializeField] private BGMController bgmController;
    public BossTraceController bossTraceController;
    private Item_StrawBerry_Collected_Effect item_StrawBerry_Collected_Effect;
    private Item_Cherry_Collected_Effect item_Cherry_Collected_Effect;
    private Item_Banana_Collected_Effect item_Banana_Collected_Effect;
    private Item_Box_Collected item_Box_Collected;

    //������ ���ӽð�, ���� ����ð�
    public SpriteRenderer spriteRenderer;
    public float effectDuration = 6f;
    public float colorChangeSpeed = 0.01f;

    //����
    public Transform groundCheck;
    public LayerMask groundLayer;
    private bool isGrounded;

    public float playerMoveSpeed;
    public float playerJumpHigh;

    //�ִϸ��̼�
    private Rigidbody2D rb;
    private Animator pAni;

    //������ ����
    public bool Strong = false;
    public bool isInvincible = false; // ���� ���� �÷���

    // �̵� �ӵ� ����
    private float baseMoveSpeed = 2.5f;
    public float moveSpeed = 2.5f;
    public float speedBoostDuration = 4f; // �ӵ� ���� ���� �ð�
    public bool isSpeedBoosted = false; // �ӵ� ���� ����

    // ���� ���� ����
    private float baseJumpForce = 2.5f;
    public float jumpForce = 2.5f;
    public float jumpBoostDuration = 4f; // �ӵ� ���� ���� �ð�
    public bool isJumpBoosted = false; // �ӵ� ���� ����

    //�ܻ����
    public GameObject afterImagePrefab; // �ܻ� ������
    public int maxAfterImages = 5; // �ܻ��� �ִ� ����
    public float spawnInterval = 0.08f; // �ܻ� ���� ����
    public float fadeSpeed = 2.0f; // ���� ���� �ӵ�

    private float spawnTimer;

    //������ �ڽ�
    public bool isBox = false; //������ ���� ȹ�� ����

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
        gamePauseUI.SetActive(true);   // UI ���̱�
        Time.timeScale = 0f;           // ���� ����
        isPaused = true;
    }

    void ResumeGame()
    {
        gamePauseUI.SetActive(false);  // UI �����
        Time.timeScale = 1f;           // ���� �簳
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
            pAni.SetBool("Run", false); // ���� ������ �� �ִϸ��̼� ����
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


        // �̵� �ӵ��� ���� �̻��� ���� ����
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


    

    //�ܻ� ����
    void SpawnAfterImage()
    {
        GameObject afterImage = Instantiate(afterImagePrefab, transform.position, transform.rotation);
        afterImage.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;

        // �̵� ���⿡ ���� �ܻ� �¿� ����
        Vector3 scale = afterImage.transform.localScale;

        if (isMovingRight()) // ������ �̵� ��
        {
            scale.x = Mathf.Abs(scale.x); // X �������� ����� ����
        }
        else if (isMovingLeft()) // ���� �̵� ��
        {
            scale.x = -Mathf.Abs(scale.x); // X �������� ������ ���� (�¿� ����)
        }

        afterImage.transform.localScale = scale;

        // ���� ����
        StartCoroutine(FadeOut(afterImage));
    }


    // �̵� ���� Ȯ�� �Լ�
    bool isMovingRight()
    {
        return GetComponent<Rigidbody2D>().velocity.x > 0.1f; // ������ �̵� ��
    }

    bool isMovingLeft()
    {
        return GetComponent<Rigidbody2D>().velocity.x < -0.1f; // ���� �̵� ��
    }

    //�ܻ����2
    System.Collections.IEnumerator FadeOut(GameObject obj)
    {
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        Color color = sr.color;

        // �ܻ��� ���� ������������ ó��
        while (color.a > 0)
        {
            color.a -= Time.deltaTime * fadeSpeed;
            sr.color = color;
            yield return null;
        }

        Destroy(obj); // �ܻ� ����
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Respawn"))
        {
            if (!isInvincible) // ������ �ƴ϶�� ���� ����
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
            isBox = true; // ���� ����
            item_Box_Collected = collision.GetComponent<Item_Box_Collected>();
            item_Box_Collected.PlayAndDestroy();
        }
    }



    //���� ���� ���
    IEnumerator ActivateStrongEffect()
    {
        float elapsedTime = 0f;
        

        while (elapsedTime < effectDuration)
        {
            Strong = true;
            isInvincible = true;

            float intensity = Mathf.Sin(Time.time * 6f) * 0.5f + 0.5f; // ��� ��ȭ �߰�

            spriteRenderer.color = new Color(
                Mathf.Sin(Time.time * 15f) * 0.8f + 0.5f,
                Mathf.Sin(Time.time * 15f + 2) * 0.8f + 0.5f,
                Mathf.Sin(Time.time * 15f + 4) * 0.8f + 0.5f
            );

            elapsedTime += Time.deltaTime;
            yield return null;
            
        }

        spriteRenderer.color = Color.white; // ���� ������ ����
        //��� false �϶� ���� �������
        Strong = false;
        isInvincible = false;
    }
    
    // �ӵ� ���� ���
    IEnumerator ActivateSpeedBoost()
    {
        float SpeedStartTime = 0f;

        while (SpeedStartTime < speedBoostDuration)
        {
            moveSpeed = baseMoveSpeed * 2f; // �ӵ� ����
            isSpeedBoosted = true; // �ӵ� ���� ���� ǥ��
            SpeedStartTime += Time.deltaTime;
            yield return null;
        }

        moveSpeed = baseMoveSpeed; // ���� �ӵ��� ����
        isSpeedBoosted = false; // �ӵ� ���� ����
    }

    // ���� ���� ���
    IEnumerator ActivateJumpBoost()
    {
        float JumpBoostStartTime = 0f;

        while (JumpBoostStartTime < jumpBoostDuration)
        {
            jumpForce = baseJumpForce * 2f; // ��������
            isJumpBoosted = true; // ���� ���� ���� ǥ��
            JumpBoostStartTime += Time.deltaTime;
            yield return null;
        }

        jumpForce = baseJumpForce; // ���� ������ ����
        isJumpBoosted = false; // ���� ���� ����
    }

}
