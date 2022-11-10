using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterControl : MonoBehaviour
{
    List<MonsterMoveController> monsterList = new List<MonsterMoveController>();    // ���͵��� ��Ƶ� ����Ʈ
    public List<MonsterMoveController> LIST { get { return monsterList; } }


    MonsterMoveController headMonster;

    bool isTurn = false;
    float turnTime = 0;

    [SerializeField]
    float minLimitX = 0;
    [SerializeField]
    float maxLimitX = 0;
    [SerializeField]
    float minLimitY = 0;
    [SerializeField]
    float maxLimitY = 0;

    Vector3 pos;    // ��� ������ ���� ��ġ�� ��Ƶ� ����

    // Start is called before the first frame update
    void Start()
    {
        turnTime = Random.Range(3, 6);
    }

    // Update is called once per frame
    void Update()
    {
        if (headMonster.ICE) return;  

        if (!isTurn)
        {
            turnTime -= Time.deltaTime;

            if(turnTime <= 0)
            {
                isTurn = true;
                RandomDirection();
            }
            MoveLimit();
        }
    }

    void MoveLimit()
    {
        if(headMonster != null)
            pos = headMonster.transform.position;

        if(headMonster.DIR == Direction.UP)
        {
            if (pos.y >= maxLimitY)
            {
                isTurn = true;

                int num = Random.Range(0, 2);

                if(num == 0)
                {
                    ChangeMoveDirection(Direction.LEFT);
                }
                else
                {
                    ChangeMoveDirection(Direction.RIGHT);
                }

                turnTime = Random.Range(3, 6);
                isTurn = false;
            }
        }
        else if(headMonster.DIR == Direction.DOWN)
        {
            if(pos.y <= minLimitY)
            {
                isTurn = true;

                int num = Random.Range(0, 2);

                if (num == 0)
                {
                    ChangeMoveDirection(Direction.LEFT);
                }
                else
                {
                    ChangeMoveDirection(Direction.RIGHT);
                }

                turnTime = Random.Range(3, 6);
                isTurn = false;
            }
        }
        else if(headMonster.DIR == Direction.LEFT)
        {
            if(pos.x <= minLimitX)
            {
                isTurn = true;

                int num = Random.Range(0, 2);

                if (num == 0)
                {
                    ChangeMoveDirection(Direction.UP);
                }
                else
                {
                    ChangeMoveDirection(Direction.DOWN);
                }

                turnTime = Random.Range(3, 6);
                isTurn = false;
            }    
        }
        else 
        {
            if (pos.x >= maxLimitX)
            {
                isTurn = true;

                int num = Random.Range(0, 2);

                if (num == 0)
                {
                    ChangeMoveDirection(Direction.UP);
                }
                else
                {
                    ChangeMoveDirection(Direction.DOWN);
                }

                turnTime = Random.Range(3, 6);
                isTurn = false;
            }
        }
    }

    void RandomDirection()
    {
        Direction dir;

        while (true)
        {
            dir = (Direction)Random.Range(0, 4);

            if(dir == Direction.UP)
            {
                if (headMonster.DIR == Direction.UP || headMonster.DIR == Direction.DOWN || headMonster.transform.position.y >= maxLimitY - 0.5f)
                {
                    continue;
                }
                else break;
            }
            else if (dir == Direction.DOWN)
            {
                if (headMonster.DIR == Direction.UP || headMonster.DIR == Direction.DOWN || headMonster.transform.position.y <= minLimitY + 0.5f)
                {
                    continue;
                }
                else break;
            }
            else if (dir == Direction.LEFT)
            {
                if (headMonster.DIR == Direction.LEFT || headMonster.DIR == Direction.RIGHT || headMonster.transform.position.x <= minLimitX + 0.5f)
                {
                    continue;
                }
                else break;
            }
            else 
            {
                if (headMonster.DIR == Direction.LEFT || headMonster.DIR == Direction.RIGHT || headMonster.transform.position.x >= maxLimitX - 0.5f)
                {
                    continue;
                }
                else break;
            }
        }

        ChangeMoveDirection(dir);

        turnTime = Random.Range(3, 6);
        isTurn = false;

    }


    void ChangeMoveDirection(Direction _dir)
    {
        if (headMonster.ICE) return;

        headMonster.Move(_dir);

        if(monsterList.Count > 1)
        {
            for(int i = 0; i < monsterList.Count; i++)
            {
                monsterList[i].AddPosDir(headMonster.transform.position, _dir);
            }
        }
    }

    public void SetHeadMonster(MonsterMoveController mmc)
    {
        headMonster = mmc;  
    }

    public void AddMonster(MonsterMoveController mmc)
    {
        monsterList.Add(mmc);
    }

    public MonsterMoveController GetHeadMonster()
    {
        return headMonster;
    }

    public MonsterMoveController GetList(int idx)
    {
        return monsterList[idx];
    }

    public void RemoveMonster(MonsterMoveController emc)
    {
        if(emc.ID == 0)
        {
            for(int i = 1; i < monsterList.Count; i++)
            {
                monsterList[i].ID--;        // ���� ���͵��� index�� �ϳ��� �ٿ���
            }
        }
        else if (emc.ID == 1)
        {
            for (int i = 2; i < monsterList.Count; i++)
            {
                monsterList[i].ID--;
            }
        }
        if(monsterList.Count == 1)
        {
            ItemManager.im.SpawnItem(headMonster.transform.position);   // ������ ����
            Destroy(gameObject);    // ���Ͱ� �Ѹ������ ��Ʈ�ѱ��� ��� ����
        }
        else
        {
            if (emc.ID == 0)
            {
                headMonster = monsterList[emc.ID + 1];
                headMonster.HEAD = true;
            }
            monsterList.RemoveAt(emc.ID);   // ���� �Ѹ����� ����
            
           
        }
    }

    public void MonsterIce()
    {
        foreach(MonsterMoveController i in monsterList)
        {
            i.Icing();
        }
    }

}
