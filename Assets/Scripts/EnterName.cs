using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;




public class EnterName : MonoBehaviour {
    
    public GameObject enterNameDialog;
    public InputField inputField;
    public GameObject currPlayerScore;
    private string playerListJson;

    private void Start()
    {
        inputField.ActivateInputField();
    }
    private int CompareByScore(PlayerInfo a,PlayerInfo b)
    {
        if (a.score <= b.score)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }
    public void OnOkButtonClick()
    {
        PlayerInfo currPlayer = new PlayerInfo();
        currPlayer.name = inputField.text;
        currPlayer.score = int.Parse(currPlayerScore.GetComponent<Text>().text);

        if (File.Exists(Application.dataPath + "/rankinglist.json"))
        {
            //如果存在这个文件，将所有数据读出
            StreamReader sr = new StreamReader(Application.dataPath + "/rankinglist.json");
            if (sr == null)
            {
                return;
            }
            playerListJson = sr.ReadToEnd();
            sr.Close();
            PlayerList currPlayerList = JsonUtility.FromJson<PlayerList>(playerListJson);
            //将新数据加入原有数据，排序
            List<PlayerInfo> processedList = new List<PlayerInfo>(currPlayerList.playerList);
            processedList.Add(currPlayer);
            processedList.Sort(CompareByScore);
            while (processedList.Count > 10)
            {
                processedList.RemoveAt(processedList.Count - 1);
            }
            currPlayerList.playerList = processedList.ToArray();
            //将排序后的数据写入文件
            playerListJson = JsonUtility.ToJson(currPlayerList);
            StreamWriter sw = new StreamWriter(Application.dataPath + "/rankinglist.json", false);
            sw.Write(playerListJson);
            sw.Close();
        }
        else
        {
            //如果没有这个文件，创建一个文件并写入第一条数据
            
                StreamWriter rl = File.CreateText(Application.dataPath + "/rankinglist.json");
                PlayerList cfmdPlayerList = new PlayerList();
                cfmdPlayerList.playerList = new PlayerInfo[] { currPlayer };
                playerListJson = JsonUtility.ToJson(cfmdPlayerList);
                rl.Write(playerListJson);
                rl.Close();
            
        }

        SceneManager.LoadScene("Start");
        Resume();
    }
    public void OnNoButtonClick()
    {
        SceneManager.LoadScene("Start");
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

[Serializable]
public class PlayerInfo
{
    public string name;
    public int score;
}
[Serializable]
public class PlayerList
{
    public PlayerInfo[] playerList;
}
