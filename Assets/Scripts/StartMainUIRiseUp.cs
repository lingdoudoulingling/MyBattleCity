using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMainUIRiseUp : MonoBehaviour {

    public GameObject screenCenter;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
            transform.position = Vector3.MoveTowards(transform.position, screenCenter.transform.position, 200 * Time.deltaTime);

    }
}
