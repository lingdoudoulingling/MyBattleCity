using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;




public class Quit : MonoBehaviour {

    public GameObject quitDialog;

    public void OnYesButtonClick()
    {
        SceneManager.LoadScene("Start");
        Resume();
    }
    public void OnNoButtonClick()
    {
        quitDialog.SetActive(false);
        Resume();
    }
    private void Resume()
    {
        //恢复游戏
        Time.timeScale = 1;
        //恢复声音
        AudioListener.pause = false;
    }
}
