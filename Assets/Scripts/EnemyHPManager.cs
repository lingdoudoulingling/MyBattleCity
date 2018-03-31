using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHPManager : MonoBehaviour {

    private int hp = 3;
    public Sprite[] sprite;
    //引用
    private Sprite[] currTankSprite;
    private Enemy enemyScript;
    private SpriteRenderer sr;

    private void Awake()
    {
        enemyScript = GetComponent<Enemy>();
        sr = GetComponent<SpriteRenderer>();
        currTankSprite = enemyScript.tankSprite;
    }

    public int Hp
    {
        get
        {
            return hp;
        }

        set
        {
            hp = value;
            if (Hp > 0)
            {
                SetTankSprite();
            }
        }
    }

    private void SetTankSprite()
    {
        for(int i = 0; i < 4; i++)
        {
            if (sr.sprite == currTankSprite[i])
            {
                sr.sprite = sprite[i + (2 - Hp) * 4];
            }
            currTankSprite[i] = sprite[i + (2 - Hp) * 4];
        }
    }
}
