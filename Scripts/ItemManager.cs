using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager im;
    
    [SerializeField]
    GameObject[] heroItems;      // ������ ����θ� ��Ƶ� �迭

    [SerializeField]
    GameObject[] gemsItems;      // ���� ��Ƶ� �迭

    [SerializeField]
    GameObject hpItem;

    [SerializeField]
    GameObject bombItem;

    [SerializeField]
    GameObject IceItem;

    bool canSpawnHero = true;                       // ���� ����� �������� ������ �� �ִ��� �Ǵ��� ����
    List<bool> spawnHero = new List<bool>();        // ���� ����ε��� �����Ǿ��ִ��� ���θ� ������ ����Ʈ

    int heroCount = 1;  // ���� ����� ��
    AudioSource clip;


    // Start is called before the first frame update
    void Start()
    {
        im = this;

        for(int i = 0; i < 9;i++)
        {
            spawnHero.Add(false);
        }

        clip = GetComponent<AudioSource>();

        spawnHero[DataManager.dm.CURRENTHERO] = true;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnItem(Vector2 pos)
    {
        int temp = Random.Range(0, 5);

        if (temp == 0)
        {
            if(heroCount == 9)
            {
                return;
            }

            int num = Random.Range(0, 9);

            while(true)
            {
                if(spawnHero[num])
                {
                    num = Random.Range(0, 9);       // ������ ����� ������ �缳��
                }
                else
                {
                    spawnHero[num] = true;
                    break;
                }
            }

            GameObject hero = Instantiate(heroItems[num], pos, Quaternion.identity);    // ����� ������ ����
            heroCount++;    // ����� �� ����
        }
        else if(temp == 1)
        {
            int num = Random.Range(0, 3);

            GameObject gem = Instantiate(gemsItems[num], pos, Quaternion.identity);
        }
        else if (temp == 2)
        {
            GameObject potion = Instantiate(hpItem, pos, Quaternion.identity);
        }
        else if (temp == 3)
        {
            GameObject bomb = Instantiate(bombItem, pos, Quaternion.identity);

        }
        else if (temp == 4)
        {
            GameObject Ice = Instantiate(IceItem, pos, Quaternion.identity);
        }

    }

    public void AddHeroItem(int id) // ����� ������ ���߰�
    {
        spawnHero[id] = false;
        heroCount--;
    }

    public void GetItemSound()
    {
        clip.Play();
    }

}
