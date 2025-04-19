using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_StrawBerry_Collected_Effect : MonoBehaviour
{
    private bool isCollected = false;


    public void PlayAndDestroy()
    {
            if (isCollected) return; // �̹� ó�������� ����
            isCollected = true;

            // ���� �Ⱥ��̰� �ϱ�
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;

            SpriteRenderer spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = true;

            // �ִϸ��̼� ����
            Animator collectedAnim = transform.Find("Collected").GetComponent<Animator>();
            collectedAnim.SetTrigger("Play_Collected");

            // ���� �ð� �� �ı�
            Destroy(this.gameObject, 0.5f);
    }
}
