using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossController : MonoBehaviour
{
    public BossTraceController bossTraceController;

    public int Health = 100;
    public int Damage = 10;

    private Animator bAni;
    public bool isHitAnimation = false;

    // Start is called before the first frame update
    void Start()
    {
        bAni = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //isMoving
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (bossTraceController.isMoving == false)
        {
            if (collision.CompareTag("Missile"))
            {
                Destroy(collision.gameObject);
                Health -= Damage;

                if (isHitAnimation == false)
                {
                    StartCoroutine(PlayHitAction());
                }

                if (Health <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
        if (bossTraceController.isMoving == true && collision.CompareTag("Missile"))
        {
            Destroy(collision.gameObject);
        }


    }
    private IEnumerator PlayHitAction()
    {
        isHitAnimation = true;
        bAni.SetBool("isHit", true);

        // �ִϸ��̼� ���̸�ŭ ��ٸ�
        yield return new WaitForSeconds(0.4f);

        bAni.SetBool("isHit", false);
        isHitAnimation = false;
    }



}
