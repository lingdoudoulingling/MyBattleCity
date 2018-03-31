using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //查看动画长度
        Destroy(gameObject, 0.5f);
	}
	
}
