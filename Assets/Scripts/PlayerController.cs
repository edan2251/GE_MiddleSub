using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //������ ���ӽð�, ���� ����ð�
    public SpriteRenderer spriteRenderer;
    public float effectDuration = 10f;
    public float colorChangeSpeed = 0.01f;

    //����, �̵� ����
    public float moveSpeed = 5f;
    public float jumpForce = 5;
    public Transform groundCheck;
    public LayerMask groundLayer;
    private bool isGrounded;

    //�ִϸ��̼�
    private Rigidbody2D rb;
    private Animator pAni;

    //������
    private bool Strong = false;

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
            pAni.SetBool("Run", false); // ���� ������ �� �ִϸ��̼� ����
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
        if (collision.CompareTag("Item"))
        {
            StartCoroutine(ActivateStrongEffect());
            Destroy(collision.gameObject);
        }

    }



    IEnumerator ActivateStrongEffect()
    {
        Strong = true;
        float elapsedTime = 0f;

        while (elapsedTime < effectDuration)
        {
            float intensity = Mathf.Sin(Time.time * 10f) * 0.5f + 0.5f; // ��� ��ȭ �߰�

            spriteRenderer.color = new Color(
                Mathf.Sin(Time.time * 15f) * 0.8f + 0.5f,
                Mathf.Sin(Time.time * 15f + 2) * 0.8f + 0.5f,
                Mathf.Sin(Time.time * 15f + 4) * 0.8f + 0.5f
            );

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = Color.white; // ���� ������ ����
        Strong = false;
    }



}
