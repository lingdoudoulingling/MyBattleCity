using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    //数字1，2看不顺眼，在依据原有规则的名字上在其中的数字后加一道下划线。
    //属性值
    public int player1_Score = 0;
    //是否死亡
    public bool isDied;
    //是否失败
    public bool isDefeated;
    //完成地图加载
    [HideInInspector]
    public bool completedMapLoading;
    //生命数
    private int player1_LifeNum = 3;
   
    //引用
    public Text uiPlayer1_LifeNum;
    public Text uiPlayer1_Score;
    //关卡数
    public Text uiStageNum;
    //敌人数
    public Text uiEnemyNum;

    public GameObject gameOverAnim;
    //退出对话框
    public GameObject quitDialog;
    //下一关提示
    public GameObject nextStagePrompt;
    //输入名字对话框
    public GameObject enterNameDialog;
    //记录所有敌人
    public List<GameObject> enemies;
    //
    public GameObject mapIncubator;
    //单例(设计模式)
    private static GameManager instance;

    
    public static GameManager Instance
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

    public int Player1_LifeNum
    {
        get
        {
            return player1_LifeNum;
        }

        set
        {
            player1_LifeNum = value;
        }
    }

    private void Awake()
    {
        instance = this;
        
    }
   
    // Update is called once per frame
    void Update () {
        if (Input.GetKey(KeyCode.Escape))
        {
            quitDialog.SetActive(true);
            //暂停游戏
            Time.timeScale = 0;
            //暂停声音
            AudioListener.pause = true;
        }
        if (isDefeated)
        {
            if (Equals(gameOverAnim.transform.position, transform.position))
            {
                Invoke("ReturnToMainMenu", 1.5f);
                return;
            }
            
            gameOverAnim.transform.position = Vector3.MoveTowards(gameOverAnim.transform.position, transform.position, 7 * Time.deltaTime);
        }
        if (!isDefeated && isDied) 
        {
            Resurrection();
        }
        uiPlayer1_LifeNum.text = Player1_LifeNum.ToString();
        uiPlayer1_Score.text = player1_Score.ToString();
        //判定胜利，进入下一关
        if (completedMapLoading && uiEnemyNum.text == "0" && enemies.Count == 0)  
        {
            Invoke("EnterNextStage", 1);
            completedMapLoading = false;
        }
    }
    //玩家复活
    private void Resurrection()
    {

        if (--Player1_LifeNum > 0)  
        {
            //复活玩家1
            MapIncubator.Instance.CreateTank(new Vector2(-1.5f, -8), true);
            isDied = false;
        }
        else
        {
            //游戏失败，返回主界面
            isDefeated = true;
        }
    }
    private void ReturnToMainMenu()
    {
        //暂停游戏
        Time.timeScale = 0;
        //暂停声音
        AudioListener.pause = true;
        enterNameDialog.SetActive(true);
    }
   private void EnterNextStage()
    {
        //出现提示
        nextStagePrompt.SetActive(true);
        Invoke("CloseNextStagePrompt", 3f);
        //关卡数增加
        uiStageNum.text = (int.Parse(uiStageNum.text) + 1).ToString();
        //生命数恢复
        Player1_LifeNum = 3;
        //敌人数恢复
        uiEnemyNum.text = "27";
        //重新加载地图
        Destroy(MapIncubator.Instance.gameObject);
        Instantiate(mapIncubator, Vector2.zero, Quaternion.identity);
}
private void CloseNextStagePrompt()
    {
        //关闭提示
        nextStagePrompt.SetActive(false);
    }
}
