using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class ShowRankingList : MonoBehaviour {

    public GameObject listItem;
    public Vector3 itemPos;
    public Canvas parentCanvas;
    private string playerListJson;
    private PlayerList currPlayerList;
	// Use this for initialization
	void Start () {
        if (File.Exists(Application.dataPath + "/rankinglist.json")) 
        {
            StreamReader sr = new StreamReader(Application.dataPath + "/rankinglist.json");
            if (sr == null)
            {
                return;
            }
            playerListJson = sr.ReadToEnd();
            sr.Close();
            currPlayerList = JsonUtility.FromJson<PlayerList>(playerListJson);
            for(int i = 0; i < currPlayerList.playerList.Length; i++)
            {
                CreateAutoItem(i, currPlayerList.playerList[i].name, currPlayerList.playerList[i].score);
            }
        }
	}

    private void CreateAutoItem(int num, string name, int score)
    {
        GameObject currItem = Instantiate(listItem);
        currItem.transform.SetParent(parentCanvas.transform);
        currItem.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, -65 - num * 22, 0);
        Text[] info = currItem.GetComponentsInChildren<Text>();
        info[0].text = (num + 1).ToString();
        info[1].text = name;
        info[2].text = score.ToString();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("Start");
        }
    }
}
