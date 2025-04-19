using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_StrawBerry_Collected_Effect : MonoBehaviour
{
    private bool isCollected = false;


    public void PlayAndDestroy()
    {
            if (isCollected) return; // 이미 처리됐으면 무시
            isCollected = true;

            // 딸기 안보이게 하기
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;

            SpriteRenderer spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = true;

            // 애니메이션 실행
            Animator collectedAnim = transform.Find("Collected").GetComponent<Animator>();
            collectedAnim.SetTrigger("Play_Collected");

            // 일정 시간 후 파괴
            Destroy(this.gameObject, 0.5f);
    }
}
