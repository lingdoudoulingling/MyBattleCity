using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Born : MonoBehaviour {

    public GameObject playerTank;
    public GameObject[] enemyTankList;
    public bool isPlayer;
	// Use this for initialization
	void Start () {
        Invoke("BornTank",1.2f);
        Destroy(gameObject, 1.2f);
	}
	
    private void BornTank()
    {
        GameObject currentTank;
        if (isPlayer)
        {
            currentTank = Instantiate(playerTank, transform.position, transform.rotation);
        }
        else
        {
            //[0,3)只有整形
            int num = Random.Range(0, 3);    
            currentTank = Instantiate(enemyTankList[num], transform.position, transform.rotation);
            GameManager.Instance.enemies.Add(currentTank);
        }

        currentTank.transform.SetParent(MapIncubator.Instance.gameObject.transform);

    }
}
