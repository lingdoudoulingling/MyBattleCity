using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour {
    //属性值
    private float h;
    private float v;
    //生成子弹时的旋转角度
    private Vector3 bullectEulerAngles;
    //移动速度
    public float moveSpeed = 3;
    //健壮度
    private int hp;
    //攻击计时器
    private float attackTimeVal;
    //攻击cd
    public float attackTime = 0.8f;
    //防护罩持续时间与状态
    public float defendTime = 5;
    [HideInInspector]
    public bool isDefended = true;//bool的默认值是false
    private Sprite[] currTankSprite = new Sprite[4];
    //引用
    public Sprite[] tankSprite;//上 右 下 左
    public GameObject bullet;
    private GameObject currentBullet;
    public GameObject explosion;
    public GameObject shield;
    private SpriteRenderer sr;
    //坦克的初始属性，星级变化时使用
    private Vector3 originalScale;
    private float originalAttackTime;
    private float originalMoveSpeed;
    //控制音效的播放
    private AudioSource moveAudioController;
    //拿到音效的资源
    public AudioClip[] tankAudio;

    public int Hp
    {
        get
        {
            return hp;
        }

        set
        {
            if (value < 4 && value >= 0)
            {
                hp = value;
                SetCurrentTankSprite();
                SetCurrentTankProperties();
            }
        }
    }

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        moveAudioController = GetComponent<AudioSource>();
        //保留初始属性
        originalScale = transform.localScale;
        originalAttackTime = attackTime;
        originalMoveSpeed = moveSpeed;
        //初始化坦克形态,hp初始为0
        SetCurrentTankSprite();
    }

    // Update is called once per frame
    void Update()
    {
        //保护状态
        if (isDefended)
        {
            defendTime -= Time.deltaTime;
            if (defendTime <= 0)
            {
                isDefended = false;
                shield.SetActive(false);
            }
        }
        //音效播放
        TankMoveAudioPlay();
    }
    //设置坦克形态，随Hp的改变而调用
    private void SetCurrentTankSprite()
    {
        for (int i = 0; i < 4; i++)
        {
            if (sr.sprite == currTankSprite[i]) 
            {
                sr.sprite = tankSprite[i + Hp * 4];
            }
            currTankSprite[i] = tankSprite[i + Hp * 4];
        }
        
        //升级变大,降级变小
        transform.localScale = new Vector3(0.1f * Hp, 0.1f * Hp, 0f) + originalScale;
    }
    //设置坦克属性，随Hp的改变而调用，移动速度0.3f递增，攻击间隔0.15f递减
    private void SetCurrentTankProperties()
    {
        moveSpeed = 0.3f * Hp + originalMoveSpeed;
        attackTime = -0.15f * Hp + originalAttackTime;
    }
    private void TankMoveAudioPlay()
    {
        if (h == 0 && v == 0)
        {
            moveAudioController.clip = tankAudio[0];
        }
        else
        {
            moveAudioController.clip = tankAudio[1];
        }
        if (!moveAudioController.isPlaying)
        {
            moveAudioController.Play();
        }
    }
    private void FixedUpdate()
    {
        if (GameManager.Instance.isDefeated)
        {
            return;
        }
        //移动
        Move();
        //攻击CD
        if (attackTimeVal > attackTime)
        {
            //攻击
            Attack();
        }
        else
        {
            attackTimeVal += Time.fixedDeltaTime;
        }
    }
    //坦克的移动方法
    private void Move()
    {
        //h返回{-1，1},接收‘a’,‘d’或者小键盘左右。上下移动类同
        if(Input.GetKey(KeyCode.A))
        {
            h = -1;
                //Input.GetAxisRaw("Horizontal")
        }else if (Input.GetKey(KeyCode.D))
        {
            h = 1;
        }
        else
        {
            h = 0;
        }
        //此处fixedDeltaTime和deltaTime效果相同。
        transform.Translate(Vector3.right * h * moveSpeed * Time.fixedDeltaTime, Space.World);

        if (h < 0)
        {
            sr.sprite = currTankSprite[3];
            bullectEulerAngles = new Vector3(0, 0, 90);
        }
        else if (h > 0)
        {
            sr.sprite = currTankSprite[1];
            bullectEulerAngles = new Vector3(0, 0, -90);

        }
        else if (h == 0)
        {
            if (Input.GetKey(KeyCode.S))
            {
                v = -1;
                //Input.GetAxisRaw("Vertical")
            }
            else if (Input.GetKey(KeyCode.W))
            {
                v = 1;
            }
            else
            {
                v = 0;
            }

            transform.Translate(Vector3.up * v * moveSpeed * Time.fixedDeltaTime, Space.World);

            if (v < 0)
            {
                sr.sprite = currTankSprite[2];
                bullectEulerAngles = new Vector3(0, 0, 180);

            }
            else if (v > 0)
            {
                sr.sprite = currTankSprite[0];
                bullectEulerAngles = Vector3.zero;
            }
        }
    }
    //坦克的攻击方法
    private void Attack()
    {
        if (Input.GetKeyDown("space"))
        {
            //子弹产生的角度=坦克当前的角度+子弹应该旋转的角度
            currentBullet = Instantiate(bullet, transform.position, Quaternion.Euler(bullectEulerAngles));//留个变量，哪有变化在哪赋值
            if (Hp == 3)
            {
                currentBullet.GetComponent<Bullect>().isDamageBarriar = true;
            }
            attackTimeVal = 0;
        }
    }
    //坦克的死亡方法
    public void Hurt()
    {
        if (!isDefended)
        {
            if (Hp == 0)
            {
            //产生爆炸特效
                Instantiate(explosion, transform.position, Quaternion.identity);
            //死亡
                Destroy(gameObject);
                GameManager.Instance.isDied = true;
            }
            else
            {
                Hp--;
            }
        }
        
    }
    
}
