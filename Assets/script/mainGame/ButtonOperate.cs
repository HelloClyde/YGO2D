using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Assets.script.common;
using LitJson;
using Assets.script.utils;

public class ButtonOperate : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LoseAndExit()
    {
        // 发送退出房间请求
        Dictionary<string, object> paramsMap = new Dictionary<string, object>();
        paramsMap.Add("token", UserInfo.token);
        string response = HttpClient.sendGet(App.serverPath + "YgoService/hall/join-room",
            paramsMap);
        JsonData jsonData = JsonMapper.ToObject(response);
        if ((int)jsonData["code"] != 0)
        {
            GUIOp.showMsg((string)jsonData["data"]);
        }
        SceneManager.LoadScene("Menu");
    }

    public void GoToBP()
    {
        Dictionary<string, object> paramsMap = new Dictionary<string, object>();
        paramsMap.Add("token", UserInfo.token);
        paramsMap.Add("action", "GoBP");
        string response = HttpClient.sendPost(App.serverPath + "YgoService/duel-controller/action",
            paramsMap);
        JsonData jsonData = JsonMapper.ToObject(response);
        if ((int)jsonData["code"] != 0)
        {
            GUIOp.showMsg((string)jsonData["data"]);
        }
    }

    public void GoToM2P()
    {
        Dictionary<string, object> paramsMap = new Dictionary<string, object>();
        paramsMap.Add("token", UserInfo.token);
        paramsMap.Add("action", "GoM2P");
        string response = HttpClient.sendPost(App.serverPath + "YgoService/duel-controller/action",
            paramsMap);
        JsonData jsonData = JsonMapper.ToObject(response);
        if ((int)jsonData["code"] != 0)
        {
            GUIOp.showMsg((string)jsonData["data"]);
        }
    }

    public void GoToEP()
    {
        Dictionary<string, object> paramsMap = new Dictionary<string, object>();
        paramsMap.Add("token", UserInfo.token);
        paramsMap.Add("action", "GoEP");
        string response = HttpClient.sendPost(App.serverPath + "YgoService/duel-controller/action",
            paramsMap);
        JsonData jsonData = JsonMapper.ToObject(response);
        if ((int)jsonData["code"] != 0)
        {
            GUIOp.showMsg((string)jsonData["data"]);
        }
    }
}
