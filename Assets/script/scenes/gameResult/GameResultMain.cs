using Assets.script.common;
using LitJson;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameResultMain : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject imageObj = GameObject.Find("ResultImage");
	    if (App.gameResult == 0)
        {
            // 隐藏losePanel
            GameObject.Find("LosePanel").SetActive(false);
        }
        else
        {
            // 隐藏winPanel
            GameObject.Find("WinPanel").SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void returnMainMenu()
    {
        // 退出房间
        Dictionary<string, object> paramsMap = new Dictionary<string, object>();
        paramsMap.Add("token", UserInfo.token);
        string response = HttpClient.sendGet(App.serverPath + "YgoService/hall/exit-room", paramsMap);
        JsonData responseResult = JsonMapper.ToObject(response);
        if ((int)responseResult["code"] == 0)
        {
            Debug.Log("exit Room" + responseResult["data"].ToString());
        }
        else
        {
            MsgBox.showMsg((string)responseResult["data"]);
        }
        SceneManager.LoadScene("MainMenu");
    }
}
