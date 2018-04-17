using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoseMode : MonoBehaviour {

    private int mode = 0;
    private Transform[] arryMode; 
    public Transform pos_1;
    public Transform pos_2;
    public Transform pos_3;

    public int Mode
    {
        get
        {
            return mode;
        }

        set
        {
            if (value < 3 && value >= 0) 
            {
                mode = value;
            }
        }
    }

    private void Start()
    {
        arryMode = new Transform[3];
        arryMode[0] = pos_1;
        arryMode[1] = pos_2;
        arryMode[2] = pos_3;
    }
    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.W))
        {
            Mode--;

        }else if (Input.GetKeyDown(KeyCode.S))
        {
            Mode++;
        }

        transform.position = arryMode[Mode].position;

        if (Mode == 0 && Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("Game");
        }

        if (Mode == 2 && Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("Rankings");
        }
    }
}
