using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Missile : MonoBehaviour
{
    // �̻��� ������
    public GameObject missilePrefab;

    // �̻����� �߻�Ǵ� ������ �ӵ�
    public float launchSpeed = 15.0f;

    private float fireCooldown = 0.7f; // ��ٿ� �ð�
    private float lastFireTime = -Mathf.Infinity; // ������ �߻� �ð� �ʱ�ȭ
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Z �������� �߻� ��
        if (Input.GetKeyDown(KeyCode.Z) && Time.time >= lastFireTime + fireCooldown)
        {
            // �߻� ���� ����
            lastFireTime = Time.time;

            // ���������κ��� ���ο� �̻��� ���� ������Ʈ ����
            GameObject missile = Instantiate(missilePrefab, transform.position, transform.rotation);

            // �̻��Ϸκ��� ������ٵ� 2D ������Ʈ ������
            Rigidbody2D rb = missile.GetComponent<Rigidbody2D>();

            // �÷��̾��� �����Ϸ� ���� �Ǻ�
            Vector2 launchDirection = transform.localScale.x > 0 ? transform.right : -transform.right;

            // ���� ���ϱ�
            rb.AddForce(launchDirection * launchSpeed, ForceMode2D.Impulse);

            Destroy(missile, 2f);
        }

        
    }

    
}
