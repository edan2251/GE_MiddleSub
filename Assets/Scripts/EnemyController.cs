using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 3f;

    private Rigidbody2D rb;
    private bool isMovingRight = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    
    void Update()
    {
        if (isMovingRight)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boundary"))
        {
            isMovingRight = !isMovingRight;
        }
    }
}
