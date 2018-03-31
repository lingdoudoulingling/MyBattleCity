using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoseMode : MonoBehaviour {

    private int mode = 1;
    public Transform pos_1;
    public Transform pos_2;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.W))
        {
            mode = 1;
            transform.position = pos_1.position;
        }else if (Input.GetKeyDown(KeyCode.S))
        {
            mode = 2;
            transform.position = pos_2.position;
        }
        if (mode == 1 && Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("Game");
        }
    }
}
