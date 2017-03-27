using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets.script.utils;
using Assets.script.common;
using LitJson;

class HandMonsterMenu : MenuAction
{
    public void CallMonster()
    {
        // 获取卡片信息
        CardInfo cardInfo = ShowCardInfo.getCardInfo(this.operateCardObj.GetComponent<ShowCardInfo>().cardId);
        if (cardInfo.starNum <= 4) {
            // 普通召唤
            CallMonsterRequest(2);
        }
        else if (cardInfo.starNum > 4 && cardInfo.starNum <= 6)
        {
            if (existMyMonster())
            {
                // 一个祭品召唤
                App.isOfferState = true;
                App.selectLimit = 1;
                App.handIdx = this.operateCardObj.transform.GetSiblingIndex();
                App.monsterStatus = 2;
            }else
            {
                MsgBox.showMsg("场上没有祭品");
            }
        }else 
        {
            if (existMyMonster())
            {
                // 两个祭品召唤
                App.isOfferState = true;
                App.selectLimit = 2;
                App.handIdx = this.operateCardObj.transform.GetSiblingIndex();
                App.monsterStatus = 2;
            }
            else
            {
                MsgBox.showMsg("场上没有祭品");
            }
        }
        closeMyself();
    }

    private bool existMyMonster()
    {
        GameObject myMonsterPanel = GameObject.Find("MyPanel/DuelDeck/Monster");
        for (int i = 0;i < myMonsterPanel.transform.childCount;i++)
        {
            if (myMonsterPanel.transform.GetChild(i).childCount != 0)
            {
                return true;
            }
        }
        return false;
    }

    public void PutMonster()
    {
        // 获取卡片信息
        CardInfo cardInfo = ShowCardInfo.getCardInfo(this.operateCardObj.GetComponent<ShowCardInfo>().cardId);
        if (cardInfo.starNum <= 4)
        {
            // 普通召唤
            CallMonsterRequest(0);
        }
        else if (cardInfo.starNum > 4 && cardInfo.starNum <= 6)
        {
            // 一个祭品召唤
            App.isOfferState = true;
            App.selectLimit = 1;
            App.handIdx = this.operateCardObj.transform.GetSiblingIndex();
            App.monsterStatus = 0;
            Debug.Log("一个祭品召唤");
        }
        else
        {
            // 两个祭品召唤
            App.isOfferState = true;
            App.selectLimit = 2;
            App.handIdx = this.operateCardObj.transform.GetSiblingIndex();
            App.monsterStatus = 0;
            Debug.Log("两个祭品召唤");
        }
        closeMyself();
    }

    private void CallMonsterRequest(int monsterStatus)
    {
        Dictionary<string, object> paramsMap = new Dictionary<string, object>();
        paramsMap.Add("token", UserInfo.token);
        paramsMap.Add("action", "CallNormalMonster");
        paramsMap.Add("HandCardIdx", this.operateCardObj.transform.GetSiblingIndex().ToString());
        paramsMap.Add("Status", monsterStatus.ToString());
        string response = HttpClient.sendPost(App.serverPath + "YgoService/duel-controller/action",
            paramsMap);
        JsonData responseResult = JsonMapper.ToObject(response);
        if ((int)responseResult["code"] != 0)
        {
            MsgBox.showMsg((string)responseResult["data"]);
        }
    }
}

