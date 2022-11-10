using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroManager : MonoBehaviour
{
    public static HeroManager hm;
    [SerializeField]
    GameObject[] prefabHeroes;  // ���� ���� ������ ������ ������ ������
    [SerializeField]
    GameObject hpBar;           // ���� ���� �� �Բ� ������ HP��
    GameObject hpBarPat;        // ������ HP�ٵ��� �θ� ������Ʈ


    [SerializeField]
    float inputRate = 0.1f;     // �̵� Ű �Է� ����
    float inputTime = 0;        // ���� üũ�� ����
    [SerializeField]
    float offset = 1.5f;        // ���� �� ����

    bool isCanInput = false;    // ���� �̵� Ű �Է� ���� �Ұ��� üũ�� ����
    
    int heroLength = 1;         // ���� ����� ��
    [SerializeField]
    HeroMoveController headHero;    // ��θӸ� ����� 
    public Transform HEAD { get { return headHero.transform; } }    // ��θӸ� ����� ��ġ�� �Ѱ��� ����
    [SerializeField]
    List<HeroMoveController> heroList = new List<HeroMoveController>();
    
    public List<HeroMoveController> LIST { get { return heroList; } }


    // Start is called before the first frame update
    void Start()
    {
        hm = this;
        hpBarPat = GameObject.Find("HeroHpBars");
        CreateHeadHero();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.gm.GameStart || GameManager.gm.GameOver) return;

        if (!isCanInput)
        {
            inputTime += Time.deltaTime;
            if(inputTime >= inputRate)
            {
                isCanInput = true;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                if(headHero.DIR == Direction.UP || headHero.DIR == Direction.DOWN)
                {
                    return;
                }
                ChangeMoveDirection(Direction.UP);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (headHero.DIR == Direction.UP || headHero.DIR == Direction.DOWN)
                {
                    return;
                }
                ChangeMoveDirection(Direction.DOWN);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                if (headHero.DIR == Direction.LEFT || headHero.DIR == Direction.RIGHT)
                {
                    return;
                }
                ChangeMoveDirection(Direction.LEFT);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                if (headHero.DIR == Direction.LEFT || headHero.DIR == Direction.RIGHT)
                {
                    return;
                }
                ChangeMoveDirection(Direction.RIGHT);
            }
        }
    }

    void ChangeMoveDirection(Direction _dir)    // �����̵� �Լ�
    {
        headHero.Move(_dir);

        if(heroList.Count > 1)
        {
            for(int i = 1; i < heroList.Count; i++)
            {
                heroList[i].AddPosDir(headHero.transform.position, _dir);   // ��ġ�� ���� ����
            }
        }



        inputTime = 0;
        isCanInput = false;
    }

    void CreateHeadHero()
    {
        GameObject hero = Instantiate(prefabHeroes[DataManager.dm.CURRENTHERO], transform.position, Quaternion.identity);
        hero.transform.SetParent(transform.GetChild(0));

        headHero = hero.GetComponent<HeroMoveController>();
        heroList.Add(headHero);


        headHero.HEAD = true;
        headHero.Move(Direction.DOWN);

        CreateHpBar(headHero.GetComponentInChildren<HeroComponent>());
        GameManager.gm.GameBegin();
    }

    public void AddHero(int id)
    {
        HeroMoveController preHero = heroList[heroLength - 1];  // ������ ���� ���� ������ ������

        Vector2 pos = Vector2.down;
        Vector2 pPos = preHero.transform.position;

        if(preHero.DIR == Direction.UP) {pos = new Vector2(pPos.x, pPos.y - offset);}       // ��ġ����
        else if(preHero.DIR == Direction.DOWN) { pos = new Vector2(pPos.x, pPos.y + offset); }
        else if (preHero.DIR == Direction.LEFT) { pos = new Vector2(pPos.x + offset, pPos.y); }
        else { pos = new Vector2(pPos.x - offset, pPos.y); }

        GameObject temp = Instantiate(prefabHeroes[id], pos, Quaternion.identity);  // ���� ����
        temp.transform.SetParent(transform.GetChild(0));                            // ������ ���� �θ� ����

        HeroMoveController hero = temp.GetComponent<HeroMoveController>();          // ������ ������ ����ι�����Ʈ�ѷ� ��Ƶ�

        hero.ID = heroLength;
        
        for(int i = 0; i < preHero.GetPosDir().Count; i++)
        {
            hero.GetPosDir().Add(preHero.GetPosDir()[i]);   
            // �� ������ ��ġ�� ������ ����� ����Ʈ�� ������ �������� ����
        }

        CreateHpBar(hero.GetComponentInChildren<HeroComponent>());  // HP�� ����

        hero.Move(preHero.DIR); // ���⼳�� �� �̵� �Լ� ȣ��!!

        heroLength++;           // ���� ũ�� �߰�
        heroList.Add(hero);     // ����Ʈ�� ���� �߰�


    }

    public void AllHeroDie()
    {
        foreach (HeroMoveController i in heroList)
        {
            Destroy(i.gameObject);
        }

        GameManager.gm.GameEnd();
        heroList.Clear();


    }

    void CreateHpBar(HeroComponent hc)
    {
        GameObject bar = Instantiate(hpBar);                // hp �� ����
        bar.transform.SetParent(hpBarPat.transform);        // hp �� �θ���
        hc.Init(50, 5, 0.5f, bar.GetComponent<Slider>());   // ������ hpBar ����
    }

    public void RemoveHero(HeroMoveController hmc)
    {
        for(int i = hmc.ID + 1; i < heroList.Count; i++)
        {
            heroList[i].ID--;
        }
        heroLength--;
        heroList.RemoveAt(hmc.ID);
    }

    public void GetHpItem(int _hp)
    {
        foreach(HeroMoveController i in heroList)
        {
            i.GetComponentInChildren<HeroComponent>().AddHp(_hp);
        }
    }

}





