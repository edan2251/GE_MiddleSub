using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Missile : MonoBehaviour
{
    // 미사일 프리팹
    public GameObject missilePrefab;

    // 미사일이 발사되는 순간의 속도
    public float launchSpeed = 15.0f;

    private float fireCooldown = 0.7f; // 쿨다운 시간
    private float lastFireTime = -Mathf.Infinity; // 마지막 발사 시간 초기화
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Z 눌렀을때 발사 슛
        if (Input.GetKeyDown(KeyCode.Z) && Time.time >= lastFireTime + fireCooldown)
        {
            // 발사 시점 갱신
            lastFireTime = Time.time;

            // 프리팹으로부터 새로운 미사일 게임 오브젝트 생성
            GameObject missile = Instantiate(missilePrefab, transform.position, transform.rotation);

            // 미사일로부터 리지드바디 2D 컴포넌트 가져옴
            Rigidbody2D rb = missile.GetComponent<Rigidbody2D>();

            // 플레이어의 스케일로 방향 판별
            Vector2 launchDirection = transform.localScale.x > 0 ? transform.right : -transform.right;

            // 힘을 가하기
            rb.AddForce(launchDirection * launchSpeed, ForceMode2D.Impulse);

            Destroy(missile, 2f);
        }

        
    }

    
}
