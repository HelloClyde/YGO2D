using UnityEngine;
using System.Collections;
using Assets.script.utils;
using System;
using LitJson;
using Assets.script.common;
using System.Collections.Generic;
using UnityEngine.UI;

public class RoomAction : MonoBehaviour {
    public int roomIdx = -1;
    public JsonData roomData = null;

    // 使用延迟
    private long duration;// -1表示上一次在该部件外
    public long durationLimit = (long)(0.2 * 10000000); // 1s延迟就触发

    // Use this for initialization
    void Start () {
        this.duration = -1;
    }
	
	// Update is called once per frame
	void Update () {
        if (GUIOp.isInGUI(Input.mousePosition, this.gameObject))
        {
            if (this.duration == -1)
            {
                this.duration = DateTime.Now.Ticks;
            }
            else
            {
                if (DateTime.Now.Ticks > this.duration + this.durationLimit)
                {
                    showRoomInfo();
                }
            }
        }
        else
        {
            this.duration = -1;
        }
    }

    private void showRoomInfo()
    {
        int playerNum = this.roomData["playerInfos"].Count;
        if (playerNum == 2)
        {
            GameObject.Find("PlayerA/Text").GetComponent<Text>().text =
                playerInfoToString(this.roomData["playerInfos"][0]);
            GameObject.Find("PlayerB/Text").GetComponent<Text>().text =
                playerInfoToString(this.roomData["playerInfos"][1]);
        }
        else if (playerNum == 1)
        {
            GameObject.Find("PlayerA/Text").GetComponent<Text>().text =
                playerInfoToString(this.roomData["playerInfos"][0]);
        }
        else
        {
            GameObject.Find("PlayerA/Text").GetComponent<Text>().text = "暂无信息";
            GameObject.Find("PlayerB/Text").GetComponent<Text>().text = "暂无信息";
        }
    }

    private string playerInfoToString(JsonData playerData)
    {
        string userName = (string)playerData["userName"];
        int winGame = (int)playerData["winGame"];
        int allGame = (int)playerData["allGame"];
        string winRate = allGame == 0 ? "--" : ((int)(winGame * 100 / allGame)).ToString() + "%";
        return 
            "用户名：" + userName + "\n" + 
            "胜场：" + winGame + "\n" +
            "总场数：" + allGame + "\n" +
            "胜率：" + winRate;
    }

    public void roomClickAction()
    {
        if (this.roomContainMe())
        {
            exitRoom();
        }else
        {
            joinRoom();
        }
    }

    private void joinRoom()
    {
        Dictionary<string, object> paramsMap = new Dictionary<string, object>();
        paramsMap.Add("roomIdx", this.roomIdx);
        paramsMap.Add("token", UserInfo.token);
        string response = HttpClient.sendGet(App.serverPath + "YgoService/hall/join-room", paramsMap);
        JsonData responseResult = JsonMapper.ToObject(response);
        if ((int)responseResult["code"] == 0)
        {
            Debug.Log("join Room" + this.roomIdx.ToString());
        }
        else
        {
            MsgBox.showMsg((string)responseResult["data"]);
        }
    }

    private void exitRoom()
    {
        Dictionary<string, object> paramsMap = new Dictionary<string, object>();
        paramsMap.Add("token", UserInfo.token);
        string response = HttpClient.sendGet(App.serverPath + "YgoService/hall/exit-room", paramsMap);
        JsonData responseResult = JsonMapper.ToObject(response);
        if ((int)responseResult["code"] == 0)
        {
            Debug.Log("exit Room" + this.roomIdx.ToString());
        }
        else
        {
            MsgBox.showMsg((string)responseResult["data"]);
        }
    }

    public bool roomContainMe()
    {
        JsonData jsonData = this.roomData;
        for (int i = 0; i < jsonData["playerInfos"].Count; i++)
        {
            if ((string)jsonData["playerInfos"][i]["userName"] == UserInfo.email)
            {
                return true;
            }
        }
        return false;
    }
}
