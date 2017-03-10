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

    private void NoParamsActionRequest(string action)
    {
        Dictionary<string, object> paramsMap = new Dictionary<string, object>();
        paramsMap.Add("token", UserInfo.token);
        paramsMap.Add("action", action);
        string response = HttpClient.sendPost(App.serverPath + "YgoService/duel-controller/action",
            paramsMap);
        JsonData responseResult = JsonMapper.ToObject(response);
        if ((int)responseResult["code"] != 0)
        {
            GUIOp.showMsg((string)responseResult["data"]);
        }
    }

    public void LoseAndExit()
    {
        // 发送退出房间请求
        NoParamsActionRequest("GiveUp");
        // TODO 之后将通过结果界面退出
        SceneManager.LoadScene("Menu");
    }

    public void GoToBP()
    {
        if (App.operateEmail == UserInfo.email && App.TurnState.Equals("M1P"))
        {
            NoParamsActionRequest("GotoBP");
        }
    }

    public void GoToM2P()
    {
        if (App.operateEmail == UserInfo.email &&
            (App.TurnState.Equals("M1P") || App.TurnState.Equals("BP")))
        {
            NoParamsActionRequest("GotoM2P");
        }
    }

    public void GoToEP()
    {
        if (App.operateEmail == UserInfo.email)
        {
            NoParamsActionRequest("GotoEP");
            TurnOperate();
        }
    }

    public void TurnOperate()
    {
        NoParamsActionRequest("TurnOperator");
    }
}
