using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    // �̻��� ������
    public GameObject missilePrefab;

    // �̻����� �߻�Ǵ� ������ �ӵ�
    public float launchSpeed = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // V �������� �߻� ��
        if (Input.GetKeyDown(KeyCode.V))
        {
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
