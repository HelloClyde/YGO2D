using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Assets.script.common;
using LitJson;
using UnityEngine.UI;
using Assets.script.utils;

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
        // 生成双方牌组
        GameObject cardPrefab = (GameObject)Resources.Load("fab/CardPrefab");
        GameObject myCardObj = Instantiate(cardPrefab);
        GameObject enemyCardObj = Instantiate(cardPrefab);
        // 获取目标位置
        GameObject myDeckObj = GameObject.Find("MyPanel/FeatureDeck1/MainDeck");
        GameObject enemyDeckObj = GameObject.Find("EnemyPanel/FeatureDeck1/MainDeck");
        // 删除牌组中ShowCardInfo组件
        Destroy(myCardObj.GetComponent<ShowCardInfo>());
        Destroy(enemyCardObj.GetComponent<ShowCardInfo>());
        // 放置在相应位置
        PutCard(myDeckObj, myCardObj);
        PutCard(enemyDeckObj, enemyCardObj);
    }

    // Update is called once per frame
    void Update()
    {
        // 定时获取服务器游戏数据
        if (DateTime.Now.Ticks > this.oldTick + MainGame.tickDelta)
        {
            GetServiceGameLog();
            this.oldTick = DateTime.Now.Ticks;
        }
    }

    void PutCard(GameObject contentObj, GameObject cardObj, int mode = 2)
    {
        cardObj.transform.SetParent(contentObj.transform);
        cardObj.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        cardObj.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        cardObj.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        cardObj.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
            contentObj.GetComponent<RectTransform>().rect.width);
        cardObj.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
            contentObj.GetComponent<RectTransform>().rect.width / 230 * 160);
        if (mode == 0 || mode == 1)
        {
            cardObj.GetComponent<RectTransform>().Rotate(new Vector3(0,0,90));
        }
    }
    
    public void GetServiceGameLog()
    {
        Dictionary<string, object> paramsMap = new Dictionary<string, object>();
        paramsMap.Add("token", UserInfo.token);
        string response = HttpClient.sendGet(App.serverPath + "YgoService/duel-controller/get-inc-log",
            paramsMap);
        JsonData responseResult = JsonMapper.ToObject(response);
        if ((int)responseResult["code"] == 0)
        {
            // 非空表示有新消息
            if (responseResult["data"] != null)
            {
                Debug.Log(JsonMapper.ToJson(responseResult));
                string action = (string)responseResult["data"]["action"];
                switch (action)
                {
                    case "DrawCard":
                        DoDrawCard(responseResult);
                        break;
                    case "CallMonsterFromHand":
                        DoCallMonsterFromHand(responseResult);
                        break;
                }
            }
        }
        else
        {
            GUIOp.showMsg((string)responseResult["data"]);
        }
    }

    public void DoDrawCard(JsonData responseResult)
    {
        GameObject handContent;
        string imagePath;
        GameObject cardPrefab = (GameObject)Resources.Load("fab/CardPrefab");
        GameObject cardObject = Instantiate(cardPrefab);
        cardObject.GetComponent<ShowCardInfo>().cardInfoImageObj = GameObject.Find("InfoPanel/CardInfoPanel/CardImage");
        cardObject.GetComponent<ShowCardInfo>().cardInfoTextObj = GameObject.Find("InfoPanel/CardInfoPanel/Scroll View/Viewport/Content/Text");
        cardObject.AddComponent<CardMenuScript>();
        // 判断是敌方还是我方
        if ((string)responseResult["data"]["email"] == UserInfo.email)
        {
            string cardId = (string)responseResult["data"]["paramsMap"]["CardId"];
            cardObject.GetComponent<ShowCardInfo>().cardId = int.Parse(cardId);
            handContent = GameObject.Find("MHandPanel/Scroll View/Viewport/Content");
            imagePath = "image/CardImage/" + cardId;
        }
        else
        {
            cardObject.GetComponent<ShowCardInfo>().cardId = -1;
            handContent = GameObject.Find("EHandPanel/Scroll View/Viewport/Content");
            imagePath = "image/CardBack";
        }
        cardObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(imagePath);
        cardObject.transform.SetParent(handContent.transform);
    }

    public void DoCallMonsterFromHand(JsonData responseResult)
    {
        int handCardIdx = int.Parse((string)responseResult["data"]["paramsMap"]["HandCardIdx"]);
        int monsterStatus = int.Parse((string)responseResult["data"]["paramsMap"]["MonsterStatus"]);
        int monsterCardIdx = int.Parse((string)responseResult["data"]["paramsMap"]["MonsterCardIdx"]);
        GameObject HandContentObj;
        string monsterContentPath;
        // 场上生成怪兽
        GameObject cardPrefab = (GameObject)Resources.Load("fab/CardPrefab");
        GameObject cardObject = Instantiate(cardPrefab);
        cardObject.GetComponent<ShowCardInfo>().cardInfoImageObj = GameObject.Find("InfoPanel/CardInfoPanel/CardImage");
        cardObject.GetComponent<ShowCardInfo>().cardInfoTextObj = GameObject.Find("InfoPanel/CardInfoPanel/Scroll View/Viewport/Content/Text");
        // 判断是敌方还是我方
        if ((string)responseResult["data"]["email"] == UserInfo.email)
        {
            int cardId = int.Parse((string)responseResult["data"]["paramsMap"]["CardId"]);
            HandContentObj = GameObject.Find("MHandPanel/Scroll View/Viewport/Content");
            monsterContentPath = "MyPanel/DuelDeck/Monster/Monster";
            cardObject.GetComponent<ShowCardInfo>().cardId = cardId;
            if (monsterStatus == 0)
            {
                cardObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("image/CardBack");
            }else
            {
                cardObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("image/CardImage/" + cardId.ToString());
            }
        }
        else
        {
            HandContentObj = GameObject.Find("EHandPanel/Scroll View/Viewport/Content");
            monsterContentPath = "EnemyPanel/DuelDeck/Monster/Monster";
            cardObject.GetComponent<ShowCardInfo>().cardId = -1;
            if (monsterStatus == 0)
            {
                cardObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("image/CardBack");
            }else
            {
                int cardId = int.Parse((string)responseResult["data"]["paramsMap"]["CardId"]);
                cardObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("image/CardImage/" + cardId.ToString());
            }
        }
        // 删除那张手牌
        GameObject desHandCardObj = HandContentObj.transform.GetChild(handCardIdx).gameObject;
        Destroy(desHandCardObj);
        GameObject monsterContent = GameObject.Find(monsterContentPath + monsterCardIdx.ToString());
        PutCard(monsterContent, cardObject, monsterStatus);
    }
}

