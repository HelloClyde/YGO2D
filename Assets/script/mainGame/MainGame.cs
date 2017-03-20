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

    // Use this for initialization
    void Start () {
        this.oldTick = DateTime.Now.Ticks;
        // 生成双方牌组
        putCard(GameObject.Find("MyPanel/FeatureDeck1/MainDeck"), createCard(-1,false));
        putCard(GameObject.Find("EnemyPanel/FeatureDeck1/MainDeck"), createCard(-1,false));
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

    void putCard(GameObject contentObj, GameObject cardObj, int mode = 0)
    {
        cardObj.transform.SetParent(contentObj.transform);
        cardObj.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        cardObj.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        cardObj.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        cardObj.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
            contentObj.GetComponent<RectTransform>().rect.width);
        cardObj.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
            contentObj.GetComponent<RectTransform>().rect.width / 230 * 160);
        if (mode == 1)
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
                    case "HandAdd":
                        DoHandAdd(responseResult);
                        break;
                    case "HandSub":
                        DoHandSub(responseResult);
                        break;
                    case "MonsterAdd":
                        DoMonsterAdd(responseResult);
                        break;
                    case "MonsterSub":
                        DoMonsterSub(responseResult);
                        break;
                    case "CemeteryAdd":
                        DoCemeteryAdd(responseResult);
                        break;
                    case "CemeterySub":
                        DoCemeterySub(responseResult);
                        break;
                    case "MagicAdd":
                        DoMagicAdd(responseResult);
                        break;
                    case "MagicSub":
                        DoMagicSub(responseResult);
                        break;
                    case "HPChange":
                        DoHPChange(responseResult);
                        break;
                    case "GotoDP":
                        DoGotoDP();
                        break;
                    case "GotoSP":
                        DoGotoSP();
                        break;
                    case "GotoM1P":
                        DoGotoM1P();
                        break;
                    case "GotoBP":
                        DoGotoBP();
                        break;
                    case "GotoM2P":
                        DoGotoM2P();
                        break;
                    case "GotoEP":
                        DoGotoEP();
                        break;
                    case "TurnOperator":
                        DoTurnOperator(responseResult);
                        break;
                }
            }
        }
        else
        {
            MsgBox.showMsg((string)responseResult["data"]);
        }
    }

    private void DoTurnOperator(JsonData responseResult)
    {
        // 获取参数
        string email = (string)responseResult["data"]["email"];
        // 设置
        App.operateEmail = email;
    }

    private void DoGotoEP()
    {
        OperateTurnState("EP");
        selectMonster.clearSelectCursor();
    }

    private void DoGotoM2P()
    {
        OperateTurnState("M2P");
        selectMonster.clearSelectCursor();
    }

    private void DoGotoBP()
    {
        OperateTurnState("BP");
        App.selectLimit = 2;
    }

    private void DoGotoM1P()
    {
        OperateTurnState("M1P");
    }

    private void DoGotoSP()
    {
        OperateTurnState("SP");
    }

    private void DoGotoDP()
    {
        OperateTurnState("DP");
    }

    private void OperateTurnState(string turnState)
    {
        string[] allTurnState = { "DP", "SP", "M1P", "BP", "M2P", "EP" };
        // 设置标记
        App.TurnState = turnState;
        // 初始化全部控制器颜色
        for (int i = 0; i < allTurnState.Length; i++)
        {
            if (!allTurnState[i].Equals(turnState))
            {
                if (App.operateEmail != UserInfo.email)
                {
                    // 敌方回合红色
                    setButtonColor(GameObject.Find(allTurnState[i] + "Button"), new Color(255, 0, 0));
                }
                else
                {
                    // 我方回合白色
                    setButtonColor(GameObject.Find(allTurnState[i] + "Button"), new Color(255, 255, 255));
                }
            }
            else
            {
                // 设置目标控制器颜色
                setButtonColor(GameObject.Find(allTurnState[i] + "Button"), new Color(0, 255, 0));
            }
        }
    }

    private void setButtonColor(GameObject buttonObj, Color normalColor)
    {
        ColorBlock colorBlock = buttonObj.GetComponent<Button>().colors;
        colorBlock.normalColor = new Color(normalColor.r * 0.8f, normalColor.g * 0.8f, normalColor.b * 0.8f);
        colorBlock.highlightedColor = new Color(normalColor.r, normalColor.g, normalColor.b);
        colorBlock.pressedColor = new Color(normalColor.r * 0.5f, normalColor.g * 0.5f, normalColor.b * 0.5f);
        buttonObj.GetComponent<Button>().colors = colorBlock;
    }

    private void DoHPChange(JsonData responseResult)
    {
        // 获取参数
        string email = (string)responseResult["data"]["email"];
        int hpValue = (int)responseResult["data"]["paramsMap"]["HPValue"];
        GameObject hpObj;
        if (email == UserInfo.email)
        {
            hpObj = GameObject.Find("MyHP");
        }
        else
        {
            hpObj = GameObject.Find("EnemyHP");
        }
        // 获取原来的hp
        int oldHP = int.Parse(hpObj.GetComponent<Text>().text.Substring(3));
        // 更新HP
        hpObj.GetComponent<Text>().text = "HP:" + (oldHP + hpValue).ToString();
    }

    private void DoMagicSub(JsonData responseResult)
    {
        // 获取参数
        string email = (string)responseResult["data"]["email"];
        int cardIdx = (int)responseResult["data"]["paramsMap"]["CardIdx"];
        GameObject contentObj;
        if (email == UserInfo.email)
        {
            contentObj = GameObject.Find("MyPanel/DuelDeck/Magic/Magic" + cardIdx.ToString());
        }
        else
        {
            contentObj = GameObject.Find("EnemyPanel/DuelDeck/Magic/Magic" + cardIdx.ToString());
        }
        // 删除牌
        GameObject desCardObj = contentObj.transform.GetChild(0).gameObject;
        Destroy(desCardObj);
    }

    private void DoMagicAdd(JsonData responseResult)
    {
        // 获取参数
        string email = (string)responseResult["data"]["email"];
        int cardIdx = (int)responseResult["data"]["paramsMap"]["CardIdx"];
        int cardId = (int)responseResult["data"]["paramsMap"]["CardId"];
        int status = (int)responseResult["data"]["paramsMap"]["Status"];
        GameObject cardObj;
        GameObject contentObj;
        if (email == UserInfo.email)
        {
            if (status == 0)
            {
                cardObj = createCard(cardId, false);
            }
            else
            {
                cardObj = createCard(cardId, true);
            }
            contentObj = GameObject.Find("MyPanel/DuelDeck/Magic/Magic" + cardIdx.ToString());
        }
        else
        {
            if (status == 0)
            {
                cardObj = createCard(-1, false);
            }
            else
            {
                cardObj = createCard(cardId, true);
            }
            contentObj = GameObject.Find("EnemyPanel/DuelDeck/Magic/Magic" + cardIdx.ToString());
        }
        if (status == 0)
        {
            putCard(contentObj, cardObj);
        }
        else
        {
            putCard(contentObj, cardObj);
        }
    }

    private void DoCemeterySub(JsonData responseResult)
    {
        // 获取参数
        string email = (string)responseResult["data"]["email"];
        int cardIdx = (int)responseResult["data"]["paramsMap"]["CardIdx"];
        GameObject contentObj;
        if(email == UserInfo.email)
        {
            contentObj = GameObject.Find("MyPanel/FeatureDeck1/Cemetery");
        }
        else
        {
            contentObj = GameObject.Find("EnemyPanel/FeatureDeck1/Cemetery");
        }
        Destroy(contentObj.transform.GetChild(cardIdx).gameObject);
    }

    private void DoCemeteryAdd(JsonData responseResult)
    {
        // 获取参数
        string email = (string)responseResult["data"]["email"];
        int cardId = (int)responseResult["data"]["paramsMap"]["CardId"];
        GameObject contentObj;
        if (email == UserInfo.email)
        {
            contentObj = GameObject.Find("MyPanel/FeatureDeck1/Cemetery");
        }
        else
        {
            contentObj = GameObject.Find("EnemyPanel/FeatureDeck1/Cemetery");
        }
        // 清空原先内容
        /*
        for (int i = 0;i < contentObj.transform.childCount;i++)
        {
            Destroy(contentObj.transform.GetChild(i).gameObject);
        }
        */
        putCard(contentObj, createCard(cardId, true));
    }

    private void DoMonsterSub(JsonData responseResult)
    {
        // 获取参数
        string email = (string)responseResult["data"]["email"];
        int cardIdx = (int)responseResult["data"]["paramsMap"]["CardIdx"];
        GameObject contentObj;
        if (email == UserInfo.email)
        {
            contentObj = GameObject.Find("MyPanel/DuelDeck/Monster/Monster" + cardIdx.ToString());
        }
        else
        {
            contentObj = GameObject.Find("EnemyPanel/DuelDeck/Monster/Monster" + cardIdx.ToString());
        }
        // 删除牌
        GameObject desCardObj = contentObj.transform.GetChild(0).gameObject;
        Destroy(desCardObj);
    }

    private void DoMonsterAdd(JsonData responseResult)
    {
        // 获取参数
        string email = (string)responseResult["data"]["email"];
        int cardIdx = (int)responseResult["data"]["paramsMap"]["CardIdx"];
        int cardId = (int)responseResult["data"]["paramsMap"]["CardId"];
        int status = (int)responseResult["data"]["paramsMap"]["Status"];
        GameObject cardObj;
        GameObject contentObj;
        if (email == UserInfo.email)
        {
            if (status == 0)
            {
                cardObj = createCard(cardId, false);
            }else
            {
                cardObj = createCard(cardId, true);
            }
            cardObj.AddComponent<CardMenuScript>();
            cardObj.GetComponent<CardMenuScript>().menuPrefab = Resources.Load<GameObject>("prefab/MonsterStatusMenu");
            cardObj.GetComponent<CardMenuScript>().turnStates = new string[] { "M1P","M2P" };
            cardObj.AddComponent<selectMonster>();
            cardObj.AddComponent<selectOfferMonster>();
            contentObj = GameObject.Find("MyPanel/DuelDeck/Monster/Monster" + cardIdx.ToString());
        }
        else
        {
            if (status == 0)
            {
                cardObj = createCard(-1, false);
            }else
            {
                cardObj = createCard(cardId, true);
            }
            cardObj.AddComponent<selectMonster>();
            contentObj = GameObject.Find("EnemyPanel/DuelDeck/Monster/Monster" + cardIdx.ToString());
        }
        if (status == 0 || status == 1)
        {
            putCard(contentObj, cardObj, 1);
        }
        else
        {
            putCard(contentObj, cardObj);
        }
    }

    private void DoHandSub(JsonData responseResult)
    {
        // 获取参数
        string email = (string)responseResult["data"]["email"];
        int cardIdx = (int)responseResult["data"]["paramsMap"]["CardIdx"];
        GameObject handContentObj;
        if (email == UserInfo.email)
        {
            handContentObj = GameObject.Find("MHandPanel/Scroll View/Viewport/Content");
        }
        else
        {
            handContentObj = GameObject.Find("EHandPanel/Scroll View/Viewport/Content");
        }
        // 删除手牌
        GameObject desCardObj = handContentObj.transform.GetChild(cardIdx).gameObject;
        Destroy(desCardObj);
    }

    private void DoHandAdd(JsonData responseResult)
    {
        // 获取参数
        string email = (string)responseResult["data"]["email"];
        int cardId = (int)responseResult["data"]["paramsMap"]["CardId"];
        GameObject desCardObj;
        GameObject handContentObj;
        if (email == UserInfo.email)
        {
            desCardObj = createCard(cardId,true);
            desCardObj.AddComponent<CardMenuScript>();
            // 这里要做区分，判断是否是怪兽
            // 获取卡片信息
            CardInfo cardInfo = ShowCardInfo.getCardInfo(cardId);
            if (cardInfo.type.Contains("怪兽"))
            {
                desCardObj.GetComponent<CardMenuScript>().menuPrefab = Resources.Load<GameObject>("prefab/HandMonsterMenu");
            }else if (cardInfo.type.Contains("魔法"))
            {
                desCardObj.GetComponent<CardMenuScript>().menuPrefab = Resources.Load<GameObject>("prefab/HandMagicMenu");
            }
            else if (cardInfo.type.Contains("陷阱"))
            {
                desCardObj.GetComponent<CardMenuScript>().menuPrefab = Resources.Load<GameObject>("prefab/HandTrapMenu");
            }
            desCardObj.GetComponent<CardMenuScript>().turnStates = new string[] { "M1P", "M2P" };
            handContentObj = GameObject.Find("MHandPanel/Scroll View/Viewport/Content");
        }
        else
        {
            desCardObj = createCard(-1,false);
            handContentObj = GameObject.Find("EHandPanel/Scroll View/Viewport/Content");
        }
        desCardObj.transform.SetParent(handContentObj.transform);
    }

    private GameObject createCard(int cardId,bool isShow)
    {
        GameObject cardPrefab = (GameObject)Resources.Load("prefab/CardPrefab");
        GameObject cardObject = Instantiate(cardPrefab);
        cardObject.GetComponent<ShowCardInfo>().cardInfoImageObj = GameObject.Find("InfoPanel/CardInfoPanel/CardImage");
        cardObject.GetComponent<ShowCardInfo>().cardInfoTextObj = GameObject.Find("InfoPanel/CardInfoPanel/Scroll View/Viewport/Content/Text");
        cardObject.GetComponent<ShowCardInfo>().cardId = cardId;
        if (!isShow)
        {
            cardObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("image/CardBack");
        }
        else
        {
            cardObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("image/CardImage/" + cardId.ToString());
        }
        return cardObject;
    }

    
}

