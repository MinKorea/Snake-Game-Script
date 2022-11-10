using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}

public struct PosDir
{
    public Vector2 pos;
    public Direction dir;
}

public class HeroMoveController : MonoBehaviour
{
    int idx;    // ����ΰ� �� ��°�� ��ġ�� �ִ��� �Ǵ��� ����
    public int ID { get { return idx; } set { idx = value; } }

    [SerializeField]
    protected float speed = 1;    // ���� ���ǵ�
    [SerializeField]
    float minSpeed = 2.5f;
    [SerializeField]
    float maxSpeed = 3.5f;
    [SerializeField]
    float normalSpeed = 3;
    
    Rigidbody2D rb;
    protected Animator anit;

    Direction dir = Direction.DOWN; // ���� ����� �̵�����
    public Direction DIR { get { return dir; } }

    List<PosDir> posDirs = new List<PosDir>();

    bool isTurn = false;    // ���� ����Ʈ�ȿ� ��ġ�� ���Ⱚ�� ����Ǿ� �ִ��� �Ǵ��� ����
    bool isHead = false;
    public bool HEAD { get { return isHead; } set { isHead = value; } }
    bool isArrive = false;

    SpriteRenderer img;
    SpriteRenderer shadow;

    GameObject hitChecker;


    // Start is called before the first frame update
    void Start()
    {
        img = transform.GetChild(0).GetComponent<SpriteRenderer>();
        shadow = transform.GetChild(1).GetComponent<SpriteRenderer>();

        if(transform.childCount == 3)
        {
            hitChecker = transform.GetChild(2).gameObject;
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        MoveSpeedChange();
        MoveDestination();
        SetOrderLayer();
        
    }

    void MoveDestination()
    {
        if(posDirs.Count != 0)
        {
            isTurn = true;

            if(dir == Direction.UP)
            {
                if(transform.position.y >= posDirs[0].pos.y)    // ���� ������� y��� ����� ��ġ�� y���� ���ؼ� ������� ��ġ�� ����� ��ġ�� ���ų� �� ���ٸ�
                {
                    isArrive = true;
                }
            }
            else if(dir == Direction.DOWN)
            {
                if(transform.position.y <= posDirs[0].pos.y)    
                {
                    isArrive = true;
                }
            }
            else if (dir == Direction.LEFT)
            {
                if (transform.position.x <= posDirs[0].pos.x)    
                {
                    isArrive = true;
                }
            }
            else if (dir == Direction.RIGHT)
            {
                if (transform.position.x >= posDirs[0].pos.x)    
                {
                    isArrive = true;
                }
            }

            if (isArrive)
            {
                isArrive = false;
                transform.position = posDirs[0].pos;        // ��ġ �缳��
                Move(posDirs[0].dir);                       // ���� ���� �� ���⿡ ���� �̵�
                posDirs.RemoveAt(0);                        // ����Ʈ �ȿ��� ��ġ ����
            }

        }
        else
        {
            if (isTurn)
            {
                isTurn = false;
            }
        }
    }

    public void Move(Direction _dir)  // �̵� �Լ�
    {
        if(rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        if(anit == null)
        {
            anit = GetComponentInChildren<Animator>();
        }
        
        dir = _dir;

        Vector3 rot = Vector3.zero;
       

        if (dir == Direction.UP)
        {
            rb.velocity = Vector2.up * speed;
            rot = new Vector3(0, 0, 180);
        }
        else if (dir == Direction.DOWN)
        {
            rb.velocity = Vector2.down * speed;
            rot = new Vector3(0, 0, 0);
        }
        else if (dir == Direction.LEFT)
        {
            rb.velocity = Vector2.left * speed;
            rot = new Vector3(0, 0, 270);
        }
        else if (dir == Direction.RIGHT)
        {
            rb.velocity = Vector2.right * speed;
            rot = new Vector3(0, 0, 90);
        }

        if(hitChecker != null)
        {
            hitChecker.transform.localEulerAngles = rot;
        }


        SetAnim();

    }

    void MoveSpeedChange()
    {
        if(tag != "Monster")
        {
            if (!isHead)
            {
                /*if (isTurn)
                {
                    return;
                }*/

                if (Vector2.Distance(HeroManager.hm.LIST[idx - 1].transform.position, transform.position) > 1.51f)
                {
                    if (speed != maxSpeed)
                    {
                        speed = maxSpeed;
                        SetSpeed();
                    }
                }
                /*else if(Vector2.Distance(HeroManager.hm.LIST[idx - 1].transform.position, transform.position) < 1.49f)
                {
                    if (speed != minSpeed)
                    {
                        speed = minSpeed;
                        SetSpeed();
                    }
                }*/
                else
                {
                    if (speed != normalSpeed)
                    {
                        speed = normalSpeed;
                        SetSpeed();
                    }
                }

            }
        }
        else
        {
            if (!isHead)
            {
                /*if (isTurn)
                {
                    return;
                }*/

                MonsterControl mc = GetComponentInParent<MonsterControl>();
                if (mc.GetHeadMonster().ICE) return;

                if (Vector2.Distance(mc.LIST[idx - 1].transform.position, transform.position) > 1.51f)
                {
                    if (speed != maxSpeed)
                    {
                        speed = maxSpeed;
                        SetSpeed();
                    }
                }
                /*else if(Vector2.Distance(mc.LIST[idx - 1].transform.position, transform.position) < 1.49f)
                {
                    if (speed != minSpeed)
                    {
                        speed = minSpeed;
                        SetSpeed();
                    }
                }*/
                else
                {
                    if(speed != normalSpeed)
                    {
                        speed = normalSpeed;
                        SetSpeed();
                    }
                }
            }
        }
        
    }

    public void SetAnim()
    {
        if(dir == Direction.UP)
        {
            anit.SetTrigger("Up");
        }
        else if (dir == Direction.DOWN)
        {
            anit.SetTrigger("Down");
        }
        else if (dir == Direction.RIGHT)
        {
            anit.SetTrigger("Right");
        }
        else if (dir == Direction.LEFT)
        {
            anit.SetTrigger("Left");
        }
    }

    public void SetSpeed()
    {
        if (dir == Direction.UP)
        {
            rb.velocity = Vector2.up * speed;
        }
        else if (dir == Direction.DOWN)
        {
            rb.velocity = Vector2.down * speed;
        }
        else if (dir == Direction.RIGHT)
        {
            rb.velocity = Vector2.right * speed;
        }
        else if (dir == Direction.LEFT)
        {
            rb.velocity = Vector2.left * speed;
        }
    }



    public void AddPosDir(Vector2 _pos, Direction _dir)
    {
        PosDir temp;

        temp.pos = _pos;
        temp.dir = _dir;

        posDirs.Add(temp);
    }

    void SetOrderLayer()
    {
        if(dir == Direction.UP || dir == Direction.DOWN)
        {
            img.sortingOrder = -1 * Mathf.RoundToInt(transform.position.y);          //���� y�� ���� ���� ����� ���������� ��ȯ
            
            shadow.sortingOrder = -1 * Mathf.RoundToInt(transform.position.y);

        }
    }

    public List<PosDir> GetPosDir()
    {
        return posDirs;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Hero"))
        {
            if(collision.GetComponent<HeroMoveController>().HEAD)
            {
                if(gameObject.CompareTag("Hero"))               
                    GetComponentInChildren<HeroComponent>().Die();
            }
        }
    }

}
