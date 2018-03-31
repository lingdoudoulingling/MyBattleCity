using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullect : MonoBehaviour {
    public float moveSpeed = 10;
    public bool isPlayerBullet;
    public bool isDamageBarriar;
	
    // Update is called once per frame
	void Update () {
        transform.Translate(transform.up * moveSpeed * Time.deltaTime, Space.World);
    }
    private void Start()
    {
        Destroy(gameObject, 2.3f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "tank":
                if (!isPlayerBullet)
                {
                    collision.GetComponent<Player>().Hurt();
                    Destroy(gameObject);
                }
                break;
            case "barriar":
                if (isDamageBarriar)
                {
                    Destroy(collision.gameObject);
                }
                Destroy(gameObject);
                break;
            case "wall"://销毁墙，销毁自身。
                Destroy(collision.gameObject);
                Destroy(gameObject);
                break;
            case "enemy":
                if (isPlayerBullet)
                {
                    collision.GetComponent<Enemy>().Die();
                    Destroy(gameObject);
                }
                break;
            case "camp":
                collision.SendMessage("Die");
                Destroy(gameObject);
                break;
            default:
                break;
        }
    }
   
}
