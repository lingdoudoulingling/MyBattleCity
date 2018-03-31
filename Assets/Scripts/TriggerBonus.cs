using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBonus : MonoBehaviour {

    /*bonus
     0：increase life num（坦克）    1：turn on shield（头盔）     2：hp increase(星)     3：kill all enemies(手雷)     4：reinforce camp（铲子）    5：halt enemies（时钟）    
     */
    //脚本绑定的bonus是什么
    public Bonus bonus;
    public Sprite[] bonusSprite;
    //记录tag以区分玩家1或玩家2
    private string tagOfTank;
    //引用
    //玩家的Player脚本组件
    private Player playerSp;
    

    public enum Bonus
    {
        IncreaseLife,
        TurnOnShield,
        IncreaseHP,
        KillEnemies,
        ReinforceCamp,
        HaltEnemies
    }

    // Use this for initialization
    void Start () {
        Destroy(gameObject, 15);
        int i = Random.Range(0, 6);
        GetComponent<SpriteRenderer>().sprite = bonusSprite[i];
        switch (i)
        {
            case 0: bonus = Bonus.IncreaseLife; break;
            case 1: bonus = Bonus.TurnOnShield; break;
            case 2: bonus = Bonus.IncreaseHP; break;
            case 3: bonus = Bonus.KillEnemies; break;
            case 4: bonus = Bonus.ReinforceCamp; break;
            case 5: bonus = Bonus.HaltEnemies; break;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "tank")
        {
            
            tagOfTank = collision.tag;
            switch (bonus)
            {
                case Bonus.IncreaseLife: IncreaseLife();
                    Destroy(gameObject); break;
                case Bonus.TurnOnShield: TurnOnShield(collision.GetComponent<Player>());
                    Destroy(gameObject); break;
                case Bonus.IncreaseHP: IncreaseHP(collision.GetComponent<Player>());
                    Destroy(gameObject); break;
                case Bonus.KillEnemies: KillEnemies();
                    Destroy(gameObject); break;
                case Bonus.ReinforceCamp: ReinforceCamp();
                    Destroy(gameObject); break;
                case Bonus.HaltEnemies: HaltEnemies();
                    Destroy(gameObject); break;

            }
        }
    }
    //增加一条生命
    private void IncreaseLife()
    {
        if (tagOfTank == "tank")
        {
            GameManager.Instance.Player1_LifeNum++;

        }
    }
    //增加10s护盾
    private void TurnOnShield(Player currentPlayer)
    {
        currentPlayer.isDefended = true;
        currentPlayer.defendTime = 10;
        currentPlayer.shield.SetActive(true);
    }
    //增加强壮度
    private void IncreaseHP(Player currentPlayer)
    {
        currentPlayer.Hp ++;
    }
    //歼灭所有敌人
    private void KillEnemies()
    {
        List<GameObject> enemiesKilled = new List<GameObject>();
        foreach (GameObject thisEnemy in GameManager.Instance.enemies)
        {
            enemiesKilled.Add(thisEnemy);
        }
        foreach (GameObject thisEnemy in enemiesKilled)
        {
            if (thisEnemy != null)
            {
                thisEnemy.GetComponent<Enemy>().Die();
            }
            
        }
        
    }
    //加固基地20s
    private void ReinforceCamp()
    {
        MapIncubator.Instance.reinforceCampTimeVal = MapIncubator.Instance.reinforceCampTime;
        MapIncubator.Instance.isCampReinforced = true;
        MapIncubator.Instance.SendMessage("ChangeCampWallTo", MapIncubator.Instance.item[2]);
    }
    //定身敌人
    private void HaltEnemies()
    {
        foreach (GameObject thisEnemy in GameManager.Instance.enemies)
        {
            if (thisEnemy != null)
            {
                thisEnemy.GetComponent<Enemy>().LockAction();
            }

        }
    }
}
