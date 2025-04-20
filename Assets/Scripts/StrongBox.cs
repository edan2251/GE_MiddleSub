using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongBox : MonoBehaviour
{
    public GameObject strongBoxPrefab;

    public PlayerController playerController;
    

    // ���ڰ� �߻�Ǵ� ������ �ӵ�
    public float BoxLaunchSpeed = 10.0f;

    private float BoxFireCooldown = 1.5f; // ��ٿ� �ð�
    private float BoxLastFireTime = -Mathf.Infinity; // ������ �߻� �ð� �ʱ�ȭ

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
                //�ڽ� ȹ����� ����
                playerController.isBox = false;

                // �߻� ���� ����
                BoxLastFireTime = Time.time;

                // ���������κ��� ���ο� �̻��� ���� ������Ʈ ����
                GameObject StrongBox = Instantiate(strongBoxPrefab, transform.position, transform.rotation);

                // �̻��Ϸκ��� ������ٵ� 2D ������Ʈ ������
                Rigidbody2D rb = StrongBox.GetComponent<Rigidbody2D>();

                // �÷��̾��� �����Ϸ� ���� �Ǻ�
                Vector2 launchDirection = transform.localScale.x > 0 ? transform.right : -transform.right;

                // ���� ���ϱ�
                rb.AddForce(launchDirection * BoxLaunchSpeed, ForceMode2D.Impulse);

                Destroy(StrongBox, 5f);
            }
        }
    }

}
