using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float jumpForce = 5;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator pAni;
    private bool isGrounded;
    

    private bool isGiant = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pAni = GetComponent<Animator>();

    
    }


    // Update is called once per frame
    private void Update()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2 (moveInput * moveSpeed, rb.velocity.y);


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

        if(isGrounded && Input.GetKeyDown(KeyCode.Space))
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
            if (isGiant)
                Destroy(collision.gameObject);
            else
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (collision.CompareTag("Item"))
        {
            isGiant = true;
            Destroy(collision.gameObject);
        }

    }


}
