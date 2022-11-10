using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHero : MonoBehaviour
{
    [SerializeField]
    int id = 0; // ���� ������ ID

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Hero") && collision.GetComponent<HeroMoveController>().HEAD)
        {
            HeroManager.hm.AddHero(id);
            ItemManager.im.GetItemSound();
            Destroy(gameObject);
        }
    }
}
