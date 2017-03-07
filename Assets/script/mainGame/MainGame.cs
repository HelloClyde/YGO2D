using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Assets.script.common;
using LitJson;
using UnityEngine.UI;

public class MainGame : MonoBehaviour {
    private long oldTick;
    private static long tickDelta = (long)(0.5 * 10000000);// 单位是100毫微秒，即2s更新一次
    public GameObject menuPanelObj;

    // Use this for initialization
    void Start () {
        this.oldTick = DateTime.Now.Ticks;
        this.menuPanelObj = GameObject.Find("MenuPanel");
        // 隐藏menuPanelObj
        this.menuPanelObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // 定时获取服务器游戏数据
        if (DateTime.Now.Ticks > this.oldTick + MainGame.tickDelta)
        {
            GetServiceGameLog();
        }
    }
    
    public void GetServiceGameLog()
    {
        Dictionary<string, object> paramsMap = new Dictionary<string, object>();
        paramsMap.Add("token", UserInfo.token);
        string response = HttpClient.sendGet("http://localhost:8080/YgoService/duel-controller/get-inc-log",
            paramsMap);
        JsonData responseResult = JsonMapper.ToObject(response);
        if ((int)responseResult["code"] == 0)
        {
            if (responseResult["data"] != null)
            {
                //responseResult.data.
                Debug.Log(JsonMapper.ToJson(responseResult));
                if ((string)responseResult["data"]["action"] == "DrawCard")
                {
                    GameObject handContent;
                    string imagePath;
                    string cardId = (string)responseResult["data"]["paramsMap"]["CardId"];
                    GameObject cardPrefab = (GameObject)Resources.Load("fab/CardPrefab");
                    GameObject cardObject = Instantiate(cardPrefab);
                    cardObject.name = "card" + cardId;
                    cardObject.GetComponent<ShowCardInfo>().cardId = int.Parse(cardId);
                    cardObject.GetComponent<ShowCardInfo>().cardInfoImageObj = GameObject.Find("InfoPanel/CardInfoPanel/CardImage");
                    cardObject.GetComponent<ShowCardInfo>().cardInfoTextObj = GameObject.Find("InfoPanel/CardInfoPanel/Scroll View/Viewport/Content/Text");
                    cardObject.AddComponent<CardMenuScript>();
                    // 判断是敌方还是我方
                    if ((string)responseResult["data"]["email"] == UserInfo.email)
                    {
                        handContent = GameObject.Find("MHandPanel/Scroll View/Viewport/Content");
                        imagePath = "image/CardImage/" + cardId;
                    }
                    else
                    {
                        handContent = GameObject.Find("EHandPanel/Scroll View/Viewport/Content");
                        imagePath = "image/CardBack";
                    }
                    cardObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(imagePath);
                    cardObject.transform.SetParent(handContent.transform);
                }
            }
            this.oldTick = DateTime.Now.Ticks;
        }
        else
        {
            Debug.Log("连接错误");
        }
    }
}

