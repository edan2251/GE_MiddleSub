using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongBox : MonoBehaviour
{
    public GameObject strongBoxPrefab;

    public PlayerController playerController;
    

    // 상자가 발사되는 순간의 속도
    public float BoxLaunchSpeed = 10.0f;

    private float BoxFireCooldown = 1.5f; // 쿨다운 시간
    private float BoxLastFireTime = -Mathf.Infinity; // 마지막 발사 시간 초기화

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.isBox == true)
        {
            if (Input.GetKeyDown(KeyCode.X) && Time.time >= BoxLastFireTime + BoxFireCooldown)
            {
                //박스 획득상태 꺼짐
                playerController.isBox = false;

                // 발사 시점 갱신
                BoxLastFireTime = Time.time;

                // 프리팹으로부터 새로운 미사일 게임 오브젝트 생성
                GameObject StrongBox = Instantiate(strongBoxPrefab, transform.position, transform.rotation);

                // 미사일로부터 리지드바디 2D 컴포넌트 가져옴
                Rigidbody2D rb = StrongBox.GetComponent<Rigidbody2D>();

                // 플레이어의 스케일로 방향 판별
                Vector2 launchDirection = transform.localScale.x > 0 ? transform.right : -transform.right;

                // 힘을 가하기
                rb.AddForce(launchDirection * BoxLaunchSpeed, ForceMode2D.Impulse);

                Destroy(StrongBox, 5f);
            }
        }
    }

}
