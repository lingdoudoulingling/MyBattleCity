using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapIncubator : MonoBehaviour {

    /*初始化地图所需物体
     0：老家     1：墙     2：障碍     3：出生特效     4：河流    5：草     6：边界      7.奖励
     */
    public GameObject[] item;
    public Vector2[] campWallPos;//从左至右
    public List<GameObject> campWall;
    public Vector2[] enemyBornPos;

    //基地围墙强化时间计时器
    [HideInInspector]
    public float reinforceCampTimeVal;
    //是否强化基地围墙
    [HideInInspector]
    public bool isCampReinforced;
    //基地围墙强化时间
    public float reinforceCampTime = 20;
    //敌人数,每关30个，打完进入下一关
    private Text uiEnemyNum;
    [HideInInspector]
    private static MapIncubator instance;

    //保存所有已生成的地形的位置
    /// <summary>
    /// 如果觉得现有的随机生成太乱的话，可以将某种地形的可能位置写死在一个List里。
    /// </summary>
    private List<Vector2> itemPosList = new List<Vector2>();

    public static MapIncubator Instance
    {
        get
        {
            return instance;
        }

        set
        {
            instance = value;
        }
    }

    //先实例化其他物体最后随机实例化地形
    private void Awake()
    {
        GameObject enemyNumGameObj = GameObject.Find("Canvas/Enemy Sign/Enemy Num");
        uiEnemyNum = enemyNumGameObj.GetComponent<Text>();
        //实例化静态对象
        instance = this;
        //创建随机地图
        CreateRandomMap();
       
    }
  
    private void CreateRandomMap()
    {
        //实例化老家
        CreateItem(item[0], new Vector2(0, -8), Quaternion.identity);
        //实例化老家围墙
        CreateCampWall(item[1]);
        //使老家围墙内的范围不生成地形，以免挡住老家
        ExcludedRange(new Vector2(-1.0f, -8.5f), new Vector2(1.0f, -7.0f));
        //实例化边界
        CreateItem(item[6], new Vector2(0, 10), Quaternion.identity);
        //实例化玩家1,玩家2（1.5f，-8）
        CreateTank(new Vector2(-1.5f, -8), true);
        //实例化敌人,先一次生成8个
        for (int i = 0; i < enemyBornPos.Length; i++)
        {
            CreateTank(enemyBornPos[i], false);
            uiEnemyNum.text = (int.Parse(uiEnemyNum.text) - 1).ToString();
        }
        //每5秒调用一次
        StartCoroutine(CreateEnemyCoroutine());
        //实例化地形种类{1,2,4,5}，每种创建的数量
        CreateTerrain(new int[] { 1, 2, 4, 5 }, new int[] { 250, 20, 40, 120 });
        //实例化奖励
        InvokeRepeating(methodName: "CreateBonus", time: 4, repeatRate: 17);

    }
    IEnumerator CreateEnemyCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(5);
            CreateRandomEnemy();
            //通知GameManager完成地图加载
            GameManager.Instance.completedMapLoading = true;

            if (int.Parse(uiEnemyNum.text) <= 0)
            {
                break;
            }
        }
    }
    private void CreateBonus()
    {
        Instantiate(item[7], new Vector2(Random.Range(-10f, 10f), Random.Range(-8f, 8f)), Quaternion.identity).transform.SetParent(transform);
        
    }
    private void CreateRandomEnemy()
    {
        CreateTank(enemyBornPos[Random.Range(0, enemyBornPos.Length)], false);
        uiEnemyNum.text = (int.Parse(uiEnemyNum.text) - 1).ToString();
    }
    public void CreateTank(Vector2 pos,bool isPlayer)
    {
        GameObject currentBornEffect = Instantiate(item[3], pos, Quaternion.identity);
        currentBornEffect.GetComponent<Born>().isPlayer = isPlayer;
        //将Born周边的位置设为禁止生成地形，player不成为Map的子物体
        //设（a，b）->[a-0.5,a+0.5]&[b-0.5,b+0.5]禁止生成物体
        ExcludedRange(new Vector2(pos.x - 0.5f, pos.y - 0.5f), new Vector2(pos.x + 0.5f, pos.y + 0.5f));
    }
    //生成地形时将一个范围排除,参数为闭区间。
    private void ExcludedRange(Vector2 lowerLeft, Vector2 upperRight)
    {
        float tX = lowerLeft.x, tY = lowerLeft.y;
        while ( tX <= upperRight.x ) 
        {
            tY = lowerLeft.y;
            while (tY <= upperRight.y) 
            {
                Vector2 temp = new Vector2(tX, tY);
                if (!itemPosList.Contains(temp)) {
                    itemPosList.Add(temp);
                }
                tY += 0.5f;
            }
            tX += 0.5f;
        }
    }
    //生成地形
    private void CreateTerrain(int[] chosed, int[] num)
    {
        int j;
        for (int i = 0; i < chosed.Length; i++)
        {
            j = num[i];
            while (j-- > 0)
            {
                CreateItem(item[chosed[i]], CreateRandomPosition(), Quaternion.identity);
            }
            j = num[i];
        }
    }
    //生成老家围墙
    private void CreateCampWall(GameObject item)
    {
        for (int i = 0; i < campWallPos.Length; i++)
        {
            GameObject currentItem = CreateItem(item, campWallPos[i], Quaternion.identity);
            campWall.Add(currentItem);
        }
    }
    //生成项目。生成的项目会成为Map的子物体，并且其位置加入位置列表。
    private GameObject CreateItem(GameObject createObject, Vector2 objectPosition, Quaternion objectRotation)
    {
        GameObject currentItem = Instantiate(createObject, objectPosition, objectRotation);
        currentItem.transform.SetParent(transform);
        itemPosList.Add(objectPosition);
        return currentItem;
    }
    //产生随机位置
    /// <summary>
    /// 可选：要留出通路保证钢板不会分割地图（地图两边不通）。
    /// </summary>
    private Vector2 CreateRandomPosition()
    {
        while (true)
        {
            //通过调整随机数的范围来调整地形的范围，使用的是int型的Range,还有float型的Range。怎样生成地图是个大课题
            Vector2 randomPosition = new Vector2(Random.Range(-10.5f, 10.5f), Random.Range(-8.5f, 8.5f));
            Vector2 fixedPos = FixMapItemPos(randomPosition);
            if (!HasThePos(fixedPos))
            {
                return fixedPos;
            }
        }
    }
    //将随机位置格式化为以0.5为基本单元的位置
    private Vector2 FixMapItemPos(Vector2 Pos)
    {   
        return new Vector2(FixNumTo0_5(Pos.x), FixNumTo0_5(Pos.y));
    }
    private float FixNumTo0_5(float x){
        //6.77
        x *= 10;
        //67.7
        x = Mathf.Round(x);
        //68
        bool symbol = false;
        if (x > 0)
        {
            symbol = true;
        }
        int IntegralPart = (int)Mathf.Abs(x / 10);//6
        int DecimalPart = (int)Mathf.Abs(x % 10);//8
        switch (DecimalPart)
        {
            case 0:
            case 1:
            case 2: DecimalPart = 0; break;
            case 3:
            case 4:
            case 5:
            case 6: DecimalPart = 5; break;
            case 7:
            case 8:
            case 9: IntegralPart++; DecimalPart = 0; break; 
        }
        x = IntegralPart + (float)DecimalPart/ 10;
        if (symbol)
        {
            return x;
        }
        else
        {
            return -x;
        }
        }
    //生成地行时，判断位置列表中是否有这个位置
    private bool HasThePos(Vector2 createPos)
    {
        return itemPosList.Contains(createPos);
    }
    private void ChangeCampWallTo(GameObject newWall)
    {
        //销毁原有围墙
        foreach (GameObject wall in campWall)
        {
            if (wall != null)
            {
                Destroy(wall);
            }
        }
        campWall.Clear();
        //生成新围墙
        CreateCampWall(newWall);
    }
    private void Update()
    {
        if (isCampReinforced)
        {
            reinforceCampTimeVal -= Time.deltaTime;
            if (reinforceCampTimeVal <= 0)
            {
                ChangeCampWallTo(item[1]);
                isCampReinforced = false;
            }
        }

    }
}
