using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTraceController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float raycastDistance = 0.2f;
    public float traceDistance = 50f;

    private Transform player;
    private Animator animator;

    private float moveTime = 3.0f;
    private float waitTime = 4.0f;
    public bool isMoving = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        if (isMoving == true)
        {
            
            moveTime -= Time.deltaTime;
            PerformMovement();

            if (moveTime <= 0)
            {
                
                animator.SetTrigger("Boss_Idle"); // Idle 애니메이션 출력
                waitTime = 4.0f;
                isMoving = false;

            }
        }
        if (isMoving == false)
        {
            
            waitTime -= Time.deltaTime;

            if (waitTime <= 0)
            {
                animator.SetTrigger("Boss_Run"); // Run 애니메이션 출력
                moveTime = 3.0f;
                isMoving = true;

            }
        }
    }


    //Trace이동
    private void PerformMovement()
    {
        Vector2 direction = player.position - transform.position;

        if (direction.magnitude > traceDistance)
            return;

        Vector2 directionNormalized = direction.normalized;

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, directionNormalized, raycastDistance);
        Debug.DrawRay(transform.position, directionNormalized * raycastDistance, Color.red);

        foreach (RaycastHit2D rHit in hits)
        {
            if (rHit.collider != null && rHit.collider.CompareTag("Obstacle"))
            {
                Vector3 alternativeDirection = Quaternion.Euler(0f, 0f, -90f) * direction;
                transform.Translate(alternativeDirection * moveSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(direction * moveSpeed * Time.deltaTime);
            }
        }

        // 방향에 따라 보스 회전
        if (direction.x < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    
}
