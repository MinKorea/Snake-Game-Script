using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterSpawn : MonoBehaviour
{
    [SerializeField]
    float minX = 0;
    [SerializeField]
    float maxX = 0;
    [SerializeField]
    float minY = 0;
    [SerializeField]
    float maxY = 0;

    [SerializeField]
    float spawnRate = 5;    // ���� ���� ����
    float time = 0;

    [SerializeField]
    GameObject monsterGroupPrefab;
    [SerializeField]
    GameObject[] monsters;      // ������ ���͵� ��Ƶ� �迭

    [SerializeField]
    GameObject hpBar;
    Transform hpBarParents;     // ������ Hp�ٸ� ��Ƶ� ���� ������Ʈ

    [SerializeField]
    float offset = 1.5f;        // ���� ������ �� ���Ϳ��� ����



    // Start is called before the first frame update
    void Start()
    {
        hpBarParents = GameObject.Find("MonsterHpBars").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.gm.GameStart || GameManager.gm.GameOver) return;

        time += Time.deltaTime;

        if (time >= spawnRate) SpawnMonster();
    }

    void SpawnMonster()
    {
        Direction dir = (Direction)Random.Range(0,4);   // ���� ����

        Vector2 pos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));      // ��ġ ����

        GameObject group = Instantiate(monsterGroupPrefab); // ���� �׷� ����
        group.transform.SetParent(this.transform);       // ���� �׷� �θ� ����

        int monLength = Random.Range(1, 4);             // ������ ���� ���� ����

        for(int i = 0; i < monLength; i++)              // ���� ����             
        {
            GameObject _hpBar = Instantiate(hpBar);     // hpbar ����

            _hpBar.transform.SetParent(hpBarParents);   // hpbar �θ� ����

            GameObject mon = Instantiate(monsters[Random.Range(0, monsters.Length)]);   // ���� ����
            mon.transform.SetParent(group.transform);   // ���� �θ� ����
            mon.GetComponent<MonsterMoveController>().ID = i;       // ���� �ε��� ����
            mon.GetComponent<MonsterComponent>().hpBar = _hpBar.GetComponent<Slider>();   // ���Ϳ� hp�� ����
            mon.GetComponent<MonsterComponent>().SetHpbar();

            if (i == 0)
            {
                mon.transform.position = pos;   // ���� ��ġ ����
                mon.GetComponent<MonsterMoveController>().HEAD = true;
                group.GetComponent<MonsterControl>().SetHeadMonster(mon.GetComponent<MonsterMoveController>());
                group.GetComponent<MonsterControl>().GetHeadMonster().Move(dir);
            }
            else
            {
                MonsterMoveController preMonster = group.GetComponent<MonsterControl>().GetList(i - 1); // ������ ���� �տ� �ִ� ���͸� ������

                FollowDirection(preMonster, mon);   // ��ġ ����

                for(int j = 0; j < preMonster.GetPosDir().Count; j++)
                {
                    mon.GetComponent<MonsterMoveController>().AddPosDir(preMonster.GetPosDir()[j].pos, preMonster.GetPosDir()[j].dir);
                    // �� ���Ϳ� ����Ǿ� �ִ� ���� ���� �����͸� ������ ���� ����Ʈ�� �����

                }

                mon.GetComponent<MonsterMoveController>().Move(preMonster.DIR);
            }

            group.GetComponent<MonsterControl>().AddMonster(mon.GetComponent<MonsterMoveController>()); // �׷� ���� ����Ʈ�� �߰�



        }

        time = 0;   // �����ð� �ʱ�ȭ
    }

    void FollowDirection(MonsterMoveController preMon, GameObject mon)
    {
        Vector2 prePos = preMon.transform.position;

        if (preMon.DIR == Direction.UP) mon.transform.position = new Vector2(prePos.x, prePos.y - offset);
        else if(preMon.DIR == Direction.DOWN) mon.transform.position = new Vector2(prePos.x, prePos.y + offset);
        else if (preMon.DIR == Direction.LEFT) mon.transform.position = new Vector2(prePos.x + offset, prePos.y);
        else mon.transform.position = new Vector2(prePos.x - offset, prePos.y);

    }



}
