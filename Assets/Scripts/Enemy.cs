using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float moveSpeed;
    //敌军价值
    public int headValue;
    //移动方向
    private float h;
    private float v;
    //动作锁
    public bool actionLock;
    //动作锁时间
    private float actionLockTime = 15f;
    //动作锁计时器
    private float actionLockTimeVal;
    //坦克碰撞后是否改变方向
    private bool isGathered;

    //改变移动方向cd
    private float changeDirectionTime = 3f;
    //改变移动方向计时器,赋初值是为了在游戏刚开始时改变方向
    private float changeDirectionTimeVal = 3f;
    //攻击cd
    public float attackDelayTime = 2f;
    //攻击计时器
    private float attackTimeVal;
    //是否是重型坦克
    public bool isHeavy; 

    private Vector3 bullectEulerAngles;
    //引用
    public Sprite[] tankSprite;//上 右 下 左
    public GameObject bullet;
    public GameObject explosion;
    private SpriteRenderer sr;
    private EnemyHPManager hpManager;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        hpManager = GetComponent<EnemyHPManager>();
    }
    //锁住行动
    public void LockAction()
    {
        actionLock = true;
        actionLockTimeVal = actionLockTime;
    }
    private void FixedUpdate()
    {
        if (actionLock)
        {
            actionLockTimeVal -= Time.fixedDeltaTime;
            if (actionLockTimeVal <= 0) 
            {
                actionLock = false;
            }
        }
        else
        {
            Move();
            //攻击CD
            if (attackTimeVal > attackDelayTime)
            {
                Attack();
            }
            else
            {
                attackTimeVal += Time.fixedDeltaTime;
            }
        }
        
    }
    //坦克的移动方法
    private void Move()
    {
        
        if (!isGathered)
        {
            RandomChoseMoveDirection();
        }
        else
        {
            h = -h;
            v = 0;
            isGathered = false;
        }

        //此处fixedDeltaTime和deltaTime效果相同。
        transform.Translate(Vector3.right * h * moveSpeed * Time.fixedDeltaTime, Space.World);

        if (h < 0)
        {
            sr.sprite = tankSprite[3];
            bullectEulerAngles = new Vector3(0, 0, 90);
        }
        else if (h > 0)
        {
            sr.sprite = tankSprite[1];
            bullectEulerAngles = new Vector3(0, 0, -90);

        }
        else if (h == 0)
        {
            transform.Translate(Vector3.up * v * moveSpeed * Time.fixedDeltaTime, Space.World);

            if (v < 0)
            {
                sr.sprite = tankSprite[2];
                bullectEulerAngles = new Vector3(0, 0, 180);

            }
            else if (v > 0)
            {
                sr.sprite = tankSprite[0];
                bullectEulerAngles = Vector3.zero;
            }
        }
    }
    private void RandomChoseMoveDirection()
    {
        if (changeDirectionTimeVal >= changeDirectionTime)
        {
            int num = Random.Range(0, 8);
            switch (num)
            {
                case 0: v = 1; h = 0; break;
                case 1:
                case 2: v = 0; h = 1; break;
                case 3:
                case 4: v = 0; h = -1; break;
                default: v = -1; h = 0; break;
            }
            changeDirectionTimeVal = 0;
        }
        else
        {
            changeDirectionTimeVal += Time.fixedDeltaTime;
        }
    }
    //坦克的攻击方法
    private void Attack()
    {
        
            //子弹产生的角度=坦克当前的角度+子弹应该旋转的角度
            Instantiate(bullet, transform.position, Quaternion.Euler(bullectEulerAngles));//留个变量，哪有变化在哪赋值
            attackTimeVal = 0;
    }
    //坦克的死亡方法
    public void Die()
    {
        if (isHeavy)
        {
            if (--hpManager.Hp == 0)
            {
                DestroyThis();
            }
        }
        else
        {
            DestroyThis();
        }
    }
    private void DestroyThis()
    {
        GameManager.Instance.enemies.Remove(gameObject);
        //加分
        GameManager.Instance.player1_Score += headValue;
        //产生爆炸特效
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    //坦克相聚的时候会散开
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemy"|| collision.gameObject.tag == "tank")
        {
            changeDirectionTimeVal = 3.0f;
            isGathered = true;
        }
    }
    
}
