using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class BossController : MonoBehaviour
{
    public BossTraceController bossTraceController;
    public PlayerController playerController;

    [SerializeField] private AudioSource sfxSource;     // 보스 효과음 재생용 AudioSource
    [SerializeField] private AudioClip hitSFX;          // 피격 효과음
    [SerializeField] private AudioClip stopSFX;

    public GameObject gameClearUI;

    public int Health = 150;
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
                sfxSource.PlayOneShot(hitSFX);

                if (isHitAnimation == false)
                {
                    StartCoroutine(PlayHitAction());
                }

                if (Health <= 0)
                {
                    HighScore.TrySet(SceneManager.GetActiveScene().buildIndex, (int)playerController.score);
                    gameClearUI.SetActive(true);
                    Destroy(gameObject);
                }
            }
        }
        if (bossTraceController.isMoving == true && collision.CompareTag("Missile"))
        {
            Destroy(collision.gameObject);
        }
        if (bossTraceController.isMoving == true && collision.CompareTag("StrongBox"))
        {
            bossTraceController.BossStopAnimation();
            sfxSource.PlayOneShot(stopSFX);
            Destroy(collision.gameObject); 
        }

    }
    private IEnumerator PlayHitAction()
    {
        isHitAnimation = true;
        bAni.SetBool("isHit", true);

        // 애니메이션 길이만큼 기다림
        yield return new WaitForSeconds(0.4f);

        bAni.SetBool("isHit", false);
        isHitAnimation = false;
    }



}
